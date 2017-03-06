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
    static class RouterDDWRT
    {
        public static int classID()
        {
            return 7070;
        }
        /// <summary>
        /// Turn on wireless radio on the router configured in AppConfig
        /// 6: Router telnet ip (dd-wrt router)
        /// 7: TELNET PORT
        /// 8: TELNET User
        /// 9: TELNET Password
        /// </summary>
        public static void TurnOn()
        {
            Dbg.LogEvent("[DD-WRT] Turning on wireless radio on " + OSERV.appConfig[6], EventLogEntryType.Information, 6000);
            TelnetClientDDWRT(true);
        }
        /// <summary>
        /// Turn off wireless radio on the router
        /// See more information in the summary of the function TurnOn();
        /// </summary>
        public static void TurnOff()
        {
            Dbg.LogEvent("[DD-WRT] Turning off wireless radio on " + OSERV.appConfig[6], EventLogEntryType.Information, 6000);
            TelnetClientDDWRT(false);
        }

        public static void TelnetClientDDWRT(bool radioOn = false)
        {
            try
            {
                //create a new telnet connection
                TelnetConnection tc = new TelnetConnection(OSERV.appConfig[6], Int32.Parse(OSERV.appConfig[7]));

                //login with user "root",password "rootpassword", using a timeout of 100ms, and show server output
                string s = tc.Login(OSERV.appConfig[8], OSERV.appConfig[9], 100);
                //Console.Write(s);
                s += tc.Read();
                // server output should end with "$" or ">", otherwise the connection failed
                string prompt = s.TrimEnd();
                prompt = s.Substring(prompt.Length - 1, 1);
                if (prompt != "#" && prompt != ">")
                    throw new Exception("Connection failed");

                prompt = "";

                // while connected

                while (tc.IsConnected && prompt.Trim() != "exit")
                {
                    // display server output
                    tc.Read();
                    //Console.Write(tc.Read());

                    // send client input to server
                    if (radioOn == true)
                    {
                        tc.WriteLine("wl radio on");
                    }
                    else
                    {
                        tc.WriteLine("wl radio off");
                    }
                    // display server output
                    tc.Read();
                    //Console.Write(tc.Read());
                    // send client input to server
                    tc.WriteLine("exit");
                }
            }
            catch (Exception ex) { }
        }
    }
}
