using PhantomInterop.Classes.Api;
using PhantomInterop.Classes.Events;
using PhantomInterop.Interfaces;
using PhantomInterop.Structs.PhantomStructs;
using System;

namespace PhantomInterop.Classes.Core
{
    public abstract class Process : IProcess
    {
        public string Application { get; protected set; }
        public string CommandLine { get; protected set; }
        protected bool _startSuspended;
        public bool HasExited { get; protected set; }
        public int ExitCode { get; protected set; }
        public uint PID { get; protected set; }
        public string StdOut { get; protected set; } = "";
        public string StdErr { get; protected set; } = "";
        public IntPtr Handle { get; protected set; }
        protected IAgent _agent;
        private delegate ulong RtlNtStatusToDosError(int status);
        public event EventHandler<StringDataEventArgs> OutputDataReceived;
        public event EventHandler<StringDataEventArgs> ErrorDataReceieved;
        public event EventHandler Exit;

        public void RobustAdapter9474(object sender, StringDataEventArgs args)
        {
            OutputDataReceived?.Invoke(sender, args);
        }

        public void PhantomDispatcherAD2F(object sender, StringDataEventArgs args)
        {
            ErrorDataReceieved?.Invoke(sender, args);
        }

        public abstract void Kill();

        public void GhostWorkerA723(object sender, EventArgs args)
        {
            Exit?.Invoke(sender, args);
        }
        public Process(IAgent agent, string lpApplication, string lpArguments = null, bool startSuspended = false)
        {
            _agent = agent;
            if (ValidationHelper.IsStringEmpty(lpApplication) && ValidationHelper.IsStringEmpty(lpArguments))
            {
                throw new Exception("Application and arguments cannot be null.");
            }
            if (ValidationHelper.IsStringEmpty(lpArguments))
            {
                CommandLine = lpApplication;
                Application = lpApplication;
            }
            else if (ValidationHelper.IsStringEmpty(lpApplication))
            {
                CommandLine = lpArguments;
            }
            else
            {
                Application = lpApplication;
                CommandLine = $"{lpApplication} {lpArguments}";
            }
            _startSuspended = startSuspended;
        }

        public int? GetExitCodeHResult()
        {
            const uint HRESULT_MASK = 0x80070000;

            if (!HasExited)
            {
                return null;
            }

            var rtlNtStatusToDosError = _agent.GetApi().GetLibraryFunction<RtlNtStatusToDosError>(Library.NTDLL, "RtlNtStatusToDosError");
            if (rtlNtStatusToDosError == null)
            {
                return null;
            }

            return unchecked((int)(rtlNtStatusToDosError(ExitCode) | HRESULT_MASK));
        }

        public abstract bool Inject(byte[] code, string arguments = "");

        public abstract bool Start();

        public abstract bool StartWithCredentials(PhantomLogonInformation logonInfo);

        public abstract bool StartWithCredentials(IntPtr hToken);

        public abstract void WaitForExit();

        public abstract void WaitForExit(int milliseconds);
    }
}
