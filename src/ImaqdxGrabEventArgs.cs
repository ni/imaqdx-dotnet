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
using System.ComponentModel;

namespace NationalInstruments.Vision.Acquisition.Imaqdx
{
    //==============================================================================================
    /// <summary>
    /// Provides data after a grab has completed.
    /// </summary>
    /// <threadsafety safety="safe"/>
    //==============================================================================================
    public class ImaqdxGrabEventArgs : AsyncCompletedEventArgs
    {
        private uint _actualBufferNumber;
        private VisionImage _image;

        //==========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxGrabEventArgs" crefType="Unqualified"/> class.
        /// </summary>
        /// <param name="error">
        /// Any error that occurred during the asynchronous operation.
        /// </param>
        /// <param name="cancelled">
        /// A value indicating whether the asynchronous operation was cancelled.
        /// </param>
        /// <param name="userState">
        /// An object used to associate client state, such as a task ID, with this particular asynchronous operation.
        /// </param>
        /// <param name="actualBufferNumber">
        /// The actual cumulative buffer number of the image retrieved.
        /// </param>
        /// <param name="image">
        /// The image that receives the captured pixel data.
        /// </param>
        //==========================================================================================
        [CLSCompliant(false)]
        public ImaqdxGrabEventArgs(Exception error, bool cancelled, object userState, uint actualBufferNumber, VisionImage image)
            : base(error, cancelled, userState)
        {
            _actualBufferNumber = actualBufferNumber;
            _image = image;
        }

        //==========================================================================================
        /// <summary>
        /// Gets the actual cumulative buffer number of the image retrieved.
        /// </summary>
        /// <value>
        /// The actual cumulative buffer number of the image retrieved.
        /// </value>
        //==========================================================================================
        [CLSCompliant(false)]
        public uint ActualBufferNumber
        {
            get { return _actualBufferNumber; }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the image that receives the captured pixel data.
        /// </summary>
        /// <value>
        /// The image that receives the captured pixel data.
        /// </value>
        //==========================================================================================
        public VisionImage Image
        {
            get { return _image; }
        }
    }
}
