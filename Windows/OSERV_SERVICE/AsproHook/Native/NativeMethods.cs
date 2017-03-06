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
using System.Diagnostics;
using System.Text;

using Asprosys.Win32.Hooks;

namespace Asprosys.Win32.Native
{
    internal static class NativeMethods
    {

        [DllImport("Kernel32.dll")]
        public static extern int GetCurrentThreadId();

        [DllImport("User32.dll", SetLastError = true)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName,
            int nMaxCount);

        public static string GetClassName(IntPtr hwnd)
        {
            StringBuilder sb = new StringBuilder(1024);
            int numChars = 0;
            numChars = GetClassName(hwnd, sb, 1024);
            if (numChars == 0)
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
            return sb.ToString(0, numChars);
        }

        [DllImport("User32.dll")]
        public static extern int CallNextHookEx(SafeHookHandle hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr SetWindowsHookExW(HookType idHook,
            SystemHookCallback lpfn, IntPtr hMod, int dwThreadId);

        [DllImport("AsproHook32.dll", SetLastError = true, EntryPoint = "SetGlobalHook")]
        static extern IntPtr SetGlobalHook32(HookType idHook, GlobalHookCallback callback,
            int threadID);

        [DllImport("AsproHook64.dll", SetLastError = true, EntryPoint = "SetGlobalHook")]
        static extern IntPtr SetGlobalHook64(HookType idHook, GlobalHookCallback callback,
            int threadID);

        public static SafeHookHandle SetLocalWindowsHook(HookType hookType,
            SystemHookCallback systemCallback, int threadID)
        {
            IntPtr hHook = SetWindowsHookExW(hookType, systemCallback, IntPtr.Zero, threadID);
            if (hHook == IntPtr.Zero)
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
            return new SafeHookHandle(hHook, UnhookWindowsHookEx);
        }

        public static SafeHookHandle SetGlobalWindowsHook(HookType hookType, 
            GlobalHookCallback globalCallback, int threadID, HookFilter.HookFilterInternal filter)
        {
            IntPtr hHook;
            RemoveHookDelegate remover;
            if (IntPtr.Size == 4)
            {
                hHook = SetGlobalHook32(hookType, globalCallback, threadID);
                remover = RemoveGlobalHook32;
            }
            else
            {
                hHook = SetGlobalHook64(hookType, globalCallback, threadID);
                remover = RemoveGlobalHook64;
            }

            if (hHook == IntPtr.Zero)
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }

            return new SafeHookHandle(hHook, remover);
        }

        [DllImport("User32.dll")]
        static extern void UnhookWindowsHookEx(SafeHookHandle hHook);

        [DllImport("AsproHook32.dll", EntryPoint = "RemoveGlobalHook")]
        static extern void RemoveGlobalHook32(SafeHookHandle hHook);

        [DllImport("AsproHook64.dll", EntryPoint = "RemoveGlobalHook")]
        static extern void RemoveGlobalHook64(SafeHookHandle hHook);

        [DllImport("User32.dll")]
        public static extern int GetKeyboardState(IntPtr lpKeyState);

        [DllImport("User32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        [DllImport("User32.dll")]
        public static extern short GetKeyState(int vKey);

        [DllImport("User32.dll")]
        public static extern IntPtr WindowFromPoint(System.Drawing.Point point);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg,
            IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int RegisterWindowMessage(string lpString);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int RegisterShellHookWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int DeregisterShellHookWindow(IntPtr hWnd);

 
    }
}
