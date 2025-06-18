using SystemAgentInterop.Structs.SystemCoreStructs;
using System;

namespace SystemAgentInterop.Interfaces
{
    public interface IProcess
    {
        bool Inject(byte[] code, string arguments = "");
        void WaitForExit();
        void WaitForExit(int milliseconds);

        bool Start();
        bool StartWithCredentials(SystemCoreLogonInformation logonInfo);

        bool StartWithCredentials(IntPtr hToken);

    }
}
