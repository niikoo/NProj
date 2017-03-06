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

using Asprosys.Win32.Native;

namespace Asprosys.Win32.Hooks.Shell
{
    public class ShellHook : WindowsHook
    {

        /// <summary>
        /// Occurs when [accessibility state changed].
        /// </summary>
        public event EventHandler<AccessibilityStateChangedEventArgs> AccessibilityStateChanged;
        /// <summary>
        /// Occurs when [app command].
        /// </summary>
        public event EventHandler<AppCommandEventArgs> AppCommand;
        /// <summary>
        /// Occurs when [window activated].
        /// </summary>
        public event EventHandler<ShellEventArgs> WindowActivated;
        /// <summary>
        /// Occurs when [window created].
        /// </summary>
        public event EventHandler<ShellEventArgs> WindowCreated;
        /// <summary>
        /// Occurs when [window redrawn].
        /// </summary>
        public event EventHandler<ShellEventArgs> WindowRedrawn;
        /// <summary>
        /// Occurs when [window replaced].
        /// </summary>
        public event EventHandler<ShellEventArgs> WindowReplaced;
        /// <summary>
        /// Occurs when [window destroyed].
        /// </summary>
        public event EventHandler<ShellEventArgs> WindowDestroyed;
        /// <summary>
        /// Occurs when [get window min rect].
        /// </summary>
        public event EventHandler<ShellEventArgs> GetWindowMinRect;
        /// <summary>
        /// Occurs when [input language changed].
        /// </summary>
        public event EventHandler<ShellEventArgs> InputLanguageChanged;
        /// <summary>
        /// Occurs when [task man invoked].
        /// </summary>
        public event EventHandler<ShellEventArgs> TaskManInvoked;
        /// <summary>
        /// Occurs when [shell window activate].
        /// </summary>
        public event EventHandler<ShellEventArgs> ShellWindowActivate;
        /// <summary>
        /// Occurs when [end task invoked].
        /// </summary>
        public event EventHandler<ShellEventArgs> EndTaskInvoked;
        /// <summary>
        /// Occurs when [window replacing].
        /// </summary>
        public event EventHandler<ShellEventArgs> WindowReplacing;
        /// <summary>
        /// Occurs when [window rude activated].
        /// </summary>
        public event EventHandler<ShellEventArgs> WindowRudeActivated;

        /// <summary>
        /// Creates the hook.
        /// </summary>
        /// <returns></returns>
        public static ShellHook CreateHook()
        {
            return CreateHook(new HookFilter());
        }

        /// <summary>
        /// Creates the hook.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static ShellHook CreateHook(HookFilter filter)
        {
            return new ShellHook(NativeMethods.GetCurrentThreadId(), true, filter);
        }

        /// <summary>
        /// Creates the hook.
        /// </summary>
        /// <param name="threadID">The thread ID.</param>
        /// <returns></returns>
        public static ShellHook CreateHook(int threadID)
        {
            return CreateHook(threadID, new HookFilter());
        }

        /// <summary>
        /// Creates the hook.
        /// </summary>
        /// <param name="threadID">The thread ID.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static ShellHook CreateHook(int threadID, HookFilter filter)
        {
            return new ShellHook(threadID, WindowsHook.IsLocalThread(threadID), filter);
        }

        internal ShellHook(int threadID, bool isLocal, HookFilter filter)
            : base(HookType.Shell, threadID, isLocal, filter)
        {
        }

