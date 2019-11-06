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

namespace NationalInstruments.Vision.Acquisition.Imaqdx.Internal
{
    internal class ImaqdxGrabAsyncState
    {
        private VisionImage _image;
        private bool _waitForNextBuffer;
        private object _userState;

        internal ImaqdxGrabAsyncState(VisionImage image, bool waitForNextBuffer, object userState)
        {
            _image = image;
            _waitForNextBuffer = waitForNextBuffer;
            _userState = userState;
        }

        public VisionImage Image
        {
            get { return _image; }
        }

        public bool WaitForNextBuffer
        {
            get { return _waitForNextBuffer; }
        }

        public object UserState
        {
            get { return _userState; }
        }
    }
}
