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

namespace Asprosys.Win32.Hooks
{
    /// <summary>
    /// Base class for the arguments returned in all hook events
    /// </summary>
    public class HookEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the ID for the thread that the hook event occurred on.
        /// </summary>
        /// <value>The native thread ID.</value>
        public int ThreadID { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the data provided in the event 
        /// arguments has been altered.
        /// </summary>
        /// <value><c>true</c> if the data has been altered; otherwise, 
        /// <c>false</c>.</value>
        public bool DataAltered { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether further processing of this 
        /// message should be cancelled.
        /// </summary>
        /// <value><c>true</c> if processing should be cancelled; otherwise, 
        /// <c>false</c>.</value>
        public bool Cancel { get; set; }

        internal HookEventArgs(int threadID)
        {
            ThreadID = threadID;
        }
    }
}