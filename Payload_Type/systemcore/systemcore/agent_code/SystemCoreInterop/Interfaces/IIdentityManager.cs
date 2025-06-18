using SystemAgentInterop.Structs.SystemCoreStructs;
using SystemAgentInterop.Structs.MythicStructs;
using System;
using System.Security.Principal;

namespace SystemAgentInterop.Interfaces
{
    public interface IIdentityManager
    {
        WindowsIdentity GetCurrentPrimaryIdentity();
        WindowsIdentity GetCurrentImpersonationIdentity();
        WindowsIdentity GetOriginal();

        bool GetCurrentLogonInformation(out SystemCoreLogonInformation logonInfo);

        void Revert();

        void SetPrimaryIdentity(WindowsIdentity identity);

        void SetPrimaryIdentity(IntPtr hToken);

        void SetImpersonationIdentity(WindowsIdentity identity);
        void SetImpersonationIdentity(IntPtr hToken);

        bool SetIdentity(SystemCoreLogonInformation token);

        IntegrityLevel GetIntegrityLevel();

        bool IsOriginalIdentity();

        (bool,IntPtr) GetSystem();

    }
}
