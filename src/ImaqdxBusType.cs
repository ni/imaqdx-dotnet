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

namespace NationalInstruments.Vision.Acquisition.Imaqdx
{
    //==============================================================================================
    /// <summary>
    /// Specifies the bus type of the camera.
    /// </summary>
    //==============================================================================================
    public enum ImaqdxBusType : int
    {
        //==========================================================================================
        /// <summary>
        /// The camera is a FireWire camera.
        /// </summary>
        //==========================================================================================
        FireWire = 0x31333934,
        //==========================================================================================
        /// <summary>
        /// The camera is an Ethernet camera.
        /// </summary>
        //==========================================================================================
        Ethernet = 0x69707634,
        //==========================================================================================
        /// <summary>
        /// The camera is a simulated camera.
        /// </summary>
        //==========================================================================================
        Simulated = 0x2073696D,
        //==========================================================================================
        /// <summary>
        /// The camera is a DirectShow camera.
        /// </summary>
        //==========================================================================================
        DirectShow = 0x64736877
    }
}
