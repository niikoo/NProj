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
using System.Windows.Forms;

namespace Asprosys.Win32.Hooks.Shell
{
    /// <summary>
    /// 
    /// </summary>
    public class ShellHookMsg : ShellHook
    {

        /// <summary>
        /// Creates the hook.
        /// </summary>
        /// <returns></returns>
        public static new ShellHookMsg CreateHook()
        {
            return CreateHook(new HookFilter());
        }

        /// <summary>
        /// Creates the hook.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static new ShellHookMsg CreateHook(HookFilter filter)
        {
            return new ShellHookMsg(filter);
        }

        private ShellHookWindow m_Window;
        private bool m_MessageLoopCreated;

        private ShellHookMsg(HookFilter filter)
            : base(0, true, filter)
        {
        }

        /// <summary>
        /// Starts the hook.
        /// </summary>
        public override void SetHook()
        {
            m_Window = new ShellHookWindow(OnLocalCallback);
            IntPtr hwnd = m_Window.Handle; //Force window creation

            if (!Application.MessageLoop)
            {
                m_MessageLoopCreated = true;
                Application.Run();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (m_Window.InvokeRequired)
            {
                Action<bool> invoker = new Action<bool>(Dispose);
                m_Window.Invoke(invoker, new object[] { true });
            }
            else
            {
                m_Window.Close();
                if (m_MessageLoopCreated)
                {
                    Application.Exit();
                }
            }
                
        }

    }
}
