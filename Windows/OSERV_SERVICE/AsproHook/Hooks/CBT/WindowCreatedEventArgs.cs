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

namespace Asprosys.Win32.Hooks.CBT
{
    /// <summary>
    /// 
    /// </summary>
    public class WindowCreatedEventArgs : CBTEventArgs
    {

        private IntPtr m_pStruct;
        private CBT_CREATEWND m_CreateWndStruct;
        private CREATESTRUCT m_CreateStruct;

        private string m_WindowName;
        private string m_ClassName;

        internal WindowCreatedEventArgs(int threadID, IntPtr hwnd, IntPtr createWndStruct)
            : base(threadID, hwnd)
        {
            m_pStruct = createWndStruct;
        }

        private void MarshalStructToPtr()
        {
            Marshal.StructureToPtr(m_CreateStruct, m_CreateWndStruct.CreateStruct, false);
            Marshal.StructureToPtr(m_CreateWndStruct, m_pStruct, false);
        }

        private void MarshalStructFromPtr()
        {
            m_CreateWndStruct = (CBT_CREATEWND)Marshal.PtrToStructure(m_pStruct, typeof(CBT_CREATEWND));
            m_CreateStruct = (CREATESTRUCT)Marshal.PtrToStructure(m_CreateWndStruct.CreateStruct, typeof(CREATESTRUCT));
        }

        /// <summary>
        /// Gets the creation parameters.
        /// </summary>
        /// <value>The creation parameters.</value>
        public IntPtr CreationParameters
        {
            get
            {
                if (m_CreateWndStruct == null) MarshalStructFromPtr();
                return m_CreateStruct.lpCreateParams;
            }
        }

        /// <summary>
        /// Gets the instance handle.
        /// </summary>
        /// <value>The instance handle.</value>
        public IntPtr InstanceHandle
        {
            get
            {
                if (m_CreateWndStruct == null) MarshalStructFromPtr();
                return m_CreateStruct.hInstance;
            }
        }
        /// <summary>
        /// Gets the menu handle.
        /// </summary>
        /// <value>The menu handle.</value>
        public IntPtr MenuHandle
        {
            get
            {
                if (m_CreateWndStruct == null) MarshalStructFromPtr();
                return m_CreateStruct.hMenu;
            }
        }
        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public IntPtr Parent
        {
            get
            {
                if (m_CreateWndStruct == null) MarshalStructFromPtr();
                return m_CreateStruct.hwndParent;
            }
        }
        /// <summary>
        /// Gets the window style.
        /// </summary>
        /// <value>The window style.</value>
        public int WindowStyle
        {
            get
            {
                if (m_CreateWndStruct == null) MarshalStructFromPtr();
                return m_CreateStruct.style;
            }
        }

        /// <summary>
        /// Gets the extended window style.
        /// </summary>
        /// <value>The extended window style.</value>
        public int ExtendedWindowStyle
        {
            get
            {
                if (m_CreateWndStruct == null) MarshalStructFromPtr();
                return m_CreateStruct.dwExStyle;
            }
        }

        /// <summary>
        /// Gets the name of the window.
        /// </summary>
        /// <value>The name of the window.</value>
        public string WindowName
        {
            get
            {
                if (m_CreateWndStruct == null) MarshalStructFromPtr();
                if (m_WindowName == null) 
                    m_WindowName = Marshal.PtrToStringUni(m_CreateStruct.lpszName);
                return m_WindowName;
            }
        }

        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        /// <value>The name of the class.</value>
        public string ClassName
        {
            get
            {
                if (m_CreateWndStruct == null) MarshalStructFromPtr();
                if (m_ClassName == null)
                    m_ClassName = NativeMethods.GetClassName(base.WindowHandle);
                return m_ClassName;
            }
        }

        /// <summary>
        /// Gets or sets the window rect.
        /// </summary>
        /// <value>The window rect.</value>
        public Rectangle WindowRect
        {
            get
            {
                if (m_CreateWndStruct == null) MarshalStructFromPtr();
                return new Rectangle(m_CreateStruct.x, m_CreateStruct.y,
                    m_CreateStruct.cx, m_CreateStruct.cy);
            }
            set
            {
                if (m_CreateWndStruct == null) MarshalStructFromPtr();
                m_CreateStruct.x = value.X;
                m_CreateStruct.y = value.Y;
                m_CreateStruct.cx = value.Width;
                m_CreateStruct.cy = value.Height;
                MarshalStructToPtr();
            }
        }

        /// <summary>
        /// Gets or sets the insert after HWND.
        /// </summary>
        /// <value>The insert after HWND.</value>
        public IntPtr InsertAfterHwnd
        {
            get
            {
                if (m_CreateWndStruct == null) MarshalStructFromPtr();
                return m_CreateWndStruct.InsertAfterHwnd;
            }
            set
            {
                if (m_CreateWndStruct == null) MarshalStructFromPtr();
                m_CreateWndStruct.InsertAfterHwnd = value;
                MarshalStructToPtr();
            }
        }

//Disable never assigned warning
#pragma warning disable 649

        private struct CREATESTRUCT
        {
            public IntPtr lpCreateParams;
            public IntPtr hInstance;
            public IntPtr hMenu;
            public IntPtr hwndParent;
            public int cy;
            public int cx;
            public int y;
            public int x;
            public int style;
            public IntPtr lpszName;
            public IntPtr lpszClass;
            public int dwExStyle;
        }

        [StructLayout(LayoutKind.Sequential)]
        private class CBT_CREATEWND
        {
            public IntPtr CreateStruct;
            public IntPtr InsertAfterHwnd;
        }
    }
}
