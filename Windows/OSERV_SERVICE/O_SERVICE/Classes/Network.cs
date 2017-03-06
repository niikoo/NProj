using OSERV_BASE.Functions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OSERV_BASE.Classes
{
    class Network
    {
        /// <summary>
        /// Give a string of basic connection info (Interface name:Status) - Line seperated and colon seperated
        /// </summary>
        /// <returns></returns>
        public static string infoCurrentNetwork()
        {
            string connectionInfo = "";
            System.Type oNSM = System.Type.GetTypeFromProgID("HNetCfg.HNetShare.1");
            dynamic comObject = System.Activator.CreateInstance(oNSM);
            dynamic conns = comObject.EnumEveryConnection;
            foreach (dynamic oItem in conns)
            {
                dynamic thisConnProps = comObject.NetConnectionProps(oItem);
                connectionInfo += thisConnProps.Name + ":" + thisConnProps.Status + "\r\n";
            }
            //Dbg.LogEvent("INFO:\r\n\r\n" + connectionInfo + "\r\n\r\n", EventLogEntryType.Information, 15);
            return connectionInfo;
        }

        private static string oldNetworkState;
        private static System.Threading.Timer timer;
        /// <summary>
        /// Network change listener
        /// </summary>
        public static void changeChecker()
        {
            try // END THREAD IF ALREADY STARTED
            {
                networkChangeCheckerThread.Abort();
                networkChangeCheckerThread.Join();
            }
            catch { }
            if (networkChangeCheckerThread != null) return;
            try
            {
                oldNetworkState = Network.infoCurrentNetwork();
                networkChangeCheckerThread = new Thread(() => NetworkChangeCheckerThread());
                networkChangeCheckerThread.Start();
                Dbg.LogEvent("[NETWORK] Change checker successfully started!", EventLogEntryType.Information, 15);
            }
            catch (Exception ex)
            {
                Dbg.LogEvent(ex.Message, EventLogEntryType.Error, false);
            }
        }
        /// <summary>
        /// Stop network change listner
        /// </summary>
        public static void stopChangeChecker()
        {
            networkChangeCheckerThread.Abort();
            networkChangeCheckerThread = null;
            Dbg.LogEvent("[NETWORK] Change checker successfully stopped!", EventLogEntryType.Information, 15);
        }
        private static Thread networkChangeCheckerThread = null;
        private static void NetworkChangeCheckerThread()
        {
            oldNetworkState = Network.infoCurrentNetwork();
            timer = new System.Threading.Timer(UpdateNetworkStatus, new Object(), new TimeSpan(0, 0, 0, 15, 0), new TimeSpan(0, 0, 0, 15, 0));
        }
        
        /// <summary>
        /// NetworkChangeChecker helper function
        /// </summary>
        /// <param name="o"></param>
        private static void UpdateNetworkStatus(object o)
        {
            string newNetworkState = Network.infoCurrentNetwork();
            if (oldNetworkState != newNetworkState)
            {
                oldNetworkState = newNetworkState;
                Dbg.LogEvent("[NETWORK] NETWORK HAS CHANGES!", EventLogEntryType.Warning, 15);
                OSERV.sessionChangeEvent(OSERV.sessionChangeReason.NETWORKCHANGE);
            }
        }
    }
}
