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

namespace Asprosys.Win32.Hooks.CBT
{
    /// <summary>
    /// 
    /// </summary>
    public class WindowStateChangingEventArgs : CBTEventArgs
    {
        /// <summary>
        /// Gets or sets the new state.
        /// </summary>
        /// <value>The new state.</value>
        public FormWindowState NewState { get; private set; }

        internal WindowStateChangingEventArgs(int threadID, IntPtr hwnd, IntPtr windowState)
            : base(threadID, hwnd)
        {
            ShowWindow newWindowState = (ShowWindow)(windowState.ToInt64() & 0x00000FFFF);
            switch (newWindowState)
            {
                case ShowWindow.SW_FORCEMINIMIZE:
                case ShowWindow.SW_MINIMIZE:
                case ShowWindow.SW_SHOWMINIMIZED:
                case ShowWindow.SW_SHOWMINNOACTIVE:
                    NewState = FormWindowState.Minimized;
                    break;
                case ShowWindow.SW_MAXIMIZE:
                    NewState = FormWindowState.Maximized;
                    break;
                default:
                    NewState = FormWindowState.Normal;
                    break;
            }
        }

        private enum ShowWindow
        {
            SW_HIDE,
            SW_NORMAL,
            SW_SHOWMINIMIZED,
            SW_MAXIMIZE,
            SW_SHOWNOACTIVATE,
            SW_SHOW,
            SW_MINIMIZE,
            SW_SHOWMINNOACTIVE,
            SW_SHOWNA,
            SW_RESTORE,
            SW_SHOWDEFAULT,
            SW_FORCEMINIMIZE
        }
    }
}
