using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace OSERV_BASE.Classes
{
    /// <summary>
    /// Dynamically choose what functions to run.
    /// Defined by an config file (see string configFile)
    /// </summary>
    static class Dynamics
    {
        private static int classID()
        {
            return 3962;
        }

        private static string configFile = OSERV.appConfig[0] + "functions.conf";

        public enum FunctionList : int
        {
            APPLIMITER = 0,
            GLOW = 1,
            ICS = 2,
            ROUTER = 3
        }

        private static List<FunctionList> activatedFunctions = new List<FunctionList>();

        private static bool configLoaded = false;

        private static bool noConfig = false;

        public static bool CanRun(FunctionList function)
        {
            if (configLoaded == false)
            {
                if (File.Exists(configFile))
                {
                    activatedFunctions.Clear(); // Clear list
                    List<String> appsToLimit = PHP.file(configFile);
                    int counter = 0;
                    string[] appex;
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
                        // FunctionID (int from enum: FunctionList)
                        // (bool) Activate function: 'true' or 'false'
                        // Optional: comma seperated list over computer names to exclude
                        try
                        {
                            bool excludeApp = false;
                            if (appex.Count() == 2 || appex.Count() == 3)
                            {
                                int FunctionID = Convert.ToInt32(appex[0]);
                                FunctionList FunctionEnum = (FunctionList)Enum.ToObject(typeof(FunctionList), FunctionID);
                                if (appex.Count() == 3 && appex[2].Length > 0)
                                {
                                    try
                                    {
                                        String[] excluded = PHP.explode(",", appex[2]);
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
                                if (appex[1].ToLower() != "true")
                                {
                                    excludeApp = true;
                                }
                                if (excludeApp == false)
                                {
                                    activatedFunctions.Add(FunctionEnum);
                                    Dbg.LogEvent("[DYNAMICS] Added function: " + FunctionEnum.ToString(), classID()); // needs to desc
                                }
                                else
                                {
                                    Dbg.LogEvent("[DYNAMICS] Skipped function: " + FunctionEnum.ToString(), classID()); // needs to desc
                                }
                            }
                            else
                            {
                                throw new Exception("SYNTAX ERROR");
                            }
                        }
                        catch
                        {
                            Dbg.LogEvent("[DYNAMICS] Syntax error on line " + counter + " in " + configFile, EventLogEntryType.Warning, classID());
                        }
                    }
                    configLoaded = true;
                }
                else
                {
                    Dbg.LogEvent("[DYNAMICS] The configuration file " + configFile + " doesn't exist! DYNAMICS will enable all functions!", EventLogEntryType.Warning, classID());
                    configLoaded = true;
                    noConfig = true;
                    return false;
                }
            }
            if (activatedFunctions.Contains(function) || noConfig == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}