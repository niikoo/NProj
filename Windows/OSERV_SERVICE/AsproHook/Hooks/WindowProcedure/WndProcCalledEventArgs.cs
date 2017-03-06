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
using System.Drawing;
using System.Runtime.InteropServices;

using Asprosys.Win32.Native;

namespace Asprosys.Win32.Hooks.WindowProcedure
{
    /// <summary>
    /// 
    /// </summary>
    public class WndProcCalledEventArgs : HookEventArgs
    {
        /// <summary>
        /// Gets or sets the target window.
        /// </summary>
        /// <value>The target window.</value>
        public IntPtr TargetWindow { get; private set; }
        /// <summary>
        /// Gets or sets the message ID.
        /// </summary>
        /// <value>The message ID.</value>
        public int MessageID { get; private set; }
        /// <summary>
        /// Gets or sets the w param.
        /// </summary>
        /// <value>The w param.</value>
        public IntPtr wParam { get; private set; }
        /// <summary>
        /// Gets or sets the l param.
        /// </summary>
        /// <value>The l param.</value>
        public IntPtr lParam { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether [from current thread].
        /// </summary>
        /// <value><c>true</c> if [from current thread]; otherwise, <c>false</c>.</value>
        public bool FromCurrentThread { get; private set; }

        internal WndProcCalledEventArgs(int threadID, IntPtr currentThread, IntPtr msg)
            : base(threadID)
        {
            CWPSTRUCT newMSG = (CWPSTRUCT)Marshal.PtrToStructure(msg, typeof(CWPSTRUCT));
            TargetWindow = newMSG.hwnd;
            MessageID = newMSG.message;
            wParam = newMSG.wParam;
            lParam = newMSG.lParam;
            FromCurrentThread = (currentThread != IntPtr.Zero);
        }
        
//Disable never assigned warning
#pragma warning disable 649
        internal struct CWPSTRUCT
        {
            public IntPtr lParam;
            public IntPtr wParam;
            public int message;
            public IntPtr hwnd;
        }
    }
}
