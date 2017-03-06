using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.DirectoryServices.ActiveDirectory;
using System.Collections;
using OSERV_BASE.Classes;
using OSERV_BASE.Impersonation;
using System.Security.Principal;
using System.Collections.Generic;
using OSERV_BASE.Functions;

namespace OSERV_BASE
{
    partial class OSERV : ServiceBase
    {
        #region CLASS STARTUP AND RUN FROM COMMAND LINE
        //# Service name
        public static string serviceName = "OSERV";

        static void starter()
        {
            Application.Run();
        }

        /// <summary>
        /// Public Constructor for WindowsService.
        /// - Put all of your Initialization code here.
        /// </summary>
        public OSERV()
        {
            this.ServiceName = serviceName;
            this.EventLog.Log = "Application";

            // These Flags set whether or not to handle that specific
            //  type of event. Set to true if you need it, false otherwise.
            this.CanHandlePowerEvent = true;
            this.CanHandleSessionChangeEvent = true;
            this.CanPauseAndContinue = false;
            this.CanShutdown = true;
            this.CanStop = true;
        }

        public static bool RunningFromCommandLine = false; // IF THE SERVICE IS RUNNED IN COMMAND LINE MODE WITH THE /i SWITCH

        public void LaunchCommandLine()
        {
            LogEvent("Running in console using /i.", EventLogEntryType.Information);
            Console.WriteLine("OSERV - RUNNING WITH OUTPUT TO CONSOLE");
            isLoggedIn = USER_LOGGED_IN;
            RunningFromCommandLine = true;
            OnStart(null);
            try
            {
                logonThread = new Thread(() => theLogonThread());
                logonThread.Start();
            }
            catch (Exception ex)
            {
                LogEvent(ex.Message, EventLogEntryType.Error, false);
            }
            Application.Run();
        }

        Thread NamedPipeServer;
        #endregion

        #region Service Events

        /// <summary>
        /// OnStart(): Put startup code here
        ///  - Start threads, get inital data, etc.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            // LOG TO FILE - DEBUG MODE ACTIVE?
            try
            {
                if (AppConfig.Get("debugToFile").ToLower() == "true")
                {
                    LogEvent("DEBUG MODE ENABLED! Logging to file: " + OSERV.appConfig[2], EventLogEntryType.Information);
                    Dbg.enableFileDebug = true;
                    LogEvent("OSERV STARTED!", EventLogEntryType.Information);
                }
            }
            catch { }
            // EXECUTE SCRIPTS AT START
            if (Dynamics.CanRun(Dynamics.FunctionList.GLOW)) GLOW.Script(Functions.GLOW.ScriptType.Start);
            // START NAMED PIPE SERVER
            NamedPipeServer = new Thread(() => Classes.NamedPipeCommunication.ThreadStartCommandServer());
            NamedPipeServer.Start();
            // START LISTENER
            Network.changeChecker();
            LogEvent("NETWORK CHANGE CHECKER STARTED");
            if (Dynamics.CanRun(Dynamics.FunctionList.APPLIMITER)) {
                AppLimiter.Stop();
                AppLimiter.Start();
            }
            base.OnStart(args);
        }

        /// <summary>
        /// OnStop(): Put your stop code here
        /// - Stop threads, set final data, etc.
        /// </summary>
        protected override void OnStop()
        {
            base.OnStop();
            try
            {
                logonThread.Abort();
            }
            catch { }
            try
            {
                if (Dynamics.CanRun(Dynamics.FunctionList.APPLIMITER)) AppLimiter.Stop();
                Network.stopChangeChecker();
            }
            catch { }
            try
            {
                //ServiceManager.startServiceThread.Abort();
            }
            catch { }
            Dbg.LogEvent("OSERV stopped.");
        }

