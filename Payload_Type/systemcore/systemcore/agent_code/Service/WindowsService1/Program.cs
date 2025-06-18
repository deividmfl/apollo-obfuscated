using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace WindowsService1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
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
            
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
