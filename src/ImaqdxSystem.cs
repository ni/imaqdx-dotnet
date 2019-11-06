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
using NationalInstruments.Vision.Acquisition.Imaqdx.Internal;

namespace NationalInstruments.Vision.Acquisition.Imaqdx
{
    //==============================================================================================
    /// <summary>
    /// Provides methods for camera information and resetting cameras.
    /// </summary>
    /// <threadsafety safety="safe"/>
    //==============================================================================================
    public static class ImaqdxSystem
    {
        //==========================================================================================
        /// <summary>
        /// Performs a manual reset on a camera. Stops any ongoing acquisitions.
        /// </summary>
        /// <param name="cameraName">
        /// The name of the camera you want to reset. <paramref name="cameraName"/>
        /// 	<format type="monospace">(cam0, cam1, ..., camN) </format> must match the configuration file name you used to configure the camera in MAX. You can also open a camera using its 64-bit serial number <format type="monospace">(uuid:XXXXXXXXXXXXXXXX)</format>, where the number following uuid must be a 64-bit hexadecimal number representing the internal serial number of the camera.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// 	<paramref name="cameraName"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// 	<paramref name="cameraName"/> is an empty string.
        /// </exception>
        //==========================================================================================
        public static void Reset(string cameraName)
        {
            if (cameraName == null)
                throw ExceptionBuilder.ArgumentNull("cameraName");
            if (cameraName.Length == 0)
                throw ExceptionBuilder.EmptyString("cameraName");

            ImaqdxSessionManager.Reset(cameraName);
        }

        //==========================================================================================
        /// <summary>
        /// Performs a manual reset on all connected cameras. Stops any ongoing acquisitions.
        /// </summary>
        //==========================================================================================
        public static void ResetAll()
        {
            ImaqdxSessionManager.ResetAll();
        }

        //==========================================================================================
        /// <summary>
        /// Returns a list of all cameras on the host computer.
        /// </summary>
        /// <param name="connectedOnly">
        /// If  <paramref name="connectedOnly"/> is <see langword="true"/>, then the cameraInformationArray only contains cameras that are currently connected to the host computer. If <paramref name="connectedOnly"/> is <see langword="false"/>, then the cameraInformationArray contains cameras that are currently connected, and were previously connected, to the host computer.
        /// </param>
        /// <returns>
        /// A list of all cameras on the host computer.
        /// </returns>
        //==========================================================================================
        public static ImaqdxCameraInformation[] GetCameraInformation(bool connectedOnly)
        {
            return ImaqdxSessionManager.GetCameraInformation(connectedOnly);
        }
    }
}
