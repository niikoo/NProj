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

namespace Asprosys.Win32.Hooks.Shell
{
    /// <summary>
    /// 
    /// </summary>
    public enum AppCommand
    {
        /// <summary>
        /// 
        /// </summary>
        BrowserBackward = 1,
        /// <summary>
        /// 
        /// </summary>
        BrowserForward = 2,
        /// <summary>
        /// 
        /// </summary>
        BrowserRefresh = 3,
        /// <summary>
        /// 
        /// </summary>
        BrowserStop = 4,
        /// <summary>
        /// 
        /// </summary>
        BrowserSearch = 5,
        /// <summary>
        /// 
        /// </summary>
        BrowserFavorites = 6,
        /// <summary>
        /// 
        /// </summary>
        BrowserHome = 7,
        /// <summary>
        /// 
        /// </summary>
        VolumeMute = 8,
        /// <summary>
        /// 
        /// </summary>
        VolumeDown = 9,
        /// <summary>
        /// 
        /// </summary>
        VolumeUp = 10,
        /// <summary>
        /// 
        /// </summary>
        MediaNextTrack = 11,
        /// <summary>
        /// 
        /// </summary>
        MediaPreviousTrack = 12,
        /// <summary>
        /// 
        /// </summary>
        MediaStop = 13,
        /// <summary>
        /// 
        /// </summary>
        MediaPlayPause = 14,
        /// <summary>
        /// 
        /// </summary>
        LaunchMail = 15,
        /// <summary>
        /// 
        /// </summary>
        LaunchMediaSelect = 16,
        /// <summary>
        /// 
        /// </summary>
        LaunchApp1 = 17,
        /// <summary>
        /// 
        /// </summary>
        LaunchApp2 = 18,
        /// <summary>
        /// 
        /// </summary>
        BassDown = 19,
        /// <summary>
        /// 
        /// </summary>
        BassBoost = 20,
        /// <summary>
        /// 
        /// </summary>
        BassUp = 21,
        /// <summary>
        /// 
        /// </summary>
        TrebleDown = 22,
        /// <summary>
        /// 
        /// </summary>
        TrebleUp = 23,
        /// <summary>
        /// 
        /// </summary>
        MicrophoneVolumeMute = 24,
        /// <summary>
        /// 
        /// </summary>
        MicrophoneVolumeDown = 25,
        /// <summary>
        /// 
        /// </summary>
        MicrophoneVolumeUp = 26,
        /// <summary>
        /// 
        /// </summary>
        Help = 27,
        /// <summary>
        /// 
        /// </summary>
        Find = 28,
        /// <summary>
        /// 
        /// </summary>
        New = 29,
        /// <summary>
        /// 
        /// </summary>
        Open = 30,
        /// <summary>
        /// 
        /// </summary>
        Close = 31,
        /// <summary>
        /// 
        /// </summary>
        Save = 32,
        /// <summary>
        /// 
        /// </summary>
        Print = 33,
        /// <summary>
        /// 
        /// </summary>
        Undo = 34,
        /// <summary>
        /// 
        /// </summary>
        Redo = 35,
        /// <summary>
        /// 
        /// </summary>
        Copy = 36,
        /// <summary>
        /// 
        /// </summary>
        Cut = 37,
        /// <summary>
        /// 
        /// </summary>
        Paste = 38,
        /// <summary>
        /// 
        /// </summary>
        ReplyToMail = 39,
        /// <summary>
        /// 
        /// </summary>
        ForwardMail = 40,
        /// <summary>
        /// 
        /// </summary>
        SendMail = 41,
        /// <summary>
        /// 
        /// </summary>
        SpellCheck = 42,
        /// <summary>
        /// 
        /// </summary>
        DictateOrCommandControlToggle = 43,
        /// <summary>
        /// 
        /// </summary>
        MicOnOffToggle = 44,
        /// <summary>
        /// 
        /// </summary>
        CorrectionList = 45,
        /// <summary>
        /// 
        /// </summary>
        MediaPlay = 46,
        /// <summary>
        /// 
        /// </summary>
        MediaPause = 47,
        /// <summary>
        /// 
        /// </summary>
        MediaRecord = 48,
        /// <summary>
        /// 
        /// </summary>
        MediaFastForward = 49,
        /// <summary>
        /// 
        /// </summary>
        MediaRewind = 50,
        /// <summary>
        /// 
        /// </summary>
        MediaChannelUp = 51,
        /// <summary>
        /// 
        /// </summary>
        MediaChannelDown = 52,
        /// <summary>
        /// 
        /// </summary>
        Delete = 53,
        /// <summary>
        /// 
        /// </summary>
        DWMFlip3D = 54
    }
}
