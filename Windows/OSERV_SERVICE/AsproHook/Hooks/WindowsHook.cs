//*******************************************************************************
//*                                                                             *
//*                                                                             *
//*             Copyright (c) 2009, Asprosys Inc.                               *
//*             All rights reserved.                                            *
//*                                                                             *
//* Redistribution and use in source and binary forms, with or without          *
//* modification, are permitted provided that the following conditions are      *
//* met:                                                                        *
//*                                                                             *
//*   * Redistributions of source code must retain the above copyright          *
//*     notice, this list of conditions and the following disclaimer.           *
//*                                                                             *
//*   * Redistributions in binary form must reproduce the above copyright       *
//*     notice, this list of conditions and the following disclaimer in the     *
//*     documentation and/or other materials provided with the distribution.    *
//*     If no materials are distributed with the binary the above copyright     *
//*     notice must be included with the binary file's version resource info.   *
//*                                                                             *
//*   * Neither the name of Asprosys Inc. nor the names of its contributors     *
//*     may be used to endorse or promote products derived from this software   *
//*     without specific prior written permission.                              *
//*                                                                             *
//*                                                                             *
//* THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS         *
//* "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED   *
//* TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR  *
//* PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR            *
//* CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,       *
//* EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,         *
//* PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; *
//* OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,    *
//* WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR     *
//* OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF      *
//* ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.                                  *
//*                                                                             *
//*                                                                             *
//*******************************************************************************

using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

using Asprosys.Win32.Native;

namespace Asprosys.Win32.Hooks
{

    internal delegate int SystemHookCallback(int nCode, IntPtr wParam, IntPtr lParam);
    internal delegate int GlobalHookCallback(int nCode, IntPtr wParam, IntPtr lParam, int threadID, ref int dataChanged);
 
    /// <summary>
    /// The base class for all the Hooks.
    /// </summary>
    public abstract class WindowsHook : IDisposable
    {

        private SafeHookHandle m_Hook;
        private HookType m_Type;

        private int m_ThreadID;
        private bool m_IsLocal;
        internal HookFilter.HookFilterInternal Filter { get; private set; }
        private SystemHookCallback m_SysCall;
        private GlobalHookCallback m_GlobalCall;

        protected static bool IsLocalThread(int threadID)
        {
            if (threadID != 0)
            {
                using (Process proc = Process.GetCurrentProcess())
                {
                    foreach (ProcessThread thread in proc.Threads)
                    {
                        if (thread.Id == threadID) return true;
                    }
                }
            }
            return false;
        }

        internal WindowsHook(HookType type, int threadId, bool isLocal, HookFilter filter)
        {
            if (!isLocal) throw new NotSupportedException("Global hooks are not supported yet.");

            m_Type = type;
            m_ThreadID = threadId;
            m_IsLocal = isLocal;
            Filter = filter.GetTrueFilter();
        }

        /// <summary>
        /// Starts the hook.
        /// </summary>
        public virtual void SetHook()
        {
            if (m_Hook == null)
            {
                if (m_IsLocal)
                {
                    m_SysCall = SystemCallback;
                    m_Hook = NativeMethods.SetLocalWindowsHook(m_Type, m_SysCall, m_ThreadID);
                }
                else
                {
                    m_GlobalCall = GlobalCallback;
                    m_Hook = NativeMethods.SetGlobalWindowsHook(m_Type, m_GlobalCall, m_ThreadID, Filter);
                }
            }
        }


        /// <summary>
        /// Removes the hook (internally this just calls Dispose()).
        /// </summary>
        public void RemoveHook()
        {
            Dispose();
        }

        protected int SystemCallback(int code, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (code >= 0)
                {
                    int retCode = OnLocalCallback(code, wParam, lParam); ;
                    if (retCode != 0) return retCode;
                }
            }
            catch (Exception ex)
            {
                ThreadPool.QueueUserWorkItem(OnUnhandledError, ex);
            }
            return NativeMethods.CallNextHookEx(m_Hook, code, wParam, lParam);            
        }
        
        protected int GlobalCallback(int code, IntPtr wParam, IntPtr lParam, int threadID, ref int dataChanged)
        {
            int retVal = 0;
            try
            {
                retVal = OnGlobalCallback(code, wParam, lParam, threadID, ref dataChanged);
            }
            catch(Exception ex)
            {
                ThreadPool.QueueUserWorkItem(OnUnhandledError, ex);
            }
            return retVal;
        }

        private void OnUnhandledError(object exception)
        {
            Dispose();
            throw new InvalidOperationException("Unhandled exception in hook callback function.", (Exception)exception);
        }

        protected abstract int OnLocalCallback(int code, IntPtr wParam, IntPtr lParam);

        protected abstract int OnGlobalCallback(int code, IntPtr wParam, IntPtr lParam, int threadID, ref int dataChanged);

        #region IDisposable Members

        /// <summary>
        /// Removes the hook and frees all unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_Hook.Dispose();
                m_Hook = null;
            }
        }

        #endregion
    }
}
