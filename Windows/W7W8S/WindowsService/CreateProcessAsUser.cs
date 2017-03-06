using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Security.Principal;
using System.ComponentModel;
using System.IO;

namespace WindowsService
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct WTS_SESSION_INFO
    {
        public int SessionId;
        public string pWinStationName;
        public int State;
    }
    [SuppressUnmanagedCodeSecurity]
    class SessionFinder
    {
        //Get the currently active local console session.
        public IntPtr GetLocalInteractiveSession()
        {
            IntPtr hToken = IntPtr.Zero;

            int sessionID = WTSGetActiveConsoleSessionId();

            if (sessionID != -1) //-1 no console session.
            {
                if (!WTSQueryUserToken(sessionID, out hToken))
                {
                    throw new System.ComponentModel.Win32Exception();
                }
            }
            return hToken;
        }

        //Get the session of the provided user. The same user could have 
        //more than one session, this just retrieves the first one found. 
        //More sophisticated checks could easily be added by requesting 
        //different types of information from WTSQuerySessionInformation.
        public IntPtr GetSessionByUser(string domain, string userName)
        {
            IntPtr hToken = IntPtr.Zero;
            int sessionID = -1;

            IntPtr pSessionsBuffer;
            int sessionsCount;

            if (!WTSEnumerateSessions(WTS_CURRENT_SERVER_HANDLE, 0, 1,
                out pSessionsBuffer, out sessionsCount))
            {
                throw new System.ComponentModel.Win32Exception();
            }

            try
            {
                for (int i = 0; i < sessionsCount; ++i)
                {
                    IntPtr pCurrentSession = new IntPtr(pSessionsBuffer.ToInt64() +
                        Marshal.SizeOf(typeof(WTS_SESSION_INFO)) * i);
                    WTS_SESSION_INFO sessionInfo = (WTS_SESSION_INFO)Marshal.PtrToStructure(pCurrentSession,
                        typeof(WTS_SESSION_INFO));

                    if (string.Compare(GetUserName(sessionInfo.SessionId), userName, true) == 0 &&
                        string.Compare(GetDomain(sessionInfo.SessionId), domain, true) == 0)
                    {
                        sessionID = sessionInfo.SessionId;
                        break;
                    }
                }
            }
            finally
            {
                WTSFreeMemory(pSessionsBuffer);
            }

            if (sessionID != -1)
            {
                if (!WTSQueryUserToken(sessionID, out hToken))
                {
                    throw new System.ComponentModel.Win32Exception();
                }
            }

            return hToken;
        }

        private string GetUserName(int sessionID)
        {
            IntPtr pNameBuffer;
            int nameLength;
            string name;

            if (!WTSQuerySessionInformation(WTS_CURRENT_SERVER_HANDLE, sessionID,
                    WTSUserName, out pNameBuffer, out nameLength))
            {
                throw new System.ComponentModel.Win32Exception();
            }

            try
            {
                name = Marshal.PtrToStringUni(pNameBuffer);
            }
            finally
            {
                WTSFreeMemory(pNameBuffer);
            }

            return name;
        }

        private string GetDomain(int sessionID)
        {
            IntPtr pDomainBuffer;
            int domainLength;
            string domain;

            if (!WTSQuerySessionInformation(WTS_CURRENT_SERVER_HANDLE, sessionID,
                    WTSDomainName, out pDomainBuffer, out domainLength))
            {
                throw new System.ComponentModel.Win32Exception();
            }

            try
            {
                domain = Marshal.PtrToStringUni(pDomainBuffer);
            }
            finally
            {
                WTSFreeMemory(pDomainBuffer);
            }

            return domain;
        }

        [DllImport("WtsApi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WTSQueryUserToken(int SessionId,
            out IntPtr phToken);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int WTSGetActiveConsoleSessionId();

        [DllImport("WtsApi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WTSQuerySessionInformation(IntPtr hServer,
            int SessionId, int WTSInfoClass, out IntPtr ppBuffer, out int pBytesReturned);

        [DllImport("WtsApi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WTSEnumerateSessions(IntPtr hServer, int Reserved,
            int Version, out IntPtr ppSessionInfo, out int pCount);

        [DllImport("WtsApi32.dll", SetLastError = true)]
        private static extern void WTSFreeMemory(IntPtr pMemory);

        private static readonly IntPtr WTS_CURRENT_SERVER_HANDLE = (IntPtr)0;
        private const int WTSUserName = 5;
        private const int WTSDomainName = 7;
    }

    [SuppressUnmanagedCodeSecurity]
    class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct ProfileInfo
        {
            ///
            /// Specifies the size of the structure, in bytes.
            ///
            public int dwSize;

            ///
            /// This member can be one of the following flags: PI_NOUI or PI_APPLYPOLICY
            ///
            public int dwFlags;

            ///
            /// Pointer to the name of the user.
            /// This member is used as the base name of the directory in which to store a new profile.
            ///
            public string lpUserName;

            ///
            /// Pointer to the roaming user profile path.
            /// If the user does not have a roaming profile, this member can be NULL.
            ///
            public string lpProfilePath;

            ///
            /// Pointer to the default user profile path. This member can be NULL.
            ///
            public string lpDefaultPath;

            ///
            /// Pointer to the name of the validating domain controller, in NetBIOS format.
            /// If this member is NULL, the Windows NT 4.0-style policy will not be applied.
            ///
            public string lpServerName;

            ///
            /// Pointer to the path of the Windows NT 4.0-style policy file. This member can be NULL.
            ///
            public string lpPolicyPath;

            ///
            /// Handle to the HKEY_CURRENT_USER registry key.
            ///
            public IntPtr hProfile;
        }

        [DllImport("userenv.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool LoadUserProfile(IntPtr hToken, ref ProfileInfo lpProfileInfo);

        [StructLayout(LayoutKind.Sequential)]
        public struct STARTUPINFO
        {
            public Int32 cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public Int32 dwProcessID;
            public Int32 dwThreadID;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public Int32 Length;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        public enum SECURITY_IMPERSONATION_LEVEL
        {
            SecurityAnonymous,
            SecurityIdentification,
            SecurityImpersonation,
            SecurityDelegation
        }

        public enum TOKEN_TYPE
        {
            TokenPrimary = 1,
            TokenImpersonation
        }

        public const int GENERIC_ALL_ACCESS = 0x10000000;
        public const int CREATE_NO_WINDOW = 0x08000000;

        [
           DllImport("kernel32.dll",
              EntryPoint = "CloseHandle", SetLastError = true,
              CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)
        ]
        public static extern bool CloseHandle(IntPtr handle);

        [
           DllImport("advapi32.dll",
              EntryPoint = "CreateProcessAsUser", SetLastError = true,
              CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)
        ]
        public static extern bool
           CreateProcessAsUser(IntPtr hToken, string lpApplicationName, string lpCommandLine,
                               ref SECURITY_ATTRIBUTES lpProcessAttributes, ref SECURITY_ATTRIBUTES lpThreadAttributes,
                               bool bInheritHandle, Int32 dwCreationFlags, IntPtr lpEnvrionment,
                               string lpCurrentDirectory, ref STARTUPINFO lpStartupInfo,
                               ref PROCESS_INFORMATION lpProcessInformation);

        [
           DllImport("advapi32.dll",
              EntryPoint = "DuplicateTokenEx")
        ]
        public static extern bool
           DuplicateTokenEx(IntPtr hExistingToken, Int32 dwDesiredAccess,
                            ref SECURITY_ATTRIBUTES lpThreadAttributes,
                            Int32 ImpersonationLevel, Int32 dwTokenType,
                            ref IntPtr phNewToken);

        /// <summary>
        /// Creates a process as another user
        /// </summary>
        /// <param name="filename">The path to the process to start</param>
        /// <param name="args">Process arguments</param>
        /// <param name="hToken">The user token, use IntPtr.Zero to get current user token (doesn't work on services)</param>
        /// <param name="loadUserProfile">Load the user's profile</param>
        /// <returns></returns>
        public static Process CreateProcessAsUser(string filename, string args, IntPtr hToken, bool loadUserProfile = false)
        {
            if (hToken == IntPtr.Zero)
            {
                hToken = WindowsIdentity.GetCurrent().Token;
            }
            var hDupedToken = IntPtr.Zero;

            var pi = new PROCESS_INFORMATION();
            var sa = new SECURITY_ATTRIBUTES();
            sa.Length = Marshal.SizeOf(sa);

            try
            {
                if (!DuplicateTokenEx(
                        hToken,
                        GENERIC_ALL_ACCESS,
                        ref sa,
                        (int)SECURITY_IMPERSONATION_LEVEL.SecurityIdentification,
                        (int)TOKEN_TYPE.TokenPrimary,
                        ref hDupedToken
                    ))
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                var si = new STARTUPINFO();
                si.cb = Marshal.SizeOf(si);
                si.lpDesktop = "";

                var path = Path.GetFullPath(filename);
                var dir = Path.GetDirectoryName(path);

                // Revert to self to create the entire process; not doing this might
                // require that the currently impersonated user has "Replace a process
                // level token" rights - we only want our service account to need
                // that right.
                using (var ctx = WindowsIdentity.Impersonate(IntPtr.Zero))
                {
                    if (loadUserProfile)
                    {
                        // Load user profile
	                    ProfileInfo profileInfo = new ProfileInfo();
	                    profileInfo.dwSize = Marshal.SizeOf(profileInfo);
                        profileInfo.lpUserName = WindowsIdentity.GetCurrent(TokenAccessLevels.MaximumAllowed).Name;
	                    profileInfo.dwFlags = 1;
	                    Boolean loadSuccess = LoadUserProfile(hDupedToken, ref profileInfo);
	 
	                    if (!loadSuccess)
	                    {
                            Console.WriteLine("LoadUserProfile() failed with error code: " +
	                                            Marshal.GetLastWin32Error());
	                        throw new Win32Exception(Marshal.GetLastWin32Error());
	                    }
	 
	                    if (profileInfo.hProfile == IntPtr.Zero)
	                    {
	                        Console.WriteLine(
	                            "LoadUserProfile() failed - HKCU handle was not loaded. Error code: " +
	                            Marshal.GetLastWin32Error());
	                        throw new Win32Exception(Marshal.GetLastWin32Error());
	                    }
                    }
                    if (!CreateProcessAsUser(
                                            hDupedToken,
                                            path,
                                            string.Format("\"{0}\" {1}", filename.Replace("\"", "\"\""), args),
                                            ref sa, ref sa,
                                            false, 0, IntPtr.Zero,
                                            dir, ref si, ref pi
                                    ))
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                return Process.GetProcessById(pi.dwProcessID);
            }
            finally
            {
                if (pi.hProcess != IntPtr.Zero)
                    CloseHandle(pi.hProcess);
                if (pi.hThread != IntPtr.Zero)
                    CloseHandle(pi.hThread);
                if (hDupedToken != IntPtr.Zero)
                    CloseHandle(hDupedToken);
            }
        }
    }
}