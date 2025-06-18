
using System;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace SystemAgent.Stealth
{
    public static class HardwareProfileValidator
    {
        public static bool ValidateConfiguration()
        {
            return ValidateProcessorInfo() && ValidateMemoryInfo() && ValidateNetworkInfo();
        }
        
        private static bool ValidateProcessorInfo()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        string name = obj["Name"]?.ToString()?.ToUpper() ?? "";
                        
                        // Check for VM-specific processor names
                        if (name.Contains("VIRTUAL") || name.Contains("VMWARE"))
                            return false;
                    }
                }
            }
            catch { }
            
            return true;
        }
        
        private static bool ValidateMemoryInfo()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        ulong totalMemory = Convert.ToUInt64(obj["TotalPhysicalMemory"]);
                        
                        // Less than 2GB indicates potential sandbox
                        if (totalMemory < 2147483648) return false;
                    }
                }
            }
            catch { }
            
            return true;
        }
        
        private static bool ValidateNetworkInfo()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter"))
                {
                    int adapterCount = 0;
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        string name = obj["Name"]?.ToString()?.ToUpper() ?? "";
                        if (!string.IsNullOrEmpty(name) && !name.Contains("LOOPBACK"))
                            adapterCount++;
                    }
                    
                    // Too few network adapters might indicate sandbox
                    return adapterCount > 0;
                }
            }
            catch { }
            
            return true;
        }
    }
}
