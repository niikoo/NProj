using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OSERV_BASE.Classes;
using System.Diagnostics;
using OSERV_BASE.Impersonation;
using System.Runtime.InteropServices;

namespace OSERV_BASE.Functions
{
    class VPN_ICS
    {
        private enum VPN_Status : uint
        {
            UNKNOWN = 0,
            DISCONNECTED = 1,
            CONNECTED = 2
        }
        private static VPN_Status ConnectionInfo = VPN_Status.UNKNOWN;
        /// <summary>
        /// Set ICS to work with VPN if connected. If the VPN is not connected, use ICS without VPN.
        /// </summary>
        public static void SetupICS()
        {
            try
            {
                System.Type oNSM = System.Type.GetTypeFromProgID("HNetCfg.HNetShare.1");
                dynamic comObject = System.Activator.CreateInstance(oNSM);
                dynamic conns = comObject.EnumEveryConnection;
                foreach (dynamic oItem in conns)
                {
                    dynamic thisConnProps = comObject.NetConnectionProps(oItem);
                    if(thisConnProps.Name == OSERV.appConfig[3] || thisConnProps.Name == OSERV.appConfig[4] || thisConnProps.Name == OSERV.appConfig[5])
                    {
                        comObject.INetSharingConfigurationForINetConnection(oItem).DisableSharing();
                    }
                }
                conns = comObject.EnumEveryConnection;
                foreach (dynamic oItem in conns)
                {
                    dynamic thisConnProps = comObject.NetConnectionProps(oItem);
                    if (thisConnProps.Name == OSERV.appConfig[3] && thisConnProps.Status == 2)
                    {
                        comObject.INetSharingConfigurationForINetConnection(oItem).EnableSharing(0);
                        ConnectionInfo = VPN_Status.CONNECTED;
                        Dbg.LogEvent("[ICS] VPN ACTIVE AND CONNECTED", EventLogEntryType.Information, 500);
                    }
                    else if (thisConnProps.Name == OSERV.appConfig[3] && thisConnProps.Status == 7)
                    {
                        comObject.INetSharingConfigurationForINetConnection(oItem).DisableSharing();
                        ConnectionInfo = VPN_Status.DISCONNECTED;
                        Dbg.LogEvent("[ICS] VPN INACTIVE AND DISCONNECTED", EventLogEntryType.Information, 500);
                    }
                    else if (thisConnProps.Name == OSERV.appConfig[3])
                    {
                        Dbg.LogEvent("[ICS] UNKNOWN STATUS ON VPN: " + thisConnProps.Status, EventLogEntryType.Warning, 500);
                    }
                }
                if (ConnectionInfo == VPN_Status.DISCONNECTED)
                {
                    conns = comObject.EnumEveryConnection;
                    foreach (dynamic oItem in conns)
                    {
                        dynamic thisConnProps = comObject.NetConnectionProps(oItem);
                        if (thisConnProps.Name == OSERV.appConfig[4] && thisConnProps.Status == 2)
                        {
                            comObject.INetSharingConfigurationForINetConnection(oItem).EnableSharing(0);
                        }
                        else if (thisConnProps.Name == OSERV.appConfig[4] && thisConnProps.Status == 7)
                        {
                            comObject.INetSharingConfigurationForINetConnection(oItem).DisableSharing();
                        }
                    }
                }
                conns = comObject.EnumEveryConnection;
                foreach (dynamic oItem in conns)
                {
                    if (comObject.NetConnectionProps(oItem).Name == OSERV.appConfig[5])
                    {
                        comObject.INetSharingConfigurationForINetConnection(oItem).EnableSharing(1);
                        Dbg.LogEvent("[ICS] ICS CONFIGURED (VPN config:" + ConnectionInfo.ToString() + ")", EventLogEntryType.Information, 500);
                    }
                }
            }
            catch (Exception ex)
            {
                Dbg.LogEvent("[ICS] Unknown error occured: " + ex.Message, System.Diagnostics.EventLogEntryType.Error, 500);
            }
        }
    }
}