        /// <summary>
        /// OnPause: Put your pause code here
        /// - Pause working threads, etc.
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
        }

        /// <summary>
        /// OnContinue(): Put your continue code here
        /// - Un-pause working threads, etc.
        /// </summary>
        protected override void OnContinue()
        {
            base.OnContinue();
        }

        /// <summary>
        /// OnShutdown(): Called when the System is shutting down
        /// - Put code here when you need special handling
        ///   of code that deals with a system shutdown, such
        ///   as saving special data before shutdown.
        /// </summary>
        protected override void OnShutdown()
        {
            base.OnShutdown();
        }

        /// <summary>
        /// OnCustomCommand(): If you need to send a command to your
        ///   service without the need for Remoting or Sockets, use
        ///   this method to do custom methods.
        /// </summary>
        /// <param name="command">Arbitrary Integer between 128 & 256</param>
        protected override void OnCustomCommand(int command)
        {
            //  A custom command can be sent to a service by using this method:
            //#  int command = 128; //Some Arbitrary number between 128 & 256
            //#  ServiceController sc = new ServiceController("NameOfService");
            //#  sc.ExecuteCommand(command);
            base.OnCustomCommand(command);
        }

        /// <summary>
        /// NATIVE FUNCTION FOR HANDLING THREAD EXECUTION STATE
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [DllImport("Kernel32.DLL",
                   CharSet = CharSet.Auto, SetLastError = true)]
        protected static extern EXECUTION_STATE
                  SetThreadExecutionState(EXECUTION_STATE state);
        [Flags]
        public enum EXECUTION_STATE : uint
        {
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001,
            ES_AWAYMODE_REQUIRED = 0x00000040
        }

        /// <summary>
        /// OnPowerEvent(): Useful for detecting power status changes,
        ///   such as going into Suspend mode or Low Battery for laptops.
        /// </summary>
        /// <param name="powerStatus">The Power Broadcast Status
        /// (BatteryLow, Suspend, etc.)</param>
        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            switch(powerStatus)
            {
                case PowerBroadcastStatus.ResumeSuspend:
                //case PowerBroadcastStatus.ResumeCritical:
                //case PowerBroadcastStatus.ResumeAutomatic:
                    LogEvent("Your computer just woke up from sleep > Sessionchanged: Resume | RUNNING SCRIPTS", EventLogEntryType.Information);
                    sessionChangeEvent(sessionChangeReason.RESUME);
                    LogEvent("Your computer just woke up from sleep > Sessionchanged: Resume | FINISHED RUNNING SCRIPTS", EventLogEntryType.Information);
                    break;
                case PowerBroadcastStatus.Suspend:
                    SetThreadExecutionState(EXECUTION_STATE.ES_SYSTEM_REQUIRED |
                                            EXECUTION_STATE.ES_CONTINUOUS);
                    SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_AWAYMODE_REQUIRED);
                    SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED);
                    LogEvent("Your computer is going to sleep > Sessionchanged: Suspend | RUNNING SCRIPTS", EventLogEntryType.Information);
                    sessionChangeEvent(sessionChangeReason.SUSPEND);
                    LogEvent("Your computer is going to sleep > Sessionchanged: Suspend | FINISHED RUNNING SCRIPTS", EventLogEntryType.Information);
                    SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
                    break;
            }
            return base.OnPowerEvent(powerStatus);
        }

        public const int USER_LOGGED_IN = 1;
        public const int USER_NOT_LOGGED_IN = 0;
        public static int isLoggedIn = USER_NOT_LOGGED_IN;

        /// <summary>
        /// OnSessionChange(): To handle a change event
        ///   from a Terminal Server session.
        ///   Useful if you need to determine
        ///   when a user logs in remotely or logs off,
        ///   or when someone logs into the console.
        /// </summary>
        /// <param name="changeDescription">The Session Change
        /// Event that occured.</param>
        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            base.OnSessionChange(changeDescription);
            switch (changeDescription.Reason)
            {
                case SessionChangeReason.ConsoleConnect:
                case SessionChangeReason.RemoteConnect:
                case SessionChangeReason.SessionLogon:
                    LogEvent(changeDescription.SessionId + " logon", EventLogEntryType.Information);
                    sessionChangeEvent(sessionChangeReason.LOGON);
                    LogEvent(changeDescription.SessionId + " logon: ENDED", EventLogEntryType.Information);
                    break;
                case SessionChangeReason.SessionLogoff:
                    LogEvent(changeDescription.SessionId + " logoff", EventLogEntryType.Information);
                    sessionChangeEvent(sessionChangeReason.LOGOFF);
                    LogEvent(changeDescription.SessionId + " logoff: ENDED", EventLogEntryType.Information);
                    break;
                case SessionChangeReason.SessionLock:
                    sessionChangeEvent(sessionChangeReason.LOCK);
                    break;
                case SessionChangeReason.SessionUnlock:
                    LogEvent(changeDescription.SessionId + " unlock", EventLogEntryType.Information);
                    sessionChangeEvent(sessionChangeReason.UNLOCK);
                    break;
            }
        }

        public enum sessionChangeReason : int {
            LOGON = 1,
            LOGOFF = 2,
            LOCK = 20,
            UNLOCK = 21,
            SUSPEND = 750,
            RESUME = 751,
            NETWORKCHANGE = 999
        }

        public static void sessionChangeEvent(sessionChangeReason scr)
        {
            switch (scr)
            {
                case sessionChangeReason.LOGON:
                case sessionChangeReason.RESUME:
                    if (scr == sessionChangeReason.LOGON) // IF LOGON > DO 'isLoggedIn' CHANGE
                    {
                        isLoggedIn = USER_LOGGED_IN;
                    }
                    try
                    {
                        logonThread = new Thread(() => theLogonThread());
                        logonThread.Start();
                    }
                    catch (Exception ex)
                    {
                        LogEvent(ex.Message, EventLogEntryType.Error, false);
                    }
                    break;
                case sessionChangeReason.LOGOFF:
                case sessionChangeReason.SUSPEND:      
                    if (scr == sessionChangeReason.LOGOFF) // IF LOGOFF > DO 'isLoggedIn' CHANGE
                    {
                        isLoggedIn = USER_NOT_LOGGED_IN;
                    }
                    if (Dynamics.CanRun(Dynamics.FunctionList.ROUTER)) RouterDDWRT.TurnOff();
                    if (Dynamics.CanRun(Dynamics.FunctionList.GLOW)) GLOW.Script(GLOW.ScriptType.Logoff);
                    break;
                case sessionChangeReason.LOCK:
                    if (Dynamics.CanRun(Dynamics.FunctionList.GLOW)) GLOW.Script(GLOW.ScriptType.Lock);
                    break;
                case sessionChangeReason.UNLOCK:
                    if (Dynamics.CanRun(Dynamics.FunctionList.GLOW)) GLOW.Script(GLOW.ScriptType.Unlock);
                    break;
                case sessionChangeReason.NETWORKCHANGE:
                    // Exec networkchange
                    if (Dynamics.CanRun(Dynamics.FunctionList.GLOW)) GLOW.Script(GLOW.ScriptType.NetworkChange);
                    // VPN INTERNET CONNECTION SHARING
                    if (Dynamics.CanRun(Dynamics.FunctionList.ICS)) VPN_ICS.SetupICS();
                    break;
            }
        }

        private static Thread logonThread;
        private static void theLogonThread()
        {
            LogEvent("logonThread started", EventLogEntryType.Information);
            if (Dynamics.CanRun(Dynamics.FunctionList.ROUTER)) RouterDDWRT.TurnOn();
            //Thread.Sleep(10 * 1000);
            //LogEvent("logonThread woke up", EventLogEntryType.Information);
            // SET PROXY -> If SetProxy is runned outside of logon, it won't work on local user registry (for firefox)
            if (Dynamics.CanRun(Dynamics.FunctionList.ICS)) VPN_ICS.SetupICS(); // ALSO UPDATES NETWORK CHANGE / UPDATE

            // EXEC GLOW SCRIPTS
            if (Dynamics.CanRun(Dynamics.FunctionList.GLOW)) GLOW.Script(GLOW.ScriptType.Logon);
            LogEvent("logonThread finished", EventLogEntryType.Information);
        }

        #endregion

        #region Application Configuration
        public static string[] appConfig = {
           @"C:\WINDOWS\GLOW\", // 0: GLOW SCRIPT FOLDER
            "IG_temp", // 1: VBS TEMP EXEC FILENAME
           @"C:\Windows\GLOW\debug.log", // 2: Debug logfile location
            "MullvadVPN", // 3: [ICS] VPN Adapter name
            "NETGEAR A6200 WiFi Adapteren", // 4: [ICS] Name of local internet connection to share (NO VPN)
            "LAN",  // 5: [ICS] Name of interface to forward to
            "192.168.10.2", // 6: [RouterDDWRT] Router telnet server IP (Must be a DD-WRT router with telnet activated)
            "23", // 7: [RouterDDWRT] Telnet server port
            "root", // 8: [RouterDDWRT] Telnet username
            "password", // 9: [RouterDDWRT] Telnet password
        }; // APPLICATION CONFIG HOLDER
        #endregion

        #region Script EXEC
        /// TEMP FILE COUNTER
        static uint _tempFileCounter = 0;
        static uint tempFileCounter
        {
            get
            {
                uint got = _tempFileCounter;
                _tempFileCounter += 1;
                return got;
            }
            set
            {
                _tempFileCounter = tempFileCounter;
            }
        }
        /// <summary>
        /// EXECUTE VBS SCRIPT FUNCTION (AS LOCAL USER)
        /// </summary>
        /// <param name="content">VBS Script Code</param>
        /// <param name="ReturnException">If true then don't handle exception (default false)</param>
        /// <param name="doNotImpersonateCurrentUser">Don't impersonate the script as the current logged in user? (default false)</param>
        /// <param name="waitForExit">Wait for program exit</param>
        public static void execVBS(string content,bool ReturnException = false,bool doNotImpersonateCurrentUser = false, bool waitForExit = false)
        {
            string filename = "C:\\windows\\" + appConfig[1] + tempFileCounter + ".vbs";
            try
            {
                if (doNotImpersonateCurrentUser)
                {
                    System.IO.File.WriteAllText(filename, content);
                    Process scriptProc = new Process();
                    scriptProc.StartInfo.FileName = @"cscript.exe";
                    scriptProc.StartInfo.Arguments = "//Nologo " + filename;
                    scriptProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; //prevent console window from popping up
                    scriptProc.Start();
                    if (waitForExit)
                    {
                        scriptProc.WaitForExit();
                        scriptProc.Close();
                    }
                }
                else
                {
                    System.IO.File.WriteAllText(filename, content);
                    InteractiveProcessRunner ipr = new InteractiveProcessRunner();
                    ipr.interactiveProcess("wscript.exe", "//Nologo " + filename, appConfig[0], ReturnException, waitForExit);
                }
            }
            catch (Exception e)
            {
                if (ReturnException == true)
                {
                    throw e;
                }
                else
                {
                    LogEvent("The file could note be written or the VBScript failed to execute (as user), exception: " + e.Message, EventLogEntryType.Error,false);
                }
            }
        }
        /// <summary>
        /// EXECUTE VBS SCRIPT FUNCTION (AS SERVICE)
        /// </summary>
        /// <param name="content">VBS Script Code</param>
        /// <param name="ReturnException">If true then don't handle exception(default false)</param>
        public static void execVBS_AsService(string content, bool ReturnException = false)
        {
            string filename = "C:\\windows\\" + appConfig[1] + tempFileCounter + ".vbs";
            try
            {
                System.IO.File.WriteAllText(filename, content);
                Process scriptProc = new Process();
                scriptProc.StartInfo.FileName = @"cscript";
                scriptProc.StartInfo.Arguments = "//Nologo " + filename;
                scriptProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; //prevent console window from popping up
                scriptProc.Start();
                scriptProc.WaitForExit();
                scriptProc.Close();
            }
            catch (Exception e)
            {
                if (ReturnException == true)
                {
                    throw e;
                }
                else
                {
                    LogEvent("The file could note be written or the VBScript failed to execute (as service), exception: " + e.Message, EventLogEntryType.Error,false);
                }
            }
        }
        #endregion

        #region Extra functions

        static public bool IsUserAdministrator()
        {
            //bool value to hold our return value 
            bool isAdmin;
            Exception exception;
            try
            {
                //get the currently logged in user 
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException ex)
            {
                exception = ex;
                isAdmin = false;
            }
            catch (Exception ex)
            {
                exception = ex;
                isAdmin = false;
            }
            return isAdmin;
        }

        /// <summary>
        /// Returns the current application version
        /// </summary>
        /// <returns>Application version</returns>
        public static string getVersion()
        {
            return Application.ProductVersion;
        }

        #endregion

        #region LOGGING
        /// <summary>
        /// LogEvent (Log to eventlog in service mode and direct console output in CLI mode)
        /// </summary>
        /// <param name="EventDescription">Text to log</param>
        /// <param name="elet">EventLogEntryType</param>
        /// <param name="OnlyShowInCommandLineMode">If true(std), output will only be visible in command line mode.</param>
        public static void LogEvent(string EventDescription, EventLogEntryType elet = EventLogEntryType.Information, bool OnlyShowInCommandLineMode = true)
        {
            Dbg.LogEvent(EventDescription, elet, OnlyShowInCommandLineMode);
        }
        #endregion
    }
}