        protected override int OnLocalCallback(int code, IntPtr wParam, IntPtr lParam)
        {
            int returnValue = 0;

            bool passedFilter = !Filter.IsInclusionList;

            if (Filter.ItemCount > 0)
            {
                foreach (int filterItem in Filter.Items)
                {
                    if (filterItem == code)
                    {
                        passedFilter = !passedFilter;
                        break;
                    }
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
            int returnValue = 0;
            ShellHookCode eventCode = (ShellHookCode)code;
            switch (eventCode)
            {
                case ShellHookCode.AccessibilityState:
                    EventHandler<AccessibilityStateChangedEventArgs> ascTemp = AccessibilityStateChanged;
                    if (ascTemp != null)
                    {
                        var args = new AccessibilityStateChangedEventArgs(threadID, wParam);
                        ascTemp(this, args);
                    }
                    break;
                case ShellHookCode.ActivateShellWindow:
                    EventHandler<ShellEventArgs> swaTemp = ShellWindowActivate;
                    if (swaTemp != null)
                    {
                        var args = new ShellEventArgs(threadID, IntPtr.Zero, eventCode, IntPtr.Zero);
                        swaTemp(this, args);
                    }
                    break;
                case ShellHookCode.AppCommand:
                    EventHandler<AppCommandEventArgs> acTemp = AppCommand;
                    if (acTemp != null)
                    {
                        var args = new AppCommandEventArgs(threadID, wParam, lParam);
                        acTemp(this, args);
                        if (args.Cancel) returnValue = 1; 
                   }
                    break;
                case ShellHookCode.GetMinRect:
                    EventHandler<ShellEventArgs> gwmrTemp = GetWindowMinRect;
                    if (gwmrTemp != null)
                    {
                        var args = new ShellEventArgs(threadID, wParam, eventCode, lParam);
                        gwmrTemp(this, args);
                    }
                    break;
                case ShellHookCode.Language:
                    EventHandler<ShellEventArgs> ilcTemp = InputLanguageChanged;
                    if (ilcTemp != null)
                    {
                        var args = new ShellEventArgs(threadID, wParam, eventCode, lParam);
                        ilcTemp(this, args);
                    }
                    break;
                case ShellHookCode.Redraw:
                    EventHandler<ShellEventArgs> wrTemp = WindowRedrawn;
                    if (wrTemp != null)
                    {
                        var args = new ShellEventArgs(threadID, wParam, eventCode, lParam);
                        wrTemp(this, args);
                    }
                    break;
                case ShellHookCode.TaskMan:
                    EventHandler<ShellEventArgs> tmiTemp = TaskManInvoked;
                    if (tmiTemp != null)
                    {
                        var args = new ShellEventArgs(threadID,IntPtr.Zero, eventCode, IntPtr.Zero);
                        tmiTemp(this, args);
                        if (args.Cancel) returnValue = 1;
                    }
                    break;
                case ShellHookCode.WindowActivated:
                    EventHandler<ShellEventArgs> waTemp = WindowActivated;
                    if (waTemp != null)
                    {
                        var args = new ShellEventArgs(threadID, wParam, eventCode, lParam);
                        waTemp(this, args);
                    }
                    break;
                case ShellHookCode.WindowCreated:
                    EventHandler<ShellEventArgs> wcTemp = WindowCreated;
                    if (wcTemp != null)
                    {
                        var args = new ShellEventArgs(threadID, wParam, eventCode, lParam);
                        wcTemp(this, args);
                    }
                    break;
                case ShellHookCode.WindowDestroyed:
                    EventHandler<ShellEventArgs> wdTemp = WindowDestroyed;
                    if (wdTemp != null)
                    {
                        var args = new ShellEventArgs(threadID, wParam, eventCode, lParam);
                        wdTemp(this, args);
                    }
                    break;
                case ShellHookCode.WindowReplaced:
                    EventHandler<ShellEventArgs> wreTemp = WindowReplaced;
                    if (wreTemp != null)
                    {
                        var args = new ShellEventArgs(threadID, wParam, eventCode, lParam);
                        wreTemp(this, args);
                    }
                    break;
                case ShellHookCode.WindowReplacing:
                    EventHandler<ShellEventArgs> wrgTemp = WindowReplacing;
                    if (wrgTemp != null)
                    {
                        var args = new ShellEventArgs(threadID, wParam, eventCode, lParam);
                        wrgTemp(this, args);
                    }
                    break;
                case ShellHookCode.RudeActivated:
                    EventHandler<ShellEventArgs> raTemp = WindowRudeActivated;
                    if (raTemp != null)
                    {
                        var args = new ShellEventArgs(threadID, wParam, eventCode, lParam);
                        raTemp(this, args);
                    }
                    break;
                case ShellHookCode.EndTask:
                    EventHandler<ShellEventArgs> etTemp = EndTaskInvoked;
                    if (etTemp != null)
                    {
                        var args = new ShellEventArgs(threadID, wParam, eventCode, lParam);
                        etTemp(this, args);
                    }
                    break;
                default:
                    break;
            }
            return returnValue;
        }
    }
}
