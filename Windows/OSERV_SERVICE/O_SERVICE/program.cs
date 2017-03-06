using System;
using System.ServiceProcess;
using System.Configuration.Install;
using System.Reflection;
using System.Security.Principal;
using OSERV_BASE.Classes;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Management;

namespace OSERV_BASE
{
    static class Program
    {
        [Flags]
        enum ExitCodes : int
        {
            Success = 0,
            SignToolNotInPath = 1,
            AssemblyDirectoryBad = 2,
            PFXFilePathBad = 4,
            PasswordMissing = 8,
            SignFailed = 16,
            UnknownError = 32
        }
        static void Main(string[] args)
        {
            OSERV service = new OSERV();
            if (System.Environment.UserInteractive) // COMMAND MODE
            {
                OSERV.RunningFromCommandLine = true; // SET RunningFromCommandLine IN OSERV
                //string parameter = string.Concat(args); // OLD
                if (OSERV.IsUserAdministrator())
                {
                    /*if (Environment.UserName.ToLower() != OSERV.appConfig[5].ToLower())
                    {
                        Console.WriteLine("You need to use the 'Administrator' user to run IRIS-Service in CLI interface");
                        Environment.Exit((int)ExitCodes.PasswordMissing);
                    }*/
                    Console.Clear();
                    Console.WriteLine("");
                    Console.WriteLine("#############################");
                    Console.WriteLine("## OSERV                   ##");
                    Console.WriteLine("#############################");
                    Console.WriteLine("## OMMUNDSEN SERVICES 2015 ##");
                    Console.WriteLine("##                         ##");
                    Console.WriteLine("## OSERV MANAGEMENT        ##");
                    Console.WriteLine("## COMMAND LINE INTERFACE  ##");
                    Console.WriteLine("#############################");
                    Console.WriteLine("");
                    Console.WriteLine("OSERV version: " + Application.ProductVersion);
                    Console.WriteLine("");
                    if(args.Length > 0) {
                        switch (args[0])
                        {
                            // RUN SERVICE IN CLI MODE
                            case "/i":
                            case "--cli":
                                service.LaunchCommandLine();
                                break;
                            // INSTALL SERVICE
                            case "--install":
                                try
                                {
                                    //ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                                    // INSTALL IRIS-Service
                                    Console.WriteLine("Installing " + OSERV.serviceName);
                                    Classes.ServiceInstallerUninstaller.Install();
                                    // START IRIS-Service
                                    Thread.Sleep(500);
                                    ServiceController controller = new ServiceController(OSERV.serviceName);
                                    controller.Start();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Coulnd't install " + OSERV.serviceName + ", exception: " + ex.Message);
                                    Thread.Sleep(3000);
                                    throw ex;
                                }
                                break;
                            // UNINSTALL SERVICE
                            case "--uninstall":
                                try
                                {
                                    //ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                                    // STOP,UNINSTALL,KILL AND DELETE IRIS-Service
                                    try
                                    {
                                        Console.WriteLine("LOAD SERVICE");
                                        ServiceController controller = new ServiceController(OSERV.serviceName);
                                        if (controller.Status == ServiceControllerStatus.Running)
                                        {
                                            Console.WriteLine("Stopping");
                                            controller.Stop();
                                            controller.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds(10000));
                                        }
                                        try
                                        {
                                            Console.WriteLine("GET LOCATION");
                                            // GET EXE LOCATION
                                            WqlObjectQuery wqlObjectQuery = new WqlObjectQuery(string.Format("SELECT * FROM Win32_Service WHERE Name = '{0}'", OSERV.serviceName));
                                            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(wqlObjectQuery);
                                            ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
                                            
                                            // UNINSTALL DELETE EXE FILE
                                            foreach (ManagementObject managementObject in managementObjectCollection)
                                            {
                                                string serviceEXEPath = "C:\\EXE-FILE_NOT-FOUND.EXE";
                                                try
                                                {
                                                    if (System.IO.File.Exists(managementObject.GetPropertyValue("PathName").ToString()))
                                                    {
                                                        Console.WriteLine("The path to the current service executable doesn't contain space(s).");
                                                        serviceEXEPath = managementObject.GetPropertyValue("PathName").ToString();
                                                    }
                                                }
                                                catch { }
                                                try
                                                {
                                                    string servicepath = managementObject.GetPropertyValue("PathName").ToString().Substring(1);
                                                    servicepath = servicepath.Substring(0, (servicepath.Length - 1));
                                                    if (System.IO.File.Exists(servicepath))
                                                    {
                                                        Console.WriteLine("The path to the current service executable contain space(s).");
                                                        serviceEXEPath = servicepath;
                                                    }
                                                }
                                                catch { }
                                                if(System.IO.File.Exists(serviceEXEPath)) {
                                                    Classes.ServiceInstallerUninstaller.Uninstall(serviceEXEPath);
                                                    Console.WriteLine("KILL");
                                                    // KILL PROCESS
                                                    Process[] processes = System.Diagnostics.Process.GetProcessesByName(OSERV.serviceName);
                                                    foreach (Process process in processes)
                                                    {
                                                        //process.EnableRaisingEvents = true;
                                                        process.Kill();
                                                    }
                                                    System.IO.File.Delete(serviceEXEPath);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("Couldn't uninstall " + OSERV.serviceName + ", exception: " + ex.Message);
                                        }
                                    }
                                    catch { }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Coulnd't uninstall " + OSERV.serviceName + ", exception:" + ex.Message);
                                }
                                break;
                            // SENDS A COMMAND DIRECTLY
                            case "--sendCommand":
                                try
                                {
                                    Classes.NamedPipeCommunication.sendMessageToServer(args[1],null,true);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Couldn't send named pipe message to server because " + ex.Message);
                                }
                                break;
                            case "/?":
                            case "--help":
                            default:
                                Console.WriteLine("|------------------------------------|");
                                Console.WriteLine("|    Command    |      Function      |");
                                Console.WriteLine("|------------------------------------|");
                                Console.WriteLine("| <no argument> | Show CLI           |");
                                Console.WriteLine("| --install     | Install Service    |");
                                Console.WriteLine("| --uninstall   | Uninstall Service  |");
                                Console.WriteLine("| --refresh     | Proxy refresh      |");
                                Console.WriteLine("|               |                    |");
                                Console.WriteLine("|             Misc.                  |");
                                Console.WriteLine("|               |                    |");
                                Console.WriteLine("| --sendCommand | Send to service(NP)|");
                                Console.WriteLine("| --cli         | Run in CLI-mode    |");
                                Console.WriteLine("| --help        | Display this help  |");
                                Console.WriteLine("|------------------------------------|");
                                Console.WriteLine("|     Alias     |     Alias For?     |");
                                Console.WriteLine("|------------------------------------|");
                                Console.WriteLine("| /i            | --cli              |");
                                Console.WriteLine("| /?            | --help             |");
                                Console.WriteLine("| <unknown arg> | --help             |");
                                Console.WriteLine("|------------------------------------|");
                                break;
                        }
                        Environment.Exit((int)ExitCodes.Success);
                        return;
                    }
                    Console.WriteLine("Need help? Use the 'help' command");
                    string input = "";
                    Thread nps = new Thread(() => NamedPipeCommunication.ThreadStartNPServer(NamedPipeCommunication.consoleNP));
                    nps.Start();
                    while (true)
                    {
                        Console.Write("\r\n#>");
                        input = Console.ReadLine();
                        if (input.ToLower() == "exit") break;
                        input = PHP.str_replace(" ", "::", input);
                        NamedPipeCommunication.sendMessageToServer(input);
                        // Wait 2 sec for data return
                        for(int i = 0; i <= 10; i++) {
                            if (NamedPipeCommunication.dataRecived == true)
                            {
                                // Special rule for debug (show disable dialog)
                                if (input.ToLower() == "debug::console")
                                {
                                    EndDebug ed_form = new EndDebug();
                                    ed_form.ShowDialog();
                                }
                                NamedPipeCommunication.dataRecived = false;
                                break;
                            }
                            Thread.Sleep(100);
                            if (i == 100)
                            {
                                Console.WriteLine("ERROR! NO DATA RETURNED");
                            }
                        }
                    }
                    nps.Abort();
                    Environment.Exit((int)ExitCodes.Success);
                }
                else if (args.Length > 0) // RUN THESE WITHOUT ADMIN RIGHTS
                {
                    // NOTHIN' HERE HAAHAAAAAA =D
                }
                // RESTART APP AND REQUEST ADMINISTRATIVE PRIVILEGIES //
                try
                {
                    ProcessStartInfo processInfo = new ProcessStartInfo();
                    processInfo.Verb = "runas";
                    processInfo.FileName = Application.ExecutablePath;
                    processInfo.Arguments = PHP.implode(" ", args);
                    Process.Start(processInfo);
                    Environment.Exit((int)ExitCodes.Success);
                }
                catch
                {
                    Console.WriteLine(OSERV.serviceName + " NEEDS ADMINISTRATIVE RIGHTS TO CONTINUE");
                    Console.ReadKey();
                    Environment.Exit((int)ExitCodes.UnknownError);
                }
                Environment.Exit((int)ExitCodes.Success);
            }
            else // NORMAL MODE (SERVICE MODE)
            {
                //Standard service entry point.
                ServiceBase.Run(service);
            }
            return;
        }
    }
}