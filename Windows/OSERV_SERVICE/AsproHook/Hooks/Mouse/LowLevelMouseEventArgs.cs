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
    public class LowLevelMouseEventArgs : HookEventArgs
    {

        /// <summary>
        /// Gets or sets the event.
        /// </summary>
        /// <value>The event.</value>
        public MouseEvent Event { get; private set; }
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
        /// Gets or sets a value indicating whether this instance is injected.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is injected; otherwise, <c>false</c>.
        /// </value>
        public bool IsInjected { get; private set; }
        /// <summary>
        /// Gets or sets the mouse data.
        /// </summary>
        /// <value>The mouse data.</value>
        public int MouseData { get; private set; }
        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>The timestamp.</value>
        public int Timestamp { get; private set; }
        /// <summary>
        /// Gets or sets the extra info.
        /// </summary>
        /// <value>The extra info.</value>
        public IntPtr ExtraInfo { get; private set; }

        internal LowLevelMouseEventArgs(IntPtr wParam, IntPtr lParam)
            : base(0)
        {
            Event = (MouseEvent)wParam.ToInt32();
            MSLLHOOKSTRUCT mouseStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
            MouseData = mouseStruct.mouseData;
            X = mouseStruct.pt.X;
            Y = mouseStruct.pt.Y;
            IsInjected = ((mouseStruct.flags & 1) == 1);
            Timestamp = mouseStruct.time;
            ExtraInfo = mouseStruct.dwExtraInfo;
        }

        IntPtr m_WindowHandle;
        public IntPtr GetWindowHandle()
        {
            if (m_WindowHandle == IntPtr.Zero)
                m_WindowHandle = NativeMethods.WindowFromPoint(new System.Drawing.Point(X, Y));
            return m_WindowHandle;
        }

        HitTestCode m_HTC = (HitTestCode)(-10);
        public HitTestCode GetHitTestCode()
        {
            const int WM_NCHITTEST = 0x0084;

            if (m_HTC == (HitTestCode)(-10))
                m_HTC = (HitTestCode)NativeMethods.SendMessage(GetWindowHandle(), WM_NCHITTEST, IntPtr.Zero, (IntPtr)(Y << 16 | X & 0x0000FFFF)).ToInt32();
            return m_HTC;
        }
        
//Disable never assigned warning
#pragma warning disable 649
        private struct MSLLHOOKSTRUCT
        {
            public Point pt;
            public int mouseData;
            public int flags;
            public int time;
            public IntPtr dwExtraInfo;
        }
    }
}
