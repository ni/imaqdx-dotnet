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
using System.Globalization;
using System.Text;
using NationalInstruments.Vision.Acquisition.Imaqdx.Internal;

namespace NationalInstruments.Vision.Acquisition.Imaqdx
{
    //==============================================================================================
    /// <summary>
    /// Provides details for cameras that are are currently connected or that were previously connected on the host computer.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <threadsafety safety="safe"/>
    //==============================================================================================
    public sealed class ImaqdxCameraInformation
    {
        private long _serialNumber;
        private ImaqdxBusType _type;
        private string _name;
        private string _vendor;
        private string _model;
        private string _cameraFile;
        private string _cameraAttributeUrl;
        private bool _isConnected;

        internal ImaqdxCameraInformation(Internal.ImaqdxCameraInformation cameraInfo)
        {
            long serialNumberLow = cameraInfo.SerialNumberLo;
            long serialNumberHigh = cameraInfo.SerialNumberHi;
            serialNumberHigh = serialNumberHigh << 32;
            _serialNumber = serialNumberLow | serialNumberHigh;
            _type = cameraInfo.BusType;
            _name = cameraInfo.InterfaceName;
            _vendor = cameraInfo.VendorName;
            _model = cameraInfo.ModelName;
            _cameraFile = cameraInfo.CameraFileName;
            _cameraAttributeUrl = cameraInfo.CameraAttributeUrl;
            _isConnected = (cameraInfo.Flags & ImaqdxInterfaceFileFlags.Connected) != 0;
        }

        //==========================================================================================
        /// <summary>
        /// Gets the camera serial number.
        /// </summary>
        /// <value>
        /// The camera serial number.
        /// </value>
        //==========================================================================================
        public long SerialNumber
        {
            get { return _serialNumber; }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the bus type for the camera.
        /// </summary>
        /// <value>
        /// The bus type for the camera.
        /// </value>
        //==========================================================================================
        public ImaqdxBusType Type
        {
            get { return _type; }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the name of the camera.
        /// </summary>
        /// <value>
        /// The name of the camera.
        /// </value>
        //==========================================================================================
        public string Name
        {
            get { return _name; }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the camera vendor name.
        /// </summary>
        /// <value>
        /// The camera vendor name.
        /// </value>
        //==========================================================================================
        public string Vendor
        {
            get { return _vendor; }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the cameral model name.
        /// </summary>
        /// <value>
        /// The cameral model name.
        /// </value>
        //==========================================================================================
        public string Model
        {
            get { return _model; }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the name of the camera file. The camera file contains all the settings for a given camera. You can configure and save these settings from Measurement <entity value="amp"/> Automation Explorer (MAX).
        /// </summary>
        /// <value>
        /// The name of the camera file.
        /// </value>
        //==========================================================================================
        public string CameraFile
        {
            get { return _cameraFile; }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the URL of the XML file that describes the camera attributes.
        /// </summary>
        /// <value>
        /// The URL of the XML file that describes the camera attributes.
        /// </value>
        //==========================================================================================
        public string CameraAttributeUrl
        {
            get { return _cameraAttributeUrl; }
        }

        //==========================================================================================
        /// <summary>
        /// Gets whether the camera is connected to the host computer.
        /// </summary>
        /// <value>
        /// 	<see langword="true"/> if the camera is connected to the host computer; otherwise, <see langword="false"/>.
        /// </value>
        //==========================================================================================
        public bool IsConnected
        {
            get { return _isConnected; }
        }

        //==========================================================================================
        /// <summary>
        /// Converts the value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of the value of this instance.
        /// </returns>
        //==========================================================================================
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}: Name={1}, Type={2}", GetType().Name, _name, _type);
        }
    }
}
