using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace OSERV_BASE.Classes
{
    class ServiceInstallerUninstaller
    {
        /// <summary>
        /// Installs a service
        /// </summary>
        /// <param name="pathToServiceToInstall">Path to service exe, if it's empty select current assembly</param>
        public static void Install(string pathToServiceToInstall = "")
        {
            try
            {
                if (pathToServiceToInstall == "")
                {
                    pathToServiceToInstall = System.Reflection.Assembly.GetExecutingAssembly().Location;
                }
                Console.WriteLine("Installing service exe: " + pathToServiceToInstall);
                string path = @"C:\WINDOWS\Microsoft.NET\Framework64\v4.0.30319";
                if(System.IO.Directory.Exists(path)) {
                    Process myProcess = new Process();
                    myProcess.StartInfo.FileName = path + "\\InstallUtil.exe";
                    myProcess.StartInfo.Arguments = "\"" + pathToServiceToInstall + "\"";
                    Console.WriteLine(myProcess.StartInfo.FileName + " " + myProcess.StartInfo.Arguments);
                    if (!System.IO.File.Exists(pathToServiceToInstall))
                    {
                        throw new Exception("Service executable to install: " + pathToServiceToInstall + " was not found!");
                    }
                    myProcess.StartInfo.CreateNoWindow = true;
                    Console.WriteLine("STARTING INSTALLATION");
                    myProcess.Start();
                    myProcess.WaitForExit(60000);
                    Console.WriteLine("INSTALLATION FINISHED");
                    if (!myProcess.HasExited)
                        myProcess.Kill();
                    myProcess.Close();
                } else {
                    MessageBox.Show("Microsoft .NET Framework 4.0 x64 not found! Please install .NET Framework 4.0 x64 to continue.");
                }
            } catch(Exception ex) {
                MessageBox.Show("Installation failed, exception message: " + ex.Message);
            }
        }
        /// <summary>
        /// Uninstalls a service
        /// </summary>
        /// <param name="pathToServiceToInstall">Path to service exe, if it's empty select current assembly</param>
        public static void Uninstall(string pathToServiceToInstall = "")
        {
            try
            {
                if (pathToServiceToInstall == "")
                {
                    pathToServiceToInstall = System.Reflection.Assembly.GetExecutingAssembly().Location;
                }
                Console.WriteLine("Uninstalling service exe: " + pathToServiceToInstall);
                string path = @"C:\WINDOWS\Microsoft.NET\Framework64\v4.0.30319";
                if(System.IO.Directory.Exists(path)) {
                    Process myProcess = new Process();
                    myProcess.StartInfo.FileName = path + "\\InstallUtil.exe";
                    myProcess.StartInfo.Arguments = "/u \"" + pathToServiceToInstall + "\"";
                    myProcess.StartInfo.CreateNoWindow = false;
                    Console.WriteLine("STARTING UNINSTALLATION");
                    myProcess.Start();
                    myProcess.WaitForExit(60000);
                    Console.WriteLine("UNINSTALLATION ENDED");
                    if (!myProcess.HasExited)
                        myProcess.Kill();
                    myProcess.Close();
                } else {
                    MessageBox.Show("Microsoft .NET Framework 4.0 x64 not found! Please install .NET Framework 4.0 x64 to continue.");
                }
            } catch(Exception ex) {
                MessageBox.Show("Uninstallation failed, exception message: " + ex.Message);
            }
        }
    }
}
