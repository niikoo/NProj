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
using System.Windows.Forms;

using Asprosys.Win32.Native;

namespace Asprosys.Win32.Hooks.Keyboard
{
    /// <summary>
    /// 
    /// </summary>
    public class KeyboardEventArgs : HookEventArgs
    {
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
        /// Gets or sets the repeat count.
        /// </summary>
        /// <value>The repeat count.</value>
        public int RepeatCount { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is peek.
        /// </summary>
        /// <value><c>true</c> if this instance is peek; otherwise, <c>false</c>.</value>
        public bool IsPeek { get; private set; }
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

        private bool m_ExtendedInfoRetrieved;

        internal KeyboardEventArgs(int threadID, int code, IntPtr wParam, IntPtr lParam)
            :base(threadID)
        {
            IsPeek = (code != NativeConstants.PM_REMOVE);
            KeyCode = (Keys)wParam.ToInt32();
            int flags = lParam.ToInt32();
            RepeatCount = flags & 0x0000FFFF;
            ScanCode = (flags & 0x00FF0000) >> 16;
            IsExtendedKey = ((flags & 0x01000000) != 0);
            IsAltDown = ((flags & 0x10000000) != 0);
            IsKeyUp = (flags < 0);
        }

        private bool m_CtrlDown;
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
                if (!m_ExtendedInfoRetrieved)
                {
                    SetExtendedKeyInfo();
                }
                return m_CtrlDown;
            }
        }

        private bool m_ShiftDown;
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
                if (!m_ExtendedInfoRetrieved)
                {
                    SetExtendedKeyInfo();
                }
                return m_ShiftDown;
            }
        }

        private bool m_LWinDown;
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
                if (!m_ExtendedInfoRetrieved)
                {
                    SetExtendedKeyInfo();
                }
                return m_LWinDown;
            }
        }

        private bool m_RWinDown;
        /// <summary>
        /// Gets a value indicating whether this instance is SR win down.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is SR win down; otherwise, <c>false</c>.
        /// </value>
        public bool IsSRWinDown
        {
            get
            {
                if (!m_ExtendedInfoRetrieved)
                {
                    SetExtendedKeyInfo();
                }
                return m_RWinDown;
            }
        }

        private bool m_CapsLockOn;
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
                if (!m_ExtendedInfoRetrieved)
                {
                    SetExtendedKeyInfo();
                }
                return m_CapsLockOn;
            }
        }

        private bool m_NumLockOn;
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
                if (!m_ExtendedInfoRetrieved)
                {
                    SetExtendedKeyInfo();
                    m_ExtendedInfoRetrieved = true;
                }
                return m_NumLockOn;
            }
        }

        private void SetExtendedKeyInfo()
        {
            const int KEY_TOGGLE_ON = 1;
            const int KBDSTATE_KEYDOWN_MASK = 0x00000080;

            unsafe
            {
                byte* buffer = stackalloc byte[256];
                NativeMethods.GetKeyboardState((IntPtr)buffer);
                m_ShiftDown = ((buffer[(int)Keys.ShiftKey] & KBDSTATE_KEYDOWN_MASK) == KBDSTATE_KEYDOWN_MASK);
                m_CtrlDown = ((buffer[(int)Keys.ControlKey] & KBDSTATE_KEYDOWN_MASK) == KBDSTATE_KEYDOWN_MASK);
                m_LWinDown = ((buffer[(int)Keys.LWin] & KBDSTATE_KEYDOWN_MASK) == KBDSTATE_KEYDOWN_MASK);
                m_RWinDown = ((buffer[(int)Keys.RWin] & KBDSTATE_KEYDOWN_MASK) == KBDSTATE_KEYDOWN_MASK);
                m_CapsLockOn = ((buffer[(int)Keys.Capital] & KEY_TOGGLE_ON) == KEY_TOGGLE_ON);
                m_NumLockOn = ((buffer[(int)Keys.NumLock] & KEY_TOGGLE_ON) == KEY_TOGGLE_ON);
            }

            m_ExtendedInfoRetrieved = true;

        }
    }
}
