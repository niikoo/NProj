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

namespace Asprosys.Win32.Hooks.CBT
{
    /// <summary>
    /// 
    /// </summary>
    public enum SystemCommand
    {
        /// <summary>
        /// 
        /// </summary>
        Size = 0xF000,
        /// <summary>
        /// 
        /// </summary>
        Move = 0xF010,
        /// <summary>
        /// 
        /// </summary>
        Minimize = 0xF020,
        /// <summary>
        /// 
        /// </summary>
        Maximize = 0xF030,
        /// <summary>
        /// 
        /// </summary>
        NextWindow = 0xF040,
        /// <summary>
        /// 
        /// </summary>
        PrevWindow = 0xF050,
        /// <summary>
        /// 
        /// </summary>
        Close = 0xF060,
        /// <summary>
        /// 
        /// </summary>
        VScroll = 0xF070,
        /// <summary>
        /// 
        /// </summary>
        HScroll = 0xF080,
        /// <summary>
        /// 
        /// </summary>
        MouseMenu = 0xF090,
        /// <summary>
        /// 
        /// </summary>
        KeyMenu = 0xF100,
        /// <summary>
        /// 
        /// </summary>
        Arrange = 0xF110,
        /// <summary>
        /// 
        /// </summary>
        Restore = 0xF120,
        /// <summary>
        /// 
        /// </summary>
        TaskList = 0xF130,
        /// <summary>
        /// 
        /// </summary>
        ScreenSave = 0xF140,
        /// <summary>
        /// 
        /// </summary>
        HotKey = 0xF150,
        /// <summary>
        /// 
        /// </summary>
        Default = 0xF160,
        /// <summary>
        /// 
        /// </summary>
        MonitorPower = 0xF170,
        /// <summary>
        /// 
        /// </summary>
        ContextHelp = 0xF180
    }
}
