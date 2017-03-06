using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace OSERV_BASE.Classes
{
    #region Event Handling
    /// <summary>
    /// SERVICE-Debug helper class
    /// </summary>
    public static class Dbg
    {
        static EventLog eventlog;
        static Dbg()
        {
            try
            {
                if (!EventLog.SourceExists(OSERV.serviceName))
                {
                    EventLog.CreateEventSource(OSERV.serviceName, "Application");
                }
                eventlog = new EventLog();
                eventlog.Source = OSERV.serviceName;
            }
            catch { }
        }
        public static void LogEvent(string EventDescription)
        {
            LogEvent(EventDescription, EventLogEntryType.Information, true, 0);
        }
        public static void LogEvent(string EventDescription, int EventID)
        {
            LogEvent(EventDescription, EventLogEntryType.Information, true, EventID);
        }
        public static void LogEvent(string EventDescription, EventLogEntryType elet = EventLogEntryType.Information)
        {
            LogEvent(EventDescription, elet, true, 0);
        }
        public static void LogEvent(string EventDescription, EventLogEntryType elet = EventLogEntryType.Information, int EventID = 0)
        {
            LogEvent(EventDescription, elet, true, EventID);
        }
        /// <summary>
        /// LogEvent (Log to eventlog in service mode and direct console output in CLI mode)
        /// </summary>
        /// <param name="EventDescription">Text to log</param>
        /// <param name="elet">EventLogEntryType</param>
        /// <param name="OnlyShowInCommandLineMode">If true(std), output will only be visible in command line mode.</param>
        public static void LogEvent(string EventDescription, EventLogEntryType elet = EventLogEntryType.Information, bool OnlyShowInCommandLineMode = true, int EventID = 0)
        {
            // If it's running as service, then disable output if OnlyShowInCommandLineMode = true
            //OnlyShowInCommandLineMode = false; // DEBUG!!!
            if (OnlyShowInCommandLineMode == true && OSERV.RunningFromCommandLine == false && enableRemoteDebug == false && enableFileDebug == false)
            {
                return;
            }
            // Skip if EventID == 15 -> infoNet
            /*if (EventID == 15 && enableFileDebug != true)
            {
                return;
            }*/
            // SKIP NamedPipeCommunication LOGEVENTS -> EventID 130-139 WHEN enableRemoteDebug == true (over NPC)
            if (EventID >= 130 && EventID <= 139 && enableRemoteDebug == true)
            {
                return;
            }
            string ExtensiveInfo = "[" + DateTime.Now.ToString("dd.MM-yyyy") + "][" + DateTime.Now.ToString("HH:mm:ss") + "] {eID:" + EventID + "} ";
            // Switch type
            switch (elet)
            {
                // Generate error message
                case EventLogEntryType.Error:
                    if (enableRemoteDebug == true)
                    {
                        NamedPipeCommunication.sendMessageToServer("ERR!: " + EventDescription, NamedPipeCommunication.consoleNP);
                    }
                    else if (enableFileDebug == true)
                    {
                        LogToFile("[ERR!] " + ExtensiveInfo + EventDescription);
                    }
                    else if (OSERV.RunningFromCommandLine)
                    {
                        Console.WriteLine("ERR!: " + EventDescription);
                    }
                    else
                    {
                        eventlog.WriteEntry(EventDescription, elet, EventID);
                    }
                    break;
                // Generate warning message
                case EventLogEntryType.Warning:
                    if (enableRemoteDebug == true)
                    {
                        NamedPipeCommunication.sendMessageToServer("WARN: " + EventDescription, NamedPipeCommunication.consoleNP);
                    }
                    else if (enableFileDebug == true)
                    {
                        LogToFile("[WARN] " + ExtensiveInfo + EventDescription);
                    }
                    else if (OSERV.RunningFromCommandLine)
                    {
                        Console.WriteLine("WARN: " + EventDescription);
                    }
                    else
                    {
                        eventlog.WriteEntry(EventDescription, elet, EventID);
                    }
                    break;
                // Generate default information message
                default:
                    if(enableRemoteDebug == true)
                    {
                        NamedPipeCommunication.sendMessageToServer("INFO: " + EventDescription, NamedPipeCommunication.consoleNP);
                    }
                    else if (enableFileDebug == true)
                    {
                        LogToFile("[INFO] " + ExtensiveInfo + EventDescription);
                    }
                    else if (OSERV.RunningFromCommandLine)
                    {
                        Console.WriteLine("INFO: " + EventDescription);
                    }
                    else
                    {
                        if (EventID != 15) // DON'T WRITE NETWORK CHANGE ID'S WITH JUST INFORMATION TO EVENTLOG
                        {
                            eventlog.WriteEntry(EventDescription, elet, EventID);
                        }
                    }
                    break;
            }
        }
        private static void LogToFile(string line)
        {
            List<string> lines = new List<string>();
            lines.Add(line);
            LogToFile(lines);
        }
        private static void LogToFile(List<string> lines)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(OSERV.appConfig[13]))
                {
                    try
                    {
                        foreach (string line in lines)
                        {
                            sw.WriteLine(line);
                        }
                        sw.Close();
                    }
                    catch
                    {
                        sw.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                enableFileDebug = false;
                Dbg.LogEvent(ex.Message, EventLogEntryType.Error, false);
            }
        }
        public static bool enableRemoteDebug = false;
        public static bool enableFileDebug = false;
    }
    #endregion
}
