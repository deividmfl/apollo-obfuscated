
using System;
using System.IO;
using System.Management;
using System.Diagnostics;
using Microsoft.Win32;

namespace SystemAgent.Stealth
{
    public static class SystemIntegrityValidator
    {
        public static bool ValidateEnvironment()
        {
            return !IsVirtualMachine() && !IsSandboxEnvironment() && !IsAnalysisToolPresent();
        }
        
        private static bool IsVirtualMachine()
        {
            string[] vmIndicators = {
                "VBOX", "VMWARE", "QEMU", "VIRTUAL", "XEN", "HYPER-V"
            };
            
            foreach (string indicator in vmIndicators)
            {
                if (Environment.MachineName.ToUpper().Contains(indicator) ||
                    Environment.UserName.ToUpper().Contains(indicator))
                    return true;
            }
            
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        string manufacturer = obj["Manufacturer"]?.ToString()?.ToUpper() ?? "";
                        string model = obj["Model"]?.ToString()?.ToUpper() ?? "";
                        
                        if (manufacturer.Contains("VMWARE") || manufacturer.Contains("VBOX") ||
                            model.Contains("VIRTUAL") || model.Contains("VMWARE"))
                            return true;
                    }
                }
            }
            catch { }
            
            return false;
        }
        
        private static bool IsSandboxEnvironment()
        {
            // Check system resources
            if (Environment.ProcessorCount < 2) return true;
            if (GC.GetTotalMemory(false) < 2000000000) return true;
            
            // Check disk space
            try
            {
                DriveInfo systemDrive = new DriveInfo(Environment.SystemDirectory);
                if (systemDrive.TotalSize < 50000000000) return true; // Less than 50GB
            }
            catch { }
            
            return false;
        }
        
        private static bool IsAnalysisToolPresent()
        {
            string[] analysisTools = {
                "wireshark", "fiddler", "procmon", "regmon", "ollydbg",
                "ida", "ghidra", "x64dbg", "immunity", "windbg"
            };
            
            try
            {
                foreach (var process in Process.GetProcesses())
                {
                    foreach (string tool in analysisTools)
                    {
                        if (process.ProcessName.ToLower().Contains(tool))
                            return true;
                    }
                }
            }
            catch { }
            
            return false;
        }
    }
}
