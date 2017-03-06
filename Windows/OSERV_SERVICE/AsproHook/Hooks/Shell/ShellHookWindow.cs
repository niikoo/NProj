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
using System.ComponentModel;
using System.Runtime.InteropServices;

using Asprosys.Win32.Native;

namespace Asprosys.Win32.Hooks.Shell
{
    internal partial class ShellHookWindow : Form
    {

        public delegate int MessageCallback(int code, IntPtr wParam, IntPtr lParam);
        
        private MessageCallback m_Callback;
        private int m_ShellMessageID;

        private const string SHELL_HOOK_MESSAGE = "SHELLHOOK";

        public ShellHookWindow()
        {
            InitializeComponent();
        }

        public ShellHookWindow(MessageCallback callback)
            : this()
        {
            m_Callback = callback;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            m_ShellMessageID = NativeMethods.RegisterWindowMessage(SHELL_HOOK_MESSAGE);
            if (m_ShellMessageID == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            if (NativeMethods.RegisterShellHookWindow(this.Handle) == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            base.OnHandleCreated(e);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            NativeMethods.DeregisterShellHookWindow(this.Handle);
            base.OnClosing(e);
        }
        
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                default:
                    if (m.Msg == m_ShellMessageID)
                    {
                        int shellMessage = m.WParam.ToInt32();
                        switch ((ShellHookCode)shellMessage)
                        {
                            case ShellHookCode.AppCommand:
                                m.Result = (IntPtr)m_Callback(shellMessage, IntPtr.Zero,
                                    m.LParam);
                                break;
                            case ShellHookCode.GetMinRect:
                                m.Result = (IntPtr)m_Callback(shellMessage, Marshal.ReadIntPtr(m.LParam), 
                                    new IntPtr(m.LParam.ToInt64() + IntPtr.Size));
                                break;
                            case ShellHookCode.Flash:
                                m.Result = (IntPtr)m_Callback(shellMessage, (IntPtr)ShellHookCode.Redraw,
                                    (IntPtr)1);
                                break;
                            default:
                                m.Result = (IntPtr)m_Callback(shellMessage, m.LParam, IntPtr.Zero);
                                break;
                        }
                    }
                    else
                    {
                        base.WndProc(ref m);
                    }
                    break;
            }
        }
    }
}
