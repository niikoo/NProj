﻿//*******************************************************************************
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

using Asprosys.Win32.Native;

namespace Asprosys.Win32.Hooks.CBT
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CBTHook : WindowsHook
    {

        /// <summary>
        /// Occurs when [window activating].
        /// </summary>
        public event EventHandler<WindowActivatingEventArgs> WindowActivating;
        /// <summary>
        /// Occurs when [window created].
        /// </summary>
        public event EventHandler<WindowCreatedEventArgs> WindowCreated;
        /// <summary>
        /// Occurs when [window destroyed].
        /// </summary>
        public event EventHandler<CBTEventArgs> WindowDestroyed;
        /// <summary>
        /// Occurs when [window state changing].
        /// </summary>
        public event EventHandler<WindowStateChangingEventArgs> WindowStateChanging;
        /// <summary>
        /// Occurs when [window rect changing].
        /// </summary>
        public event EventHandler<WindowRectChangingEventArgs> WindowRectChanging;
        /// <summary>
        /// Occurs when [focus changing].
        /// </summary>
        public event EventHandler<FocusChangingEventArgs> FocusChanging;
        /// <summary>
        /// Occurs when [system command].
        /// </summary>
        public event EventHandler<SystemCommandEventArgs> SystemCommand;

        /// <summary>
        /// Creates the hook.
        /// </summary>
        /// <returns></returns>
        public static CBTHook CreateHook()
        {
            return CreateHook(new HookFilter());
        }

        /// <summary>
        /// Creates the hook.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static CBTHook CreateHook(HookFilter filter)
        {
            return new CBTHook(NativeMethods.GetCurrentThreadId(), true, filter);
        }

        /// <summary>
        /// Creates the hook.
        /// </summary>
        /// <param name="threadID">The thread ID.</param>
        /// <returns></returns>
        public static CBTHook CreateHook(int threadID)
        {
            return CreateHook(threadID, new HookFilter());
        }

        /// <summary>
        /// Creates the hook.
        /// </summary>
        /// <param name="threadID">The thread ID.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static CBTHook CreateHook(int threadID, HookFilter filter)
        {
            return new CBTHook(threadID, WindowsHook.IsLocalThread(threadID), filter);
        }

        private CBTHook(int threadID, bool isLocal, HookFilter filter)
            : base(HookType.CBT, threadID, isLocal, filter)
        {

        }

        protected override int OnLocalCallback(int code, IntPtr wParam, IntPtr lParam)
        {
            int returnValue = 0;

            bool passedFilter = !Filter.IsInclusionList;
            foreach (int filterItem in Filter.Items)
            {
                if (filterItem == code)
                {
                    passedFilter = !passedFilter;
                    break;
                }
            }

            if (passedFilter)
            {
                int dataChanged = 0;
                returnValue = OnGlobalCallback(code, wParam, lParam, NativeMethods.GetCurrentThreadId(), ref dataChanged);
            }

            return returnValue;
        }

        protected override int OnGlobalCallback(int code, IntPtr wParam, IntPtr lParam, int threadID, ref int dataChanged)
        {
            int retVal = 0;
            CBTEventArgs args = new CBTEventArgs(0, IntPtr.Zero);
            switch ((CBTCode)code)
            {
                case CBTCode.Activate:
                    EventHandler<WindowActivatingEventArgs> tempWA = WindowActivating;
                    if (tempWA != null)
                    {
                        args = new WindowActivatingEventArgs(threadID, wParam, lParam);
                        tempWA(this, (WindowActivatingEventArgs)args);
                    }
                    break;
                case CBTCode.CreateWnd:
                    EventHandler<WindowCreatedEventArgs> tempCW = WindowCreated;
                    if (tempCW != null)
                    {
                        args = new WindowCreatedEventArgs(threadID, wParam, lParam);
                        tempCW(this, (WindowCreatedEventArgs)args);
                    }
                    break;
                case CBTCode.DestroyWnd:
                    EventHandler<CBTEventArgs> tempDW = WindowDestroyed;
                    if (tempDW != null)
                    {
                        args = new CBTEventArgs(threadID, wParam);
                        tempDW(this, args);
                    }
                    break;
                case CBTCode.MinMax:
                    EventHandler<WindowStateChangingEventArgs> tempMM = WindowStateChanging;
                    if (tempMM != null)
                    {
                        args = new WindowStateChangingEventArgs(threadID, wParam, lParam);
                        tempMM(this, (WindowStateChangingEventArgs)args);
                    }
                    break;
                case CBTCode.MoveSize:
                    EventHandler<WindowRectChangingEventArgs> tempMS = WindowRectChanging;
                    if (tempMS != null)
                    {
                        args = new WindowRectChangingEventArgs(threadID, wParam, lParam);
                        tempMS(this, (WindowRectChangingEventArgs)args);
                    }
                    break;
                case CBTCode.SetFocus:
                    EventHandler<FocusChangingEventArgs> tempSF = FocusChanging;
                    if (tempSF != null)
                    {
                        args = new FocusChangingEventArgs(threadID, wParam, lParam);
                        tempSF(this, (FocusChangingEventArgs)args);
                    }
                    break;
                case CBTCode.SysCommand:
                    EventHandler<SystemCommandEventArgs> tempSC = SystemCommand;
                    if (tempSC != null)
                    {
                        args = new SystemCommandEventArgs(threadID, wParam, lParam);
                        tempSC(this, (SystemCommandEventArgs)args);
                    }
                    break;
                default:
                    break;
            }
            if (args.Cancel) retVal = 1;
            dataChanged = (args.DataAltered ? 1 : 0);
            return retVal;
        }
    }
}
