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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Asprosys.Win32.Hooks
{
    /// <summary>
    /// 
    /// </summary>
    public class HookFilter
    {
        /// <summary>
        /// Gets or sets the ID of the process that will be monitored.
        /// A value of zero indicates all processes.
        /// </summary>
        /// <value>The ID of the process to monitor.</value>
        public int Process { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is an inclusion list.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is an inclusion list; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// <para>If the list is an exclusion list (the default) then all messages 
        /// will be passed on except those in the list. The list can be empty.</para>
        /// <para>If the list is an inclusion list then only messages in the list will
        /// be passed on. Attemting to set an inclusion list with an empty filter
        /// on a hook will cause an InvalidOperationException to be thrown.</para>
        /// </remarks>
        public bool IsInclusionList { get; set; }
        /// <summary>
        /// Gets the list of integer values to filter on.
        /// </summary>
        /// <value>The filter list.</value>
        /// <remarks>
        /// <para>The values that can be included in the list depend on the type of 
        /// hook. See the specific hook you will be using for details.</para>
        /// <para></para>
        /// </remarks>
        public List<int> FilterList { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HookFilter"/> class.
        /// </summary>
        public HookFilter()
        {
            FilterList = new List<int>();
        }

        internal HookFilterInternal GetTrueFilter()
        {
            HookFilterInternal hfi = new HookFilterInternal();
            hfi.ProcessID = Process;
            hfi.IsInclusionList = IsInclusionList;
            if (FilterList != null)
            {
                hfi.ItemCount = FilterList.Count;
                if (hfi.ItemCount > 29) hfi.ItemCount = 29;
                for (int i = 0; i < hfi.ItemCount; ++i)
                {
                    hfi.Items[i] = FilterList[i];
                }
            }
            if (IsInclusionList && hfi.ItemCount == 0)
                throw new InvalidOperationException("Inclusion list cannot be empty.");

            return hfi;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class HookFilterInternal
        {
            public int ProcessID;
            [MarshalAs(UnmanagedType.Bool)]public bool IsInclusionList;
            public int ItemCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 29)]
            public int[] Items = new int[29];
        }
    }
}
