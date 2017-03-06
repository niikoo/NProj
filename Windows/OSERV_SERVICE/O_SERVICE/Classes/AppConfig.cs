using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace OSERV_BASE.Classes
{
    class AppConfig
    {
        public static string Get(string name) {
            string returner = "";
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\OMMUNDSEN SERVICES\\OSERV\\", true))
                {
                    try
                    {
                        returner = key.GetValue(name).ToString();
                    }
                    catch
                    {
                        throw new Exception("Couldn't read setting: " + name);
                    }
                    key.Close();
                }
            }
            catch
            {
                throw new Exception("Couldn't find settings");
            }
            return returner;
        }
        public static void Set(string name, string value)
        {
            RegistryKey hklm_software = Registry.LocalMachine.OpenSubKey("SOFTWARE\\", true);
            hklm_software.CreateSubKey("OMMUNDSEN SERVICES").CreateSubKey("OSERV");
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\OMMUNDSEN SERVICES\\OSERV\\", true))
            {
                try
                {
                    key.SetValue(name,value);
                }
                catch
                {
                    throw new Exception("Couldn't write setting: " + name);
                }
                key.Close();
            }
        }
    }
}
