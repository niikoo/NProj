using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;


namespace OSERV_BASE
{
    [RunInstaller(true)]
    public partial class ServiceInstaller : System.Configuration.Install.Installer
    {
        public ServiceInstaller()
        {
            InitializeComponent();
            ServiceProcessInstaller pi = new ServiceProcessInstaller();
            System.ServiceProcess.ServiceInstaller si = new System.ServiceProcess.ServiceInstaller();

            //If the service is running under LocalSystem it cannot launch processes
            //under new credentials, any attempt to do so will throw a Win32Exception
            //(ERROR_ACCESS_DENIED).
            pi.Account = ServiceAccount.LocalSystem;
            si.ServiceName = OSERV.serviceName;
            si.Description = OSERV.serviceName;
            si.DisplayName = OSERV.serviceName;
            si.StartType = ServiceStartMode.Automatic;
            this.Installers.AddRange(new Installer[] { pi, si });
        }
        public override void Install(IDictionary savedState)
        {
            base.Install(savedState);
            //Add custom code here
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
            //Add custom code here
        }

        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
            //Add custom code here
        }

        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
            //Add custom code here
        }
    }
}
