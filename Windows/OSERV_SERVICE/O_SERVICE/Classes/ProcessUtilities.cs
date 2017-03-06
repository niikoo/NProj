using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Diagnostics;
using System.Threading;

namespace OSERV_BASE.Classes
{
    static class ProcessUtilities
    {
        public static void KillProcessTree(Process root)
        {
            if (root != null)
            {
                var list = new List<Process>();
                GetProcessChildren(Process.GetProcesses(), root, list, 1, root);

                foreach (Process p in list)
                {
                    try
                    {
                        p.Kill();
                    }
                    catch (Exception ex)
                    {
                        Dbg.LogEvent("Couldn't kill process tree -> Message: " + ex.Message, EventLogEntryType.Warning);
                    }
                }
            }
        }

        public static List<Process> GetChildrenProcesses(Process root)
        {
            var list = new List<Process>();
            if (root != null)
            {
                try
                {
                    GetProcessChildren(Process.GetProcesses(), root, list, 1, root);
                    return list;
                }
                catch (Exception ex)
                {
                    Dbg.LogEvent("Couldn't get process tree -> Message: " + ex.Message, EventLogEntryType.Warning);
                }
            }
            return list;
        }

        private static int GetParentProcessId(Process p)
        {
            int parentId = 0;
            try
            {
                ManagementObject mo = new ManagementObject("win32_process.handle='" + p.Id + "'");
                mo.Get();
                parentId = Convert.ToInt32(mo["ParentProcessId"]);
            }
            catch // (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                parentId = 0;
            }
            return parentId;
        }

        private static void GetProcessChildren(Process[] plist, Process parent, List<Process> output, int indent, Process rootProcess)
        {
            foreach (Process p in plist)
            {
                if (GetParentProcessId(p) == parent.Id)
                {
                    GetProcessChildren(plist, p, output, indent + 1, rootProcess);
                }
            }
            if (parent.Id != rootProcess.Id)
            {
                output.Add(parent);
            }
        }
        /// <summary>
        /// Kill a process, and all of its children.
        /// </summary>
        /// <param name="pid">Process ID.</param>
        public static void KillProcessAndChildren(int pid)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection moc = searcher.Get();
            foreach (ManagementObject mo in moc)
            {
                KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
            }
            try
            {
                Process proc = Process.GetProcessById(pid);
                proc.Kill();
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }
        }
        /// <summary>
        /// Closes a process, and all of its children.
        /// Will wait for close, and if it doesn't close, kill
        /// NOT FINISHED!
        /// <todo>Finish it</todo>
        /// </summary>
        /// <param name="pid">Process ID.</param>
        public static void CloseProcessAndChildren(int pid)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection moc = searcher.Get();
            foreach (ManagementObject mo in moc)
            {
                CloseProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
            }
            try
            {
                Process proc = Process.GetProcessById(pid);
                proc.Close();
                bool closed = false;
                try
                {
                    for (int i = 0; i < 100; i++)
                    {
                        if (proc.HasExited == true)
                        {
                            closed = true;
                            i = 100;
                            continue;
                        }
                        Thread.Sleep(50);
                    }
                }
                catch
                {
                    closed = false;
                }
                if (closed == false)
                {
                    KillProcessAndChildren(pid);
                }
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }
        }
    }
}
