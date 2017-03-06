using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;

namespace OSERV_BASE
{
    [RunInstaller(true)]
    public partial class ServiceInstall : System.Configuration.Install.Installer
    {
        public ServiceInstall()
        {
            InitializeComponent();
        }
        //Standard service installation.
        /*public ServiceInstall
        {
           ServiceProcessInstaller pi = new ServiceProcessInstaller();
            ServiceInstaller si = new ServiceInstaller();

            //If the service is running under LocalSystem it cannot launch processes
            //under new credentials, any attempt to do so will throw a Win32Exception
            //(ERROR_ACCESS_DENIED).
            pi.Account = ServiceAccount.LocalSystem;
            si.ServiceName = "IRISService";
            si.Description = "IRISService";
            si.DisplayName = "IRISService";
            si.StartType = ServiceStartMode.Automatic;
            this.Installers.AddRange(new Installer[] { pi, si });
            
        }*/
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
            EventLog.CreateEventSource("IRIS-Service-Installer", "Application");
            EventLog.WriteEntry("IRIS-Service-Installer", "Eventsource created");
        }
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
        }
        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }
        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
        }
    }
}
