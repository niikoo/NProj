using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;
using OSERV_BASE.Classes;

namespace OSERV_BASE.Impersonation
{
    public class InteractiveProcessRunner
    {
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObj);

        /// <summary>
        /// Runs an interactiveProcess from a Service.
        /// </summary>
        /// <param name="Application">Application path</param>
        /// <param name="Arguments">Arguments, defaults to empty</param>
        /// <param name="WorkingDirectory">Working directory, defaults to C:\</param>
        /// <param name="ReturnException">If true then exception handling is bypassed, defaults to false</param>
        /// <param name="waitForExit">Wait for the program to exit</param>
        public void interactiveProcess(string Application, string Arguments = "", string WorkingDirectory = @"C:\", bool ReturnException = false, bool waitForExit = false)
        {
            IntPtr hSessionToken = IntPtr.Zero;
            try
            {
                SessionFinder sf = new SessionFinder();
                //Get the ineractive console session.
                hSessionToken = sf.GetLocalInteractiveSession();

                //Use this instead to get the session of a specific user.
                //hSessionToken = sf.GetSessionByUser("IRIS", "nom1");

                if (hSessionToken != IntPtr.Zero)
                {
                    //Run notepad in the session that we found using the default
                    //values for working directory and desktop.
                    InteractiveProcessRunnerHelper runner = new InteractiveProcessRunnerHelper(Application, hSessionToken);
                    runner.CommandLine = Arguments;
                    runner.WorkingDirectory = WorkingDirectory;
                    runner.Run(waitForExit);
                }
                else
                {
                    if (ReturnException == true)
                    {
                        throw new Exception("Session not found");
                    }
                    else
                    {
                        Dbg.LogEvent("Session not found.", EventLogEntryType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ReturnException == true)
                {
                    throw ex;
                }
                else
                {
                    Dbg.LogEvent(string.Format("Exception thrown: {0}{0}{1}", Environment.NewLine, ex), EventLogEntryType.Error);
                }
            }
            finally
            {
                try
                {
                    if (hSessionToken != IntPtr.Zero)
                    {
                        CloseHandle(hSessionToken);
                    }
                }
                catch (Exception ex)
                {
                    if (ReturnException == true)
                    {
                        throw ex;
                    }
                    else
                    {
                        Dbg.LogEvent(string.Format("Exception thrown: {0}{0}{1}", Environment.NewLine, ex), EventLogEntryType.Error);
                    }
                }
            }
        }
    }
    class InteractiveProcessRunnerHelper
    {
        private string m_ApplicationPath;
        private IntPtr m_SessionTokenHandle;

        //Remember many applications expect the application name to be the first
        //command line argument.
        public string CommandLine { get; set; }
        public string WorkingDirectory { get; set; }
        public bool CreateNoWindow { get; set; }
        public string Desktop { get; set; }

        private const int NORMAL_PRIORITY_CLASS = 0x20;
        private const int CREATE_UNICODE_ENVIRONMENT = 0x400;
        private const int CREATE_NO_WINDOW = 0x08000000;

        public InteractiveProcessRunnerHelper(string appPath, IntPtr hSessionToken)
        {
            m_ApplicationPath = appPath;
            m_SessionTokenHandle = hSessionToken;
            
            //Working directory must be set to something. If there is no working directory
            //CreateProcessAsUser will fail with invalid directory error.
            WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);

            //Default desktop and window station name. This is valid for an interactive 
            //session but if launching into a non-interactive session the name of the window
            //station will need to be discovered. If there are multiple desktops in the 
            //window staton the name of the correct one will need to be discovered.
            Desktop = "WinSta0\\Default";
        }
        
        /// <summary>
        /// Run a process as a different user
        /// </summary>
        /// <param name="waitForExit">Wait for the created process to exit</param>
        /// <returns>Process ID</returns>
        public int Run(bool waitForExit=false)
        {
            STARTUPINFO si = new STARTUPINFO();
            si.lpDesktop = Desktop;
            PROCESSINFO pi = new PROCESSINFO();

            int creationFlags = NORMAL_PRIORITY_CLASS | CREATE_UNICODE_ENVIRONMENT;
            creationFlags |= CreateNoWindow ? CREATE_NO_WINDOW : 0;

            //This creates the default environment block for this user. If you need 
            //something custom skip te CreateEnvironmentBlock (and DestroyEnvironmentBlock) 
            //calls. You need to handle the allocation of the memory and writing to 
            //it yourself.
            IntPtr envBlock;
            if (!CreateEnvironmentBlock(out envBlock, m_SessionTokenHandle, 0))
            {
                throw new System.ComponentModel.Win32Exception();
            }

            try
            {
                if (!CreateProcessAsUser(m_SessionTokenHandle, m_ApplicationPath, CommandLine, IntPtr.Zero,
                    IntPtr.Zero, 0, creationFlags, envBlock, WorkingDirectory,
                    si, pi))
                {
                    throw new System.ComponentModel.Win32Exception();
                }
            }
            finally
            {
                if (waitForExit == true)
                {
                    const uint INFINITE = 0xFFFFFFFF;
                    WaitForSingleObject(pi.hProcess, INFINITE);
                }
                DestroyEnvironmentBlock(envBlock);
            }

            CloseHandle(pi.hThread);
            CloseHandle(pi.hProcess);

            if (waitForExit == true)
            {
                return -1;
            }
            else
            {
                return pi.dwProcessId;
            }
        }

        [DllImport("AdvApi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CreateProcessAsUser(IntPtr hToken, string lpApplicationName,
            string lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
            int bInheritHandles, int dwCreationFlags, IntPtr lpEnvironment,
            string lpCurrentDirectory, STARTUPINFO lpStartupInfo, 
            [Out]PROCESSINFO lpProcessInformation);


        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObj);

        [DllImport("userenv.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CreateEnvironmentBlock(out IntPtr lpEnvironment,
            IntPtr hToken, int bInherit);

        [DllImport("userenv.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DestroyEnvironmentBlock(IntPtr lpEnvironment);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

    }
}
