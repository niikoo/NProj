using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceProcess;
using System.Diagnostics;

namespace OSERV_BASE.Classes
{
    class ServiceManager
    {
        public static int classID()
        {
            return 778647;
        }
        /// <summary>
        /// Starts a windows service
        /// </summary>
        /// <param name="serviceName">The name of the service</param>
        /// <param name="timeoutMilliseconds">Timeout</param>
        private static Thread startServiceThread;
        public static void Start(string serviceName, int timeoutMilliseconds)
        {
            /*if (startServiceThread.IsAlive)
            {
                startServiceThread.Abort();
                startServiceThread.Join();
            }*/
            try
            {
                startServiceThread = new Thread(() => StartThread(serviceName, timeoutMilliseconds));
                startServiceThread.Start();
            }
            catch (Exception ex)
            {
                Dbg.LogEvent(ex.Message, EventLogEntryType.Error, false, classID());
            }
        }
        /* THE WORKER THREAD */
        private static void StartThread(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                Dbg.LogEvent("Starting the service '" + serviceName + "'", EventLogEntryType.Information, classID());
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
                service.Start();
            }
            catch (Exception ex)
            {
                Dbg.LogEvent("Couldn't start service: " + serviceName + " -> Message: " + ex.Message, EventLogEntryType.Warning, classID());
            }
            finally
            {
                Dbg.LogEvent("Successfully started the service '" + serviceName + "'", classID());
            }
            startServiceThread.Abort();
            startServiceThread.Join();
        }
        /// <summary>
        /// Stops a windows service
        /// </summary>
        /// <param name="serviceName">The name of the service</param>
        /// <param name="timeoutMilliseconds">Timeout</param>
        public static void Stop(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                Dbg.LogEvent("Stopping the service '" + serviceName + "'");
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            }
            catch //(Exception ex)
            {
                //Dbg.LogEvent("Couldn't stop service: " + serviceName + " -> Message: " + ex.Message, EventLogEntryType.Warning);
            }
            finally
            {
                Dbg.LogEvent("Successfully stopped the service '" + serviceName + "'", EventLogEntryType.Information, classID());
            }
        }
    }
}
