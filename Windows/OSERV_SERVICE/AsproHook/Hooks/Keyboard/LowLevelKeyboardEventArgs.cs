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
using System.Runtime.InteropServices;

using Asprosys.Win32.Native;

namespace Asprosys.Win32.Hooks.Keyboard
{
    /// <summary>
    /// 
    /// </summary>
    public class LowLevelKeyboardEventArgs : HookEventArgs
    {

        private const int KEY_TOGGLE_ON = 1;
        private const int KEYSTATE_KEYDOWN_MASK = 0x00008000;

        /// <summary>
        /// Gets or sets the key code.
        /// </summary>
        /// <value>The key code.</value>
        public Keys KeyCode { get; private set; }
        /// <summary>
        /// Gets or sets the scan code.
        /// </summary>
        /// <value>The scan code.</value>
        public int ScanCode { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is extended key.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is extended key; otherwise, <c>false</c>.
        /// </value>
        public bool IsExtendedKey { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is alt down.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is alt down; otherwise, <c>false</c>.
        /// </value>
        public bool IsAltDown { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is key up.
        /// </summary>
        /// <value><c>true</c> if this instance is key up; otherwise, <c>false</c>.</value>
        public bool IsKeyUp { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is injected.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is injected; otherwise, <c>false</c>.
        /// </value>
        public bool IsInjected { get; private set; }
        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>The timestamp.</value>
        public int Timestamp { get; private set; }

        internal LowLevelKeyboardEventArgs(IntPtr lParam)
            : base(0)
        {
            KBDLLHOOKSTRUCT keyStruct = MarshalPtrToStructure(lParam);
            KeyCode = (Keys)keyStruct.vkCode;
            ScanCode = keyStruct.scanCode;
            Timestamp = keyStruct.time;
            int flags = keyStruct.flags;
            IsInjected = ((flags & 0x00000010) != 0);
            IsExtendedKey = ((flags & 0x00000001) != 0);
            IsAltDown = ((flags & 0x00000020) != 0);
            IsKeyUp = ((flags & 0x00000080) != 0);
        }

        private KBDLLHOOKSTRUCT MarshalPtrToStructure(IntPtr pStruct)
        {
            return (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(pStruct, typeof(KBDLLHOOKSTRUCT));
        }

        private int m_CtrlDown = -1;
        /// <summary>
        /// Gets a value indicating whether this instance is CTRL down.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is CTRL down; otherwise, <c>false</c>.
        /// </value>
        public bool IsCtrlDown
        {
            get
            {
                if (m_CtrlDown == -1)
                {
                    m_CtrlDown = (NativeMethods.GetAsyncKeyState((int)Keys.ControlKey) & KEYSTATE_KEYDOWN_MASK);
                }
                return (m_CtrlDown == KEYSTATE_KEYDOWN_MASK);
            }
        }

        private int m_ShiftDown = -1;
        /// <summary>
        /// Gets a value indicating whether this instance is shift down.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is shift down; otherwise, <c>false</c>.
        /// </value>
        public bool IsShiftDown
        {
            get
            {
                if (m_ShiftDown == -1)
                {
                    m_ShiftDown = (NativeMethods.GetAsyncKeyState((int)Keys.ShiftKey) & KEYSTATE_KEYDOWN_MASK);
                }
                return (m_ShiftDown == KEYSTATE_KEYDOWN_MASK);
            }
        }

        private int m_LWinDown = -1;
        /// <summary>
        /// Gets a value indicating whether this instance is L win down.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is L win down; otherwise, <c>false</c>.
        /// </value>
        public bool IsLWinDown
        {
            get
            {
                if (m_LWinDown == -1)
                {
                    m_LWinDown = (NativeMethods.GetAsyncKeyState((int)Keys.LWin) & KEYSTATE_KEYDOWN_MASK);
                }
                return (m_LWinDown == KEYSTATE_KEYDOWN_MASK);
            }
        }

        private int m_RWinDown = -1;
        /// <summary>
        /// Gets a value indicating whether this instance is R win down.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is R win down; otherwise, <c>false</c>.
        /// </value>
        public bool IsRWinDown
        {
            get
            {
                if (m_RWinDown == -1)
                {
                    m_RWinDown = (NativeMethods.GetAsyncKeyState((int)Keys.RWin) & KEYSTATE_KEYDOWN_MASK);
                }
                return (m_RWinDown == KEYSTATE_KEYDOWN_MASK);
            }
        }

        private int m_CapsLockOn = -1;
        /// <summary>
        /// Gets a value indicating whether this instance is caps lock on.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is caps lock on; otherwise, <c>false</c>.
        /// </value>
        public bool IsCapsLockOn
        {
            get
            {
                if (m_CapsLockOn == -1)
                {
                    m_CapsLockOn = (NativeMethods.GetKeyState((int)Keys.Capital) & KEY_TOGGLE_ON);
                }
                return (m_CapsLockOn == KEYSTATE_KEYDOWN_MASK);
            }
        }

        private int m_NumLockOn = -1;
        /// <summary>
        /// Gets a value indicating whether this instance is num lock on.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is num lock on; otherwise, <c>false</c>.
        /// </value>
        public bool IsNumLockOn
        {
            get
            {
                if (m_NumLockOn == -1)
                {
                    m_NumLockOn = (NativeMethods.GetKeyState((int)Keys.NumLock) & KEY_TOGGLE_ON);
                }
                return (m_NumLockOn == KEYSTATE_KEYDOWN_MASK);
            }
        }


//Disable never assigned warning
#pragma warning disable 649
        [StructLayout(LayoutKind.Sequential)]
        private class KBDLLHOOKSTRUCT
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public IntPtr dwExtraInfo;
        }

    }
}
