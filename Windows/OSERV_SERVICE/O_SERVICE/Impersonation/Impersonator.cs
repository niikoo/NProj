namespace OSERV_BASE.Impersonation
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Security.Principal;
	using System.Runtime.InteropServices;
	using System.ComponentModel;
    using System.Diagnostics;
    using OSERV_BASE.Classes;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Impersonation of a user. Allows to execute code under another
	/// user context.
	/// Please note that the account that instantiates the Impersonator class
	/// needs to have the 'Act as part of operating system' privilege set.
	/// </summary>
	/// <remarks>	
	/// This class is based on the information in the Microsoft knowledge base
	/// article http://support.microsoft.com/default.aspx?scid=kb;en-us;Q306158
	/// 
	/// Encapsulate an instance into a using-directive like e.g.:
	/// 
	///		...
	///		using ( new Impersonator( "myUsername", "myDomainname", "myPassword" ) )
	///		{
	///			...
	///			[code that executes under the new context]
	///			...
	///		}
	///		...
	/// 
	/// Please contact the author Uwe Keim (mailto:uwe.keim@zeta-software.de)
	/// for questions regarding this class.
	/// </remarks>
	public class Impersonator :
		IDisposable
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor. Starts the impersonation with the given credentials.
		/// Please note that the account that instantiates the Impersonator class
		/// needs to have the 'Act as part of operating system' privilege set.
        /// If parameters == null, then inpersonate with the current user's session
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		public Impersonator(
			string userName = null,
			string domainName = null,
			string password = null )
		{
            if (userName == null)
            {
                ImpersonateCurrentUser();
            }
            else
            {
                //Dbg.LogEvent("IMP:2", EventLogEntryType.Information, false);
                ImpersonateValidUser(userName, domainName, password);
            }
		}

		// ------------------------------------------------------------------
		#endregion

		#region IDisposable member.
		// ------------------------------------------------------------------

		public void Dispose()
		{
			UndoImpersonation();
		}

		// ------------------------------------------------------------------
		#endregion

		#region P/Invoke.
		// ------------------------------------------------------------------

		[DllImport("advapi32.dll", SetLastError=true)]
		private static extern int LogonUser(
			string lpszUserName,
			string lpszDomain,
			string lpszPassword,
			int dwLogonType,
			int dwLogonProvider,
			ref IntPtr phToken);
		
		[DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern int DuplicateToken(
			IntPtr hToken,
			int impersonationLevel,
			ref IntPtr hNewToken);

		[DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		private static extern bool RevertToSelf();

		[DllImport("kernel32.dll", CharSet=CharSet.Auto)]
		private static extern  bool CloseHandle(
			IntPtr handle);

		private const int LOGON32_LOGON_INTERACTIVE = 2;
		private const int LOGON32_PROVIDER_DEFAULT = 0;

		// ------------------------------------------------------------------
		#endregion

		#region Private member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Does the actual impersonation.
		/// </summary>
		/// <param name="userName">The name of the user to act as.</param>
		/// <param name="domainName">The domain name of the user to act as.</param>
		/// <param name="password">The password of the user to act as.</param>
		private void ImpersonateValidUser(
			string userName, 
			string domain, 
			string password )
		{
            //Dbg.LogEvent("IMP:3", EventLogEntryType.Information, false);
			WindowsIdentity tempWindowsIdentity = null;
			IntPtr token = IntPtr.Zero;
			IntPtr tokenDuplicate = IntPtr.Zero;
            //Dbg.LogEvent("IMP:4", EventLogEntryType.Information, false);
            try
            {
                if (RevertToSelf())
                {
                    //Dbg.LogEvent("IMP:5", EventLogEntryType.Information, false);
                    if (LogonUser(
                        userName,
                        domain,
                        password,
                        LOGON32_LOGON_INTERACTIVE,
                        LOGON32_PROVIDER_DEFAULT,
                        ref token) != 0)
                    {
                        //Dbg.LogEvent("IMP:6", EventLogEntryType.Information, false);
                        if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                        {
                            //Dbg.LogEvent("IMP:7", EventLogEntryType.Information, false);
                            tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                            impersonationContext = tempWindowsIdentity.Impersonate();
                        }
                        else
                        {
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }
                    }
                    else
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                }
                else
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            catch (Win32Exception ex)
            {
                Dbg.LogEvent("W32EXEPTION: " + ex,EventLogEntryType.Error);
            }
			finally
			{
                //Dbg.LogEvent("IMP:8", EventLogEntryType.Information, false);
				if ( token!= IntPtr.Zero )
				{
					CloseHandle( token );
				}
				if ( tokenDuplicate!=IntPtr.Zero )
				{
					CloseHandle( tokenDuplicate );
				}
                //Dbg.LogEvent("IMP:9", EventLogEntryType.Information, false);
			}
		}

		/// <summary>
		/// Does the actual impersonation, as the current logged on user.
		/// </summary>
		private void ImpersonateCurrentUser()
		{
			WindowsIdentity tempWindowsIdentity = null;
			IntPtr token = IntPtr.Zero;
			IntPtr tokenDuplicate = IntPtr.Zero;
            try
            {
                SessionFinder sf = new SessionFinder();
                token = sf.GetLocalInteractiveSession();
            }
            catch {
                //Dbg.LogEvent("No user logon?",EventLogEntryType.Error,false);
                return;
            }
            try
            {
                if (RevertToSelf())
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        impersonationContext = tempWindowsIdentity.Impersonate();
                    }
                    else
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                }
                else
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            catch (Win32Exception ex)
            {
                Dbg.LogEvent("W32EXEPTION: " + ex,EventLogEntryType.Error);
            }
			finally
			{
				if ( token!= IntPtr.Zero )
				{
					CloseHandle( token );
				}
				if ( tokenDuplicate!=IntPtr.Zero )
				{
					CloseHandle( tokenDuplicate );
				}
			}
		}

		/// <summary>
		/// Reverts the impersonation.
		/// </summary>
		private void UndoImpersonation()
		{
            //Dbg.LogEvent("IMP:10", EventLogEntryType.Information, false);
			if ( impersonationContext!=null )
			{
				impersonationContext.Undo();
			}
            //Dbg.LogEvent("IMP:11", EventLogEntryType.Information, false);
		}

		private WindowsImpersonationContext impersonationContext = null;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}