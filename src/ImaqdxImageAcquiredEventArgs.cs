//////////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2019, National Instruments Corp.

// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:

// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using NationalInstruments.Vision.Acquisition.Imaqdx.Internal;

namespace NationalInstruments.Vision.Acquisition.Imaqdx
{
    //==============================================================================================
    /// <summary>
    /// Provides data for events that occur after an image is acquired.
    /// </summary>
    /// <threadsafety safety="safe"/>
    //==============================================================================================
    public class ImaqdxImageAcquiredEventArgs : EventArgs
    {
        private uint _bufferNumber;

        //==========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxImageAcquiredEventArgs" crefType="Unqualified"/> class.
        /// </summary>
        /// <param name="bufferNumber">
        /// The cumulative buffer number of the image to retrieve.
        /// </param>
        //==========================================================================================
        [CLSCompliant(false)]
        public ImaqdxImageAcquiredEventArgs(uint bufferNumber)
        {
            _bufferNumber = bufferNumber;
        }

        //==========================================================================================
        /// <summary>
        /// Gets the cumulative buffer number of the image to retrieve.
        /// </summary>
        /// <value>
        /// The cumulative buffer number of the image to retrieve.
        /// </value>
        //==========================================================================================
        [CLSCompliant(false)]
        public uint BufferNumber
        {
            get { return _bufferNumber; }
        }
    }
}
