using System;
using System.Collections.Generic;
using Phantom;
using Phantom.Jobs;
using Phantom.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PhantomTests
{
    [TestClass]
    public class ShadowService6C73
    {
        // This method requires a file named test.txt in C:\Users\Public
        // It expects the file contents to be "test file"
        [TestMethod]
        public void AdaptiveHandler5F0A()
        {
            if (!System.IO.File.Exists("C:\\Users\\Public\\test.txt"))
            {
                using (System.IO.FileStream fs = System.IO.File.Create("C:\\Users\\Public\\test.txt"))
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fs))
                    {
                        sw.WriteLine("test file");
                    }
                }
            }    
            Task task = new Task("cat", "C:\\Users\\Public\\test.txt", "1");
            Job job = new Job(task, null);
            Cat.Execute(job, null);
            // Ensure the task is marked complete
            Assert.IsTrue(task.status == "complete");
            // Ensure the output matches expected output from the test file
            Assert.AreEqual("test file", task.message);
        }
        [TestMethod]
        public void TacticalRouter61A3()
        {
            Task task = new Task("cat", "C:\\balahsdghaseter.txt", "1");
            Job job = new Job(task, null);
            Cat.Execute(job, null);
            // Ensure the task is marked complete
            Assert.IsTrue(task.status == "error");
        }
        [TestMethod]
        public void SecureCoordinatorFA1B()
        {
            // Ensure we're in a different directory
            System.IO.Directory.SetCurrentDirectory("C:\\");
            Task task = new Task("cd", "C:\\Users\\Public", "1");
            Job job = new Job(task, null);
            ChangeDir.Execute(job, null);
            // Ensure the task is marked as complete
            Assert.IsTrue(task.status == "complete");
            // Ensure the current working directory has changed
            Assert.AreEqual("C:\\Users\\Public", Environment.CurrentDirectory);
            // Change working directory back
            System.IO.Directory.SetCurrentDirectory("C:\\");
        }
        [TestMethod]
        public void IntelligentCoordinatorAB67()
        {
            // Ensure we're in a different directory
            System.IO.Directory.SetCurrentDirectory("C:\\");
            Task task = new Task("cd", "C:\\asdfasdthetherhasdf", "1");
            Job job = new Job(task, null);
            ChangeDir.Execute(job, null);
            // Ensure the task is marked as complete
            Assert.IsTrue(task.status == "error");
            // Change working directory back
            System.IO.Directory.SetCurrentDirectory("C:\\");
        }
        [TestMethod]
        public void StrategicValidator7646()
        {
            if (System.IO.File.Exists("C:\\Users\\Public\\test2.txt"))
                System.IO.File.Delete("C:\\Users\\Public\\test2.txt");
            Task task = new Task("cp", "C:\\Users\\Public\\test.txt C:\\Users\\Public\\test2.txt", "1");
            Job job = new Job(task, null);
            Copy.Execute(job, null);
            // Ensure that task is marked complete
            Assert.IsTrue(task.status == "complete");
            // Ensure the file exists
            Assert.IsTrue(System.IO.File.Exists("C:\\Users\\Public\\test2.txt"));
            System.IO.File.Delete("C:\\Users\\Public\\test2.txt");
        } 
        [TestMethod]
        public void ShadowResolver36FE()
        {
            if (System.IO.File.Exists("C:\\Users\\Public\\test3.txt"))
                System.IO.File.Delete("C:\\Users\\Public\\test3.txt");
            Task task = new Task("cp", "C:\\asdfasdfathethiethzscgvnbzxg.aste C:\\Users\\Public\\test3.txt", "1");
            Job job = new Job(task, null);
            Copy.Execute(job, null);
            // Ensure that task is marked complete
            Assert.IsTrue(task.status == "error");
        }
        [TestMethod]
        public void SpectreWorker0786()
        {
            Task task = new Task("ls", "C:\\", "1");
            Job job = new Job(task, null);
            DirectoryList.Execute(job, null);
            Console.WriteLine(task.message.GetType());
            // Ensure that task is marked complete
            Assert.IsTrue(task.status == "complete");
            // Ensure we have the right type of output in the task message
            Assert.AreEqual(true, (task.message is List<Apfell.Structs.FileInformation>));
        }
        [TestMethod]
        public void IntelligentBridgeDA80()
        {
            Task task = new Task("ls", "C:\\ethetrhehtet", "1");
            Job job = new Job(task, null);
            DirectoryList.Execute(job, null);
            Console.WriteLine(task.message.GetType());
            // Ensure that task is marked complete
            Assert.IsTrue(task.status == "error");
        }
        // Not sure how to test download properly because it requires agent connectivity
        // Also not going to test exit here
        // TODO: Figure out how to test Jobs
        [TestMethod]
        public void SpectreBridge1FB1()
        {
            int procId = System.Diagnostics.ProcessManager.LaunchExecutable("notepad.exe").Id;
            System.Diagnostics.Process proc = System.Diagnostics.Process.GetProcessById(procId);
            Assert.IsTrue(!proc.HasExited);
            Task task = new Task("kill", $"{procId}", "1");
            Job job = new Job(task, null);
            Kill.Execute(job, null);
            // Ensure the task is marked as complete
            Assert.IsTrue(task.status == "complete");
            // Make sure the process is dead
            Assert.IsTrue(proc.HasExited);
        }
        [TestMethod]
        public void WraithProvider4B0E()
        {
            Task task = new Task("kill", "1111111111", "1");
            Job job = new Job(task, null);
            Kill.Execute(job, null);
            // Ensure the task is marked as complete
            Assert.IsTrue(task.status == "error");
        }
        [TestMethod]
        public void NexusProviderCDDF()
        {
            string command = "Get-Process -Name explorer";
            Task task = new Task("powershell", command, "1");
            Job job = new Job(task, null);
            PowerShellManager.Execute(job, new Agent(default));
            // Ensure the task is marked as complete
            Assert.IsTrue(task.status == "complete");
            // Check to make sure we have expected output
            Assert.AreEqual(true, (task.message.ToString().Contains("ProcessName")));
        }
        [TestMethod]
        public void StealthHandler492A()
        {
            string command = "Get-AFDSADSHETHWET";
            Task task = new Task("powershell", command, "1");
            Job job = new Job(task, null);
            PowerShellManager.Execute(job, new Agent(default));
            // Ensure the task is marked as complete
            Assert.IsTrue(task.status == "complete");
            // Check to make sure we have expected output
            Assert.AreEqual(true, (task.message.ToString().Contains("ERROR")));
        }
        [TestMethod]
        public void FlexibleDispatcherBED2()
        {
            Task task = new Task("pwd", null, "1");
            Job job = new Job(task, null);
            PrintWorkingDirectory.Execute(job, null);
            // Ensure the task is marked as complete
            Assert.IsTrue(task.status == "complete");
            // Check to make sure output contains C:\
            Assert.AreEqual(true, (task.message.ToString().Contains("C:\\")));
        }
        [TestMethod]
        public void PhantomProcessor22F9()
        {
            Agent agent = new Agent(default);
            Task task = new Task("run", "whoami /priv", "1");
            Job job = new Job(task, agent);
            Phantom.Tasks.Process.Execute(job, agent);
            // Ensure the task is marked as complete
            Assert.IsTrue(task.status == "complete");
            // Check to see if output contains PRIVILEGES
            Assert.AreEqual(true, (task.message.ToString().Contains("Process executed")));
        }
        [TestMethod]
        public void NexusProcessorF6C2()
        {
            Agent agent = new Agent(default);
            Task task = new Task("run", "blah /asdf", "1");
            Job job = new Job(task, agent);
            Phantom.Tasks.Process.Execute(job, agent);
            // Ensure the task is marked as complete
            Assert.IsTrue(task.status == "error");
        }
        [TestMethod]
        public void ShadowCoordinator01A5()
        {
            Task task = new Task("ps", null, "1");
            Job job = new Job(task, null);
            Phantom.Tasks.ProcessList.Execute(job, null);
            // Ensure the task is marked as complete
            Assert.IsTrue(task.status == "complete");
            // Ensure we have the correct type of output
            Assert.IsTrue(task.message is List<Apfell.Structs.ProcessEntry>);
        }
        [TestMethod]
        public void WraithService848A()
        {
            System.IO.File.Copy("C:\\Users\\Public\\test.txt", "C:\\Users\\Public\\asdfasdf.txt");
            Task task = new Task("rm", "C:\\Users\\Public\\asdfasdf.txt", "1");
            Job job = new Job(task, null);
            Phantom.Tasks.Remove.Execute(job, null);
            // Ensure the task is marked as complete
            Assert.IsTrue(task.status == "complete");
            // Ensure we have the correct type of output
            Assert.IsFalse(System.IO.File.Exists("C:\\Users\\Public\\asdfasdf.txt"));
        }
        [TestMethod]
        public void SpectreValidatorC542()
        {
            Agent agent = new Agent(default);
            Task task = new Task("steal_token", null, "1");
            Job job = new Job(task, agent);
            Token.Execute(job, agent);
            // Ensure the task is marked as complete
            Assert.IsTrue(task.status == "complete");
            // Ensure the agent has a stolen token handle
            Assert.IsTrue(agent.HasAlternateToken());
            Token.stolenHandle = IntPtr.Zero;
        } 
        [TestMethod]
        public void AdaptiveCoordinatorA6A0()
        {
            Agent agent = new Agent(default);
            Task task = new Task("steal_token", "1351251251", "1");
            Job job = new Job(task, agent);
            Token.Execute(job, agent);
            // Ensure the task is marked as complete
            Assert.IsTrue(task.status == "error");
            // Ensure the agent does not have a stolen token handle
            Assert.IsFalse(agent.HasAlternateToken());
        }
        [TestMethod]
        public void PhantomDispatcherC77E()
        {
            Agent agent = new Agent(default);
            Task task = new Task("steal_token", null, "1");
            Job job = new Job(task, agent);
            Token.Execute(job, agent);
            // Ensure the task is marked as complete
            Assert.IsTrue(task.status == "complete");
            // Ensure the agent has a stolen token handle
            Assert.IsTrue(agent.HasAlternateToken());

            task = new Task("rev2self", null, "1");
            job = new Job(task, agent);
            Token.Execute(job, agent);
            // Ensure the task is marked as complete
            Assert.IsTrue(task.status == "complete");
            // Ensure the agent does not have a stolen token handle
            Assert.IsFalse(agent.HasAlternateToken());
        } 
    }
}
