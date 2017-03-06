using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Management;

namespace OSERV_BASE.Classes
{
    class TimePeriod
    {
        public TimePeriod(DateTime from, DateTime to)
        {
            _from = from;
            _to = to;
        }
        private DateTime _from;
        public DateTime from
        {
            get { return _from; }
            set { _from = value; }
        }
        private DateTime _to;
        public DateTime to
        {
            get { return _to; }
            set { _to = value; }
        }
        /// <summary>
        /// Checks if a given time point is in the given period of time
        /// </summary>
        /// <param name="point">Time point</param>
        /// <returns>Is the point in the period?</returns>
        public bool inPeriod(DateTime point) {
            double fromTime = toSXDATE(_from);
            double toTime = toSXDATE(_to);
            double nowTime = toSXDATE(point);
            if (fromTime >= toTime)
            {
                return false; // Fra dato er lik eller størren enn til dato
            }
            if (nowTime >= fromTime && nowTime <= toTime)
            {
                return true; // Dato er inni perioden
            }
            return false;
        }
        // SXDATE CONVERTER (DateTime to double->{YYYYMMDDHHII.1})
        private double toSXDATE(DateTime input)
        {
            double returner;
            char leadingZero = Convert.ToChar("0");
            string[] parts = {
                input.Year.ToString().PadLeft(2, leadingZero),
                input.Month.ToString().PadLeft(2, leadingZero),
                input.Day.ToString().PadLeft(2, leadingZero),
                input.Hour.ToString().PadLeft(2, leadingZero),
                input.Minute.ToString().PadLeft(2,leadingZero)
            };
            returner = 0.1 + Convert.ToDouble(PHP.implode("", parts));
            return returner;
        }
    }
    class LimitedApp
    {
        public LimitedApp(String processName, DateTime killFrom, DateTime killTo)
        {
            _processName = processName;
            _KillFrom = killFrom;
            _KillTo = killTo;
        }
        private String _processName;
        public String ProcessName
        {
            get { return _processName; }
        }
        private DateTime _KillFrom;
        public DateTime KillFrom
        {
            get { return _KillFrom; }
        }
        private DateTime _KillTo;
        public DateTime KillTo
        {
            get { return _KillTo; }
        }
    }
    class LimitedApps : List<LimitedApp>
    { }

    static class AppLimiter
    {
        public static int classID()
        {
            return 2775;
        }
        private static Thread _appLimiterThread = null;
        public static void Start()
        {
            try
            {
                _appLimiterThread = new Thread(processInvokerDeluxe);
                _appLimiterThread.Start();
            }
            catch (Exception ex)
            {
                Dbg.LogEvent("[APP KILLER] Couldn't start AppLimiter because of: " + ex.Message, classID());
            }
        }
        public static void Stop()
        {
            if (_appLimiterThread != null)
            {
                _appLimiterThread.Abort();
                _appLimiterThread.Join();
                _appLimiterThread = null;
            }
            else
            {
                Dbg.LogEvent("[APP KILLER] Couldn't stop the AppLimiter because it isn't running!", EventLogEntryType.Warning, classID());
            }
        }

        public static LimitedApps appLimitList = new LimitedApps();

