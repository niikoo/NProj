using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Pipes;
using System.IO;
using System.Threading;
using OSERV_BASE.Functions;

namespace OSERV_BASE.Classes
{
    /*
     * Using named pipe to send simple function from console window to service 
     **/
    public static class NamedPipeCommunication
    {
        public static string consoleNP = "OSERV_console";
        // SUPER MANUAL "EVENT" xD
        public static bool dataRecived = false;
        /// <summary>
        /// Start the direct output NPC-server in another thread
        /// </summary>
        /// <param name="namedPipeName">The name of the named pipe, if it's null it uses the defualt name</param>
        public static void ThreadStartNPServer(string namedPipeName = null)
        {
            if (namedPipeName == null)
            {
                namedPipeName = PHP.md5(OSERV_BASE.OSERV.serviceName + OSERV.getVersion());
            }
            //Dbg.LogEvent("[NPS] Debug thread started",130);
            // Create a loop
            bool keepThreadRunning = true;
            while (keepThreadRunning)
            {
                try
                {
                    // Create a named pipe
                    using (NamedPipeServerStream pipeStream = new NamedPipeServerStream(namedPipeName))
                    {
                        //Dbg.LogEvent("[NPS] Pipe created: " + pipeStream.GetHashCode(), 130);

                        // Wait for a connection
                        pipeStream.WaitForConnection();
                        //Dbg.LogEvent("[NPS] Pipe connection established", 130);

                        using (StreamReader sr = new StreamReader(pipeStream))
                        {
                            string temp;
                            // We read a line from the pipe and print it together with the current time
                            while ((temp = sr.ReadLine()) != null)
                            {
                                if (temp == "!!FIN!!")
                                {
                                    dataRecived = true;
                                }
                                else
                                {
                                    Console.WriteLine(temp);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Dbg.LogEvent("[NPS] Couldn't create named pipe: " + ex.Message, System.Diagnostics.EventLogEntryType.Error, 130);
                    keepThreadRunning = false;
                }
            }
            //Dbg.LogEvent("[NPS] Thread ended", 130);
        }
        /// <summary>
        /// Start the command NPC-server in another thread
        /// </summary>
        /// <param name="namedPipeName">The name of the named pipe, if it's null it uses the defualt name</param>
        public static void ThreadStartCommandServer(string namedPipeName = null)
        {
            if (namedPipeName == null)
            {
                namedPipeName = PHP.md5(OSERV_BASE.OSERV.serviceName + OSERV.getVersion());
            }
            Dbg.LogEvent("[NPS] Thread started",130);
            // Create a loop
            bool keepThreadRunning = true;
            while (keepThreadRunning)
            {
                try
                {
                    // Create a named pipe
                    using (NamedPipeServerStream pipeStream = new NamedPipeServerStream(namedPipeName))
                    {
                        Dbg.LogEvent("[NPS] Pipe created: " + pipeStream.GetHashCode(), 130);

                        // Wait for a connection
                        pipeStream.WaitForConnection();
                        Dbg.LogEvent("[NPS] Pipe connection established", 130);

                        using (StreamReader sr = new StreamReader(pipeStream))
                        {
                            string temp;
                            // We read a line from the pipe and print it together with the current time
                            while ((temp = sr.ReadLine()) != null)
                            {
                                string[] command = PHP.explode("::", PHP.trim(temp));
                                if(command.Length > 0)
                                {
                                    Dbg.LogEvent("[NPS] Function: " + command[0], 130);
                                    bool functionExec = true;
                                    switch (command[0])
                                    {
                                        // HELP
                                        case "?":
                                        case "help":
                                            sendMessageToServer("|--------------------------------------------------------------|", consoleNP);
                                            sendMessageToServer("|    Command    |        Arguments        |        Info        |", consoleNP);
                                            sendMessageToServer("|--------------------------------------------------------------|", consoleNP);
                                            sendMessageToServer("| debug         | console,file,disable    | Debug mode         |", consoleNP);
                                            sendMessageToServer("| event         | logon,logoff,lock,      | Fire 'system' event|", consoleNP);
                                            sendMessageToServer("|               | unlock, networkchange,  |                    |", consoleNP);
                                            sendMessageToServer("|               | resume, suspend         | (power events)     |", consoleNP);
                                            sendMessageToServer("| glow          | <GLOW SCRIPTS TYPE>     | Run a type of G.S. |", consoleNP);
                                            sendMessageToServer("| ics           | setup                   | ICS control cmds.  |", consoleNP);
                                            sendMessageToServer("| ddwrt         | radio-on, radio-off     | DD-WRT Router cmds.|", consoleNP);
                                            sendMessageToServer("| help          |                         | Show help          |", consoleNP);
                                            sendMessageToServer("|--------------------------------------------------------------|", consoleNP);
                                            sendMessageToServer("| Command line interface syntax:                               |", consoleNP);
                                            sendMessageToServer("|                                                              |", consoleNP);
                                            sendMessageToServer("|     Command with arguments:      <command> <arguments>       |", consoleNP);
                                            sendMessageToServer("|                              EX: glow NETWORKCHANGE          |", consoleNP);
                                            sendMessageToServer("|     Command without arguments:   <command>                   |", consoleNP);
                                            sendMessageToServer("|                              EX: help                        |", consoleNP);
                                            sendMessageToServer("|                                                              |", consoleNP);
                                            sendMessageToServer("|--------------------------------------------------------------|", consoleNP);
                                            break;
                                        // DEBUGMODE
                                        case "debug":
                                            try
                                            {
                                                switch (command[1])
                                                {
                                                    case "console":
                                                        // After editing: Remember to check program.cs -> special rule because of GUI
                                                        sendMessageToServer("Debug mode enabled\r\n\r\n", consoleNP);
                                                        Dbg.enableRemoteDebug = true;
                                                        //Dbg.LogEvent("[NPS] Debug mode disabled", System.Diagnostics.EventLogEntryType.Information, false, 135);
                                                        break;
                                                    case "file":
                                                        sendMessageToServer("Debug to file enabled\r\nLog file: " + OSERV.appConfig[2] + "\r\n/!\\ Remember to disable debug mode when finished! /!\\", consoleNP);
                                                        AppConfig.Set("debugToFile", "true");
                                                        Dbg.enableFileDebug = true;
                                                        break;
                                                    case "disable":
                                                        sendMessageToServer("Debug mode disabled\r\n\r\n", consoleNP);
                                                        // DISABLE DEBUG TO FILE
                                                        AppConfig.Set("debugToFile", "false");
                                                        Dbg.enableFileDebug = false;
                                                        // DISABLE REMOTE DEBUG
                                                        Dbg.enableRemoteDebug = false;
                                                        //Dbg.LogEvent("[NPS] Debug mode disabled",System.Diagnostics.EventLogEntryType.Information, false,135);
                                                        break;
                                                    default:
                                                        sendMessageToServer("Unknown arguments, known arguments: [console|file|disable]", consoleNP);
                                                        break;
                                                }
                                            }
                                            catch {
                                                sendMessageToServer("Missing arguments [console|file|disable]", consoleNP);
                                            }
                                            break;
                                        // RUN EVENTS
                                        case "event":
                                            try
                                            {
                                                switch (command[1])
                                                {
                                                    case "logon":
                                                        OSERV.sessionChangeEvent(OSERV.sessionChangeReason.LOGON);
                                                        break;
                                                    case "logoff":
                                                        OSERV.sessionChangeEvent(OSERV.sessionChangeReason.LOGOFF);
                                                        break;
                                                    case "lock":
                                                        OSERV.sessionChangeEvent(OSERV.sessionChangeReason.LOCK);
                                                        break;
                                                    case "unlock":
                                                        OSERV.sessionChangeEvent(OSERV.sessionChangeReason.UNLOCK);
                                                        break;
                                                    case "networkchange":
                                                        OSERV.sessionChangeEvent(OSERV.sessionChangeReason.NETWORKCHANGE);
                                                        break;
                                                    case "resume":
                                                        OSERV.sessionChangeEvent(OSERV.sessionChangeReason.RESUME);
                                                        break;
                                                    case "suspend":
                                                        OSERV.sessionChangeEvent(OSERV.sessionChangeReason.SUSPEND);
                                                        break;
                                                    default:
                                                        sendMessageToServer("Unknown argument, known arguments: [logon|logoff|lock|unlock|networkchange|resume|suspend]", consoleNP);
                                                        Dbg.LogEvent("Could not run event " + command[1] + " because it doesn't exist", System.Diagnostics.EventLogEntryType.Information, false, 135);
                                                        break;
                                                }
                                                //Dbg.LogEvent("Event forced: " + command[1], System.Diagnostics.EventLogEntryType.Information, false, 135);
                                            }
                                            catch {
                                                sendMessageToServer("Missing arguments [logon|logoff|lock|unlock|networkchange|resume|suspend]", consoleNP);
                                            }
                                            break;
                                        // RUN DD-WRT ROUTER FUNCTIONS
                                        case "ddwrt":
                                            try
                                            {
                                                switch (command[1])
                                                {
                                                    case "radio-on":
                                                        RouterDDWRT.TurnOn();
                                                        break;
                                                    case "radio-off":
                                                        RouterDDWRT.TurnOff();
                                                        break;
                                                    default:
                                                        sendMessageToServer("Unknown argument, known arguments: [radio-on|radio-off]", consoleNP);
                                                        Dbg.LogEvent("Could not run event " + command[1] + " because it doesn't exist", System.Diagnostics.EventLogEntryType.Information, false, 135);
                                                        break;
                                                }
                                                //Dbg.LogEvent("Event forced: " + command[1], System.Diagnostics.EventLogEntryType.Information, false, 135);
                                            }
                                            catch
                                            {
                                                sendMessageToServer("Missing arguments [radio-on|radio-off]", consoleNP);
                                            }
                                            break;
                                        // RUN DD-WRT ROUTER FUNCTIONS
                                        case "ics":
                                            try
                                            {
                                                switch (command[1])
                                                {
                                                    case "setup":
                                                        VPN_ICS.SetupICS();
                                                        break;
                                                    default:
                                                        sendMessageToServer("Unknown argument, known arguments: [setup]", consoleNP);
                                                        Dbg.LogEvent("Could not run event " + command[1] + " because it doesn't exist", System.Diagnostics.EventLogEntryType.Information, false, 135);
                                                        break;
                                                }
                                                //Dbg.LogEvent("Event forced: " + command[1], System.Diagnostics.EventLogEntryType.Information, false, 135);
                                            }
                                            catch
                                            {
                                                sendMessageToServer("Missing arguments [setup]", consoleNP);
                                            }
                                            break;
                                        // GLOW FORCE RUN
                                        case "glow":
                                            try
                                            {
                                                bool runnedGLOWScripts = false;
                                                foreach (GLOW.ScriptType value in Enum.GetValues(typeof(GLOW.ScriptType)))
                                                {
                                                    if (StringEnum.GetDescription<GLOW.ScriptType>(value) == command[1].ToUpper())
                                                    {
                                                        GLOW.Script(value);
                                                        sendMessageToServer("Started GLOW-scripts of type " + command[1].ToUpper(), consoleNP);
                                                        runnedGLOWScripts = true;
                                                    }
                                                }
                                                if(runnedGLOWScripts == false) {
                                                    sendMessageToServer(command[1].ToUpper() + " is an unknown type of GLOW-script!", consoleNP);
                                                    List<string> GStypes = new List<string>();
                                                    foreach (GLOW.ScriptType value in Enum.GetValues(typeof(GLOW.ScriptType)))
                                                    {
                                                        GStypes.Add(StringEnum.GetDescription<GLOW.ScriptType>(value));
                                                    }
                                                    sendMessageToServer("Known GLOW-script types: " + PHP.implode(",",GStypes), consoleNP);
                                                }

                                            }
                                            catch {
                                                sendMessageToServer("Unknown error", consoleNP);
                                            }
                                            break;
                                        default:
                                            functionExec = false;
                                            sendMessageToServer("Unknown function!", consoleNP);
                                            break;
                                    }
                                    if (functionExec == true)
                                    {
                                        Dbg.LogEvent("[NPS] Finished running function: " + command[0], 130);
                                        sendMessageToServer("!!FIN!!", consoleNP);
                                    }
                                }
                                else
                                {
                                    sendMessageToServer("Couldn't understand recived data!", consoleNP);
                                    sendMessageToServer("!!FIN!!", consoleNP);
                                }
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    Dbg.LogEvent("[NPS] Couldn't create named pipe: " + ex.Message, System.Diagnostics.EventLogEntryType.Error, 130);
                    keepThreadRunning = false;
                }
            }
            Dbg.LogEvent("[NPS] Thread ended", 130);
        }

        /// <summary>
        /// Sends a message to a named pipe server
        /// </summary>
        /// <param name="Message">The message to send</param>
        /// <param name="namedPipeName">The named pipe to send to (null gives the default service named pipe)</param>
        /// <param name="debugInfo">Display debuginfo, default: false</param>
        public static void sendMessageToServer(string Message, string namedPipeName = null,bool debugInfo = false)
        {
            if (namedPipeName == null)
            {
                namedPipeName = PHP.md5(OSERV_BASE.OSERV.serviceName + OSERV.getVersion());
            }
            if (debugInfo)
            {
                Dbg.LogEvent("[NPC] Contacting running OSERV...", 131);
            }
            try
            {
                using (NamedPipeClientStream pipeStream = new NamedPipeClientStream(namedPipeName))
                {
                    // The connect function will indefinately wait for the pipe to become available
                    // If that is not acceptable specify a maximum waiting time (in ms)
                    pipeStream.Connect(250);
                    using (StreamWriter sw = new StreamWriter(pipeStream))
                    {
                        sw.AutoFlush = true;
                        sw.WriteLine(Message);
                        if (debugInfo)
                        {
                            Dbg.LogEvent("[NPC] Command sent!", 131);
                        }
                    }
                }
            } catch {
                Dbg.LogEvent("[NPC] OSERV NOT RUNNING? (or version incompatibility?)", System.Diagnostics.EventLogEntryType.Error, 131);
            }
        }
    }      
}

