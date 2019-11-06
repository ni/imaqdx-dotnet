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
using System.Runtime.InteropServices;
using System.Text;

namespace NationalInstruments.Vision.Acquisition.Imaqdx.Internal
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
    internal struct ImaqdxCameraInformation
    {
        public int Type;
        public int Version;
        public ImaqdxInterfaceFileFlags Flags;
        public int SerialNumberHi;
        public int SerialNumberLo;
        public ImaqdxBusType BusType;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string InterfaceName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string VendorName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string ModelName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string CameraFileName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string CameraAttributeUrl;

        public static Imaqdx.ImaqdxCameraInformation[] CreateCameraInformationArray(ImaqdxCameraInformation[] cameraInfoArray)
        {
            Imaqdx.ImaqdxCameraInformation[] cameraInformationArray = new Imaqdx.ImaqdxCameraInformation[cameraInfoArray.Length];
            for (int i = 0; i < cameraInfoArray.Length; i++)
            {
                cameraInformationArray[i] = new Imaqdx.ImaqdxCameraInformation(cameraInfoArray[i]);
            }
            return cameraInformationArray;
        }
    }
}
