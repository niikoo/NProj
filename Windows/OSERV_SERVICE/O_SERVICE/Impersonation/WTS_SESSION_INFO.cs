using System;
using System.Runtime.InteropServices;

namespace OSERV_BASE.Impersonation
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct WTS_SESSION_INFO
    {
        public int SessionId;
        public string pWinStationName;
        public int State;
    }
}
