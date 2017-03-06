using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSERV_BASE.Classes;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Reflection;

namespace OSERV_BASE.Functions
{
    static class GLOW
    {
        public static int classID()
        {
            return 4569;
        }
        /// -----------------------------
        /// | GLOW SCRIPT DOCUMENTATION |
        /// -----------------------------
        /// This is the "offical" 'GLOW Script' documentation included OSERV
        /// -----------------
        /// FIRST LINE CONFIG
        /// -----------------
        ///   RUN SCRIPT ON VALUES: (Multiple choices possible)
        ///     - [LOGON] [default]
        ///     - [LOGOFF]
        ///     - [LOCK]
        ///     - [UNLOCK]
        ///     - [START]
        ///     - [NETWORKCHANGE]
        ///   SCRIPTING LANGUAGES:
        ///     - {VBS} [default]
        ///   RUN AS:
        ///     - (USER) [default]
        ///     - (SERVICE)
        ///     
        /// EXAMPLES:
        ///     [LOGON][UNLOCK]{VBS}(USER)
        ///     [LOCK]{VBS}(SERVICE)
        ///     [START]
        ///     [UNLOCK](SERVICE)
        /// -----------------------------
        public enum ScriptType
        {
            [Description("LOGON")]
            Logon,
            [Description("LOGOFF")]
            Logoff,
            [Description("LOCK")]
            Lock,
            [Description("UNLOCK")]
            Unlock,
            [Description("START")]
            Start,
            [Description("NETWORKCHANGE")]
            NetworkChange
        }

        /// <summary>
        /// Function to start script exec
        /// </summary>
        /// <param name="run_on">When to run scripts</param>
        public static void Script(ScriptType run_at)
        {
            // Set when to run scripts
            RUN_GLOW_SCRIPT_ON = run_at;
            Dbg.LogEvent("[GLOW] Running scripts of type: " + StringEnum.GetDescription<ScriptType>(RUN_GLOW_SCRIPT_ON), classID());
            // Delete old temporary files
            DirectoryFunctions.DeleteFilesByWildcard("C:\\windows", OSERV.appConfig[1] + "*.vbs"); // Slett IG_temp filene
            DirectoryFunctions.DeleteEmptyFilesRecursively(DirectoryFunctions.Info(OSERV.appConfig[0])); // Slett tomme filer
            // Check if directory exists
            if (System.IO.Directory.Exists(OSERV.appConfig[0]))
            {
                // Scan directory, ScanDirectory_FileEvent will be triggered on 'File Found'
                ScanDirectory scanDir = new ScanDirectory();
                scanDir.SearchPattern = "";
                scanDir.FileEvent += new ScanDirectory.FileEventHandler(ScanDirectory_FileEvent);
                scanDir.WalkDirectory(OSERV.appConfig[0]);
                // Cleanup - Delete empty directories
                DirectoryFunctions.DeleteEmptyDirectoriesRecursively(DirectoryFunctions.Info(OSERV.appConfig[0]));
            }
            else
            {
                Dbg.LogEvent("[GLOW] Script directory not found: " + OSERV.appConfig[0], EventLogEntryType.Error, classID());
            }
        }
        /// <summary>
        /// Holder variable for selected script type
        /// </summary>
        private static ScriptType RUN_GLOW_SCRIPT_ON = ScriptType.Logon;
        /// <summary>
        /// RUN SCRIPT - FILE FOUND EVENT
        /// </summary>
        private static void ScanDirectory_FileEvent(object sender, FileEventArgs e)
        {
            // CLEANUP - delete empty files
            if (e.Info.Length == 0)
            {
                File.Delete(e.Info.FullName);
            }
            else if (e.Info.Extension == ".igs")
            { // CHECK FILE EXTENSION
                //Dbg.LogEvent("[GLOW] Found file: " + e.Info.FullName, EventLogEntryType.Information, classID());
                // Get and split file on newline
                string[] file_contents = PHP.explode("\n", PHP.file_get_contents(e.Info.FullName));
                // BBCODE REGEX
                Regex re = new Regex(@"\[(.*?)\]", RegexOptions.Singleline);
                string output = "LOGON";
                // Check for BBCODE-STYLE tags with run info, example: [LOGOFF]
                if (re.Match(file_contents[0]).Success)
                {
                    // TO SEMICOLON SEPERATED STRING
                    output = re.Replace(file_contents[0], "$1;");
                    output = output.Substring(0, output.Length - 1);
                }
                // IF THIS SCRIPT SHOULD BE RUNNED
                bool runthis = false;
                foreach (string runwhen in PHP.explode(";", output))
                {
                    if (runwhen.ToUpper() == StringEnum.GetDescription<ScriptType>(RUN_GLOW_SCRIPT_ON))
                    {
                        runthis = true;
                    }
                }
                if (runthis == true)
                {
                    // RUN AS USER OR SERVICE?
                    bool runAsService = false; // Run as user
                    if (file_contents[0].IndexOf("(SERVICE)") > -1)
                    {
                        runAsService = true; // Run as service
                    }
                    // Default scripting language
                    string filetype = "VBS";
                    // Detect specify-language tags, example: {VBS}
                    re = new Regex(@"\{(.*?)\}", RegexOptions.Singleline);
                    // DETECTED?
                    if (re.Match(file_contents[0]).Success)
                    {
                        // Get tag
                        filetype = re.Replace(file_contents[0], "$1");
                    }
                    // UNSET file_contents[0]
                    Array.Clear(file_contents, 0, 1);
                    // Check if the tag is supported
                    Dbg.LogEvent("[GLOW] Running script: " + e.Info.Name, EventLogEntryType.Information, classID());
                    switch (filetype)
                    {
                        default:
                            // PRESET VARIABLES FOR THE SCRIPT
                            string presetVariables = "";
                            // RUN
                            try
                            {
                                if (runAsService == true) // Run as service
                                {
                                    if (OSERV.RunningFromCommandLine == true)
                                    {
                                        Dbg.LogEvent("[GLOW] Can't exec script as service when running in command line mode", EventLogEntryType.Warning, classID());
                                    }
                                    else
                                    {
                                        OSERV.execVBS_AsService(presetVariables + PHP.implode("\r\n", file_contents), false);
                                    }
                                }
                                else // Run as user
                                {
                                    OSERV.execVBS(presetVariables + PHP.implode("\r\n", file_contents), false, OSERV.RunningFromCommandLine);
                                }
                            }
                            catch (Exception ex)
                            {
                                Dbg.LogEvent("[GLOW] The file could note be written or the VBScript failed to execute, exception: " + ex.Message, EventLogEntryType.Error, false, classID());
                            }
                            break;
                    }
                }
            }
        }
    }
}
