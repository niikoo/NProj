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

namespace Asprosys.Win32.Hooks.Mouse
{
    /// <summary>
    /// 
    /// </summary>
    public enum HitTestCode
    {
        /// <summary>
        /// 
        /// </summary>
        Error = (-2),
        /// <summary>
        /// 
        /// </summary>
        Transparent = (-1),
        /// <summary>
        /// 
        /// </summary>
        Nowhere = 0,
        /// <summary>
        /// 
        /// </summary>
        Client = 1,
        /// <summary>
        /// 
        /// </summary>
        Caption = 2,
        /// <summary>
        /// 
        /// </summary>
        SysMenu = 3,
        /// <summary>
        /// 
        /// </summary>
        GrowBox = 4,
        /// <summary>
        /// 
        /// </summary>
        Size = GrowBox,
        /// <summary>
        /// 
        /// </summary>
        Menu = 5,
        /// <summary>
        /// 
        /// </summary>
        HScroll = 6,
        /// <summary>
        /// 
        /// </summary>
        VScroll = 7,
        /// <summary>
        /// 
        /// </summary>
        MinButton = 8,
        /// <summary>
        /// 
        /// </summary>
        MaxButton = 9,
        /// <summary>
        /// 
        /// </summary>
        Left = 10,
        /// <summary>
        /// 
        /// </summary>
        Right = 11,
        /// <summary>
        /// 
        /// </summary>
        Top = 12,
        /// <summary>
        /// 
        /// </summary>
        TopLeft = 13,
        /// <summary>
        /// 
        /// </summary>
        TopRight = 14,
        /// <summary>
        /// 
        /// </summary>
        Bottom = 15,
        /// <summary>
        /// 
        /// </summary>
        BottomLeft = 16,
        /// <summary>
        /// 
        /// </summary>
        BottomRight = 17,
        /// <summary>
        /// 
        /// </summary>
        Border = 18,
        /// <summary>
        /// 
        /// </summary>
        Reduce = MinButton,
        /// <summary>
        /// 
        /// </summary>
        Zoom = MaxButton,
        /// <summary>
        /// 
        /// </summary>
        SizeFirst = Left,
        /// <summary>
        /// 
        /// </summary>
        SizeLast = BottomRight,
        /// <summary>
        /// 
        /// </summary>
        Object = 19,
        /// <summary>
        /// 
        /// </summary>
        Close = 20,
        /// <summary>
        /// 
        /// </summary>
        Help = 21
    }
}