        /// <summary>
        /// Gets the current CPU load
        /// </summary>
        /// <returns>CPU load percent (out of 100)</returns>
        private static int GetCPUStatus()
        {
            int cpustatus = 0;
            try
            {
                ObjectQuery wmicpus = new WqlObjectQuery("SELECT * FROM Win32_Processor");
                ManagementObjectSearcher cpus = new ManagementObjectSearcher(wmicpus);
                int coreCount = 0;
                int totusage = 0;
                foreach (ManagementObject cpu in cpus.Get())
                {
                    //cpuStatus.Add(cpu["DeviceId"] + " = " + cpu["LoadPercentage"]);
                    coreCount += 1;
                    totusage += Convert.ToInt32(cpu["LoadPercentage"]);
                }
                if (coreCount > 1)
                {
                    double ActUtiFloat = totusage / coreCount;
                    int ActUti = Convert.ToInt32(Math.Round(ActUtiFloat));
                    //Utilisation = ActUti + "%";
                    cpustatus = ActUti;
                }
                else
                {
                    cpustatus = totusage;
                }
                cpus.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return cpustatus;
        }


        private static void processInvokerDeluxe()
        {
            int DO_DEEP_SCAN_LOWCPU = 30; // Cycles before deep scan when it's low CPU load -- (1 cycle = 10sec) number * 10 sec = delay between each deep scan run (example: 18 will be 18*10sec=180sec=3min) -> use longer delay for less cpu use
            int DO_DEEP_SCAN_HIGHCPU = 60; // Cycles before deep scan when it's high CPU load (over 50% load)
            int DO_DEEP_SCAN = DO_DEEP_SCAN_LOWCPU; // Will automatically adjust between DO_DEEP_SCAN_LOWCPU and DO_DEEP_SCAN_HIGHCPU
            int deepScan = DO_DEEP_SCAN_LOWCPU;
            while (true)
            {
                if (appLimitList.Count == 0 || deepScan == 0) // Run on start and after deep scan
                {
                    if (File.Exists(OSERV.appConfig[0] + "appLimiter.conf"))
                    {
                        appLimitList.Clear(); // Clear list
                        List<String> appsToLimit = PHP.file(OSERV.appConfig[0] + "appLimiter.conf");
                        int counter = 0;
                        DateTime from, to;
                        string[] ex, appex;
                        foreach (string app in appsToLimit)
                        {
                            if (app.Substring(0, 1) == "#")
                            {
                                continue;
                            }
                            counter += 1;
                            appex = PHP.explode(";", app);
                            // appLimiter.conf format:
                            // EACH LINE IS ONE APP, EACH FIELD IS SEMICOLON (;) SEPARATED
                            // Process name
                            // Kill app from this time (HH:ii -> hours:minutes -> 08:30 -> 14:13)
                            // Kill app to this time (same format as above)
                            // Optional: comma seperated list over computer names to exclude
                            try
                            {
                                if (appex.Count() == 3 || appex.Count() == 4)
                                {
                                    ex = PHP.explode(":", appex[1]); // FROM TIME
                                    if (ex.Count() == 2)
                                    {
                                        from = DateTime.Today;
                                        from = from.AddHours(Convert.ToInt32(ex[0]));
                                        from = from.AddMinutes(Convert.ToInt32(ex[1]));
                                        ex = PHP.explode(":", appex[2]); // TO TIME
                                        if (ex.Count() == 2)
                                        {
                                            to = DateTime.Today;
                                            to = to.AddHours(Convert.ToInt32(ex[0]));
                                            to = to.AddMinutes(Convert.ToInt32(ex[1]));
                                            bool excludeApp = false;
                                            if (appex.Count() == 4) // If it includes computer names to exclude
                                            {
                                                if (appex[3].Length > 0)
                                                {
                                                    try
                                                    {
                                                        String[] excluded = PHP.explode(",", appex[3]);
                                                        foreach (string exclude in excluded)
                                                        {
                                                            if (exclude == Environment.MachineName)
                                                            {
                                                                excludeApp = true;
                                                            }
                                                        }
                                                    }
                                                    catch { }
                                                }
                                            }
                                            if (excludeApp == false)
                                            {
                                                appLimitList.Add(new LimitedApp(appex[0], from, to));
                                                Dbg.LogEvent("[APP KILLER] Added app: " + appex[0] + " -> KILL BETWEEN " + appex[1] + " AND " + appex[2], classID());
                                            }
                                            else
                                            {
                                                Dbg.LogEvent("[APP KILLER] Skipped app: " + appex[0], classID());
                                            }
                                        }
                                        else
                                        {
                                            throw new Exception("SYNTAX ERROR");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("SYNTAX ERROR");
                                    }
                                }
                                else
                                {
                                    throw new Exception("SYNTAX ERROR");
                                }
                            }
                            catch
                            {
                                Dbg.LogEvent("[APP KILLER] Syntax error on line " + counter + " in " + OSERV.appConfig[0] + "appLimiter.conf", EventLogEntryType.Warning, classID());
                            }
                        }
                    }
                    else
                    {
                        Dbg.LogEvent("[APP KILLER] The configuration file " + OSERV.appConfig[0] + "appLimiter.conf doesn't exist! APP KILLER will not work.", EventLogEntryType.Warning, classID());
                        return;
                    }
                }
                if (appLimitList.Count == 0)
                {
                    Dbg.LogEvent("[APP KILLER] No Apps limited", EventLogEntryType.Warning, classID());
                    return;
                }
                foreach (LimitedApp app in appLimitList)
                {
                    if (GetCPUStatus() > 50) // If CPU load is over 50%
                    {
                        DO_DEEP_SCAN = DO_DEEP_SCAN_HIGHCPU;
                    }
                    else
                    {
                        DO_DEEP_SCAN = DO_DEEP_SCAN_LOWCPU;
                    }
                    Process[] processes = System.Diagnostics.Process.GetProcessesByName(app.ProcessName);
                    foreach (Process process in processes)
                    {
                        //process.EnableRaisingEvents = true;
                        TheProcessKiller(process, app.KillFrom, app.KillTo);
                    }
                    if (deepScan >= DO_DEEP_SCAN) // DO A DEEP SCAN WITH A DELAY BASED ON CPU LOAD
                    {
                        //Dbg.LogEvent("[APP KILLER] Doing a deep scan to find traces of: " + app.ProcessName);
                        Process[] runningProcesses = System.Diagnostics.Process.GetProcesses();
                        bool match = false;
                        foreach (Process runningProcess in runningProcesses)
                        {
                            match = false;
                            if (runningProcess.ProcessName == "example")
                            {
                                foreach (System.Diagnostics.ProcessModule procModule in runningProcess.Modules)
                                {
                                    if (PHP.stristr(procModule.ModuleName, app.ProcessName).Length > 0)
                                    {
                                        match = true;
                                    }
                                    else if (PHP.stristr(procModule.FileVersionInfo.OriginalFilename, app.ProcessName).Length > 0)
                                    {
                                        match = true;
                                    }
                                    else if (PHP.stristr(procModule.FileVersionInfo.InternalName, app.ProcessName).Length > 0)
                                    {
                                        match = true;
                                    }
                                    else if (PHP.stristr(procModule.FileName, app.ProcessName).Length > 0)
                                    {
                                        match = true;
                                    }
                                }
                                if (match == true)
                                {
                                    //Dbg.LogEvent("[APP KILLER] Deep scan of process: " + app.ProcessName + " gave results! Found a similar process: " + runningProcess.ProcessName);
                                    TheProcessKiller(runningProcess, app.KillFrom, app.KillTo);
                                }
                            }
                        }
                        //Dbg.LogEvent("[APP KILLER] Deep scan of process: " + app.ProcessName + " finished!");
                    }
                }
                Thread.Sleep(10000);
                if (deepScan >= DO_DEEP_SCAN)
                {
                    deepScan = 0; // RESET
                }
                else
                {
                    deepScan++; // INCREASE
                }
            }
        }

        private static List<String> alreadyKilling = new List<String>();

        private static bool TheProcessKiller(Process proc, DateTime from, DateTime to, bool runThread = true) {
            if (runThread == true)
            {
                try
                {
                    string result = alreadyKilling.Find(x => x == proc.ProcessName);
                    foreach (string processName in alreadyKilling)
                    {
                        if (processName == proc.ProcessName)
                        {
                            return false;
                        }
                    }
                }
                catch { }
                alreadyKilling.Add(proc.ProcessName);
                Thread tpk = new Thread(() => TheProcessKiller(proc, from, to, false));
                tpk.Start();
                return true;
            }
            TimePeriod tp = new TimePeriod(from, to);
            if (tp.inPeriod(DateTime.Now))
            {
                // APPLICATION CLOSING WARNING //
                string warningText = String.Format("The application {0} will be closed in 30 seconds because of system policy, please save your work. Click 'OK' to close this application now.",proc.ProcessName.ToUpper());
                // THE MESSAGEBOX HAS A TIMEOUT OF 30sec -> THIS SERVICE WILL WAIT UNTIL IT'S FINISHED
                OSERV.execVBS("On Error Resume Next\r\nSet WshShell = CreateObject(\"WScript.Shell\")\r\nintButton = WshShell.Popup (\"" + warningText + "\", 30, \"IRIS\", 48)", false, OSERV.RunningFromCommandLine, true);
                // END APPLICATION CLOSING WARNING //
                
                // HENT ALLE INSTANCER MED SAMME NAVN -> IKKE BARE DENNE PROSESSEN
                Process[] procs = System.Diagnostics.Process.GetProcessesByName(proc.ProcessName);
                foreach (Process procTK in procs)
                {
                    try
                    {
                        ProcessUtilities.KillProcessAndChildren(procTK.Id);
                        //proc.Kill(); // Kill process
                    }
                    catch (Exception ex)
                    {
                        Dbg.LogEvent("Couldn't kill process: " + proc.ProcessName + " -> Exception: " + ex.Message, EventLogEntryType.Error, false, classID());
                    }
                }
                alreadyKilling.Remove(proc.ProcessName);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
