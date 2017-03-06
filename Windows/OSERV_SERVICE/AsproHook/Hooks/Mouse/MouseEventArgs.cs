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
using System.Runtime.InteropServices;
using System.Drawing;

using Asprosys.Win32.Native;

namespace Asprosys.Win32.Hooks.Mouse
{
    /// <summary>
    /// 
    /// </summary>
    public class MouseEventArgs : HookEventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is peek.
        /// </summary>
        /// <value><c>true</c> if this instance is peek; otherwise, <c>false</c>.</value>
        public bool IsPeek { get; private set; }
        /// <summary>
        /// Gets or sets the event.
        /// </summary>
        /// <value>The event.</value>
        public MouseEvent Event { get; private set; }
        /// <summary>
        /// Gets or sets the window handle.
        /// </summary>
        /// <value>The window handle.</value>
        public IntPtr WindowHandle { get; private set; }
        /// <summary>
        /// Gets or sets the X.
        /// </summary>
        /// <value>The X.</value>
        public int X { get; private set; }
        /// <summary>
        /// Gets or sets the Y.
        /// </summary>
        /// <value>The Y.</value>
        public int Y { get; private set; }
        /// <summary>
        /// Gets or sets the hit test code.
        /// </summary>
        /// <value>The hit test code.</value>
        public HitTestCode HitTestCode { get; private set; }
        /// <summary>
        /// Gets or sets the extra info.
        /// </summary>
        /// <value>The extra info.</value>
        public IntPtr ExtraInfo { get; private set; }

        internal MouseEventArgs(int threadID, int code, IntPtr wParam, IntPtr lParam)
            :base(threadID)
        {
            IsPeek = (code != NativeConstants.PM_REMOVE);
            Event = (MouseEvent)wParam.ToInt32();
            MOUSEHOOKSTRUCT mouseStruct = (MOUSEHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MOUSEHOOKSTRUCT));
            WindowHandle = mouseStruct.hwnd;
            X = mouseStruct.pt.X;
            Y = mouseStruct.pt.Y;
            HitTestCode = (HitTestCode)mouseStruct.wHitTestCode;
            ExtraInfo = mouseStruct.dwExtraInfo;
        }

//Disable never assigned warning
#pragma warning disable 649
        private struct MOUSEHOOKSTRUCT
        {
            public Point pt;
            public IntPtr hwnd;
            public int wHitTestCode;
            public IntPtr dwExtraInfo;
        }
    }
}
