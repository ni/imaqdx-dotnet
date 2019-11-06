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
using System.Net;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace NationalInstruments.Vision.Acquisition.Imaqdx.Internal
{
    internal static class ImaqdxSessionManager
    {
        #region Camera Functions

        public static ImaqdxSessionHandle Open(string cameraName, ImaqdxCameraControlMode mode)
        {
            Debug.Assert(cameraName != null, "The cameraName parameter cannot be null.");

            ImaqdxSessionHandle session;
            int status = NiImaqdxDll.IMAQdxOpenCamera(cameraName, mode, out session);
            ExceptionBuilder.CheckErrorAndThrow(status);
            return session;
        }

        public static void Close(ImaqdxSessionHandle session)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");

            int status = NiImaqdxDll.IMAQdxCloseCamera(session);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void Reset(string cameraName)
        {
            Debug.Assert(cameraName != null, "The cameraName parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxResetCamera(cameraName, Convert.ToUInt32(false));
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void ResetAll()
        {
            int status = NiImaqdxDll.IMAQdxResetCamera(String.Empty, Convert.ToUInt32(true));
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void ResetEthernetCameraAddress(string name, IPAddress address, IPAddress subnet, IPAddress gateway, int timeout)
        {
            Debug.Assert(name != null, "The name parameter cannot be null.");
            Debug.Assert(address != null, "The address parameter cannot be null.");
            Debug.Assert(subnet != null, "The subnet parameter cannot be null.");
            Debug.Assert(gateway != null, "The gateway parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxResetEthernetCameraAddress(name, address.ToString(), subnet.ToString(), gateway.ToString(), timeout);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static Imaqdx.ImaqdxCameraInformation[] GetCameraInformation(bool connectedOnly)
        {
            uint count;
            int status = NiImaqdxDll.IMAQdxEnumerateCameras(null, out count, Convert.ToUInt32(connectedOnly));
            ExceptionBuilder.CheckErrorAndThrow(status);
            Internal.ImaqdxCameraInformation[] cameraInfoArray = new Internal.ImaqdxCameraInformation[count];
            status = NiImaqdxDll.IMAQdxEnumerateCameras(cameraInfoArray, out count, Convert.ToUInt32(connectedOnly));
            ExceptionBuilder.CheckErrorAndThrow(status);

            return ImaqdxCameraInformation.CreateCameraInformationArray(cameraInfoArray);
        }

        public static void DiscoverEthernetCameras(IPAddress address, int timeout)
        {
            Debug.Assert(address != null, "The address parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxDiscoverEthernetCameras(address.ToString(), timeout);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        #endregion

        #region Attribute Functions

        public static void WriteAttributesToCameraFile(ImaqdxSessionHandle session)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");

            int status = NiImaqdxDll.IMAQdxWriteAttributes(session, null);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void WriteAttributesToFile(ImaqdxSessionHandle session, string fullPath)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(fullPath != null, "The fullPath parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxWriteAttributes(session, fullPath);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void ReadAttributesFromCameraFile(ImaqdxSessionHandle session)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");

            int status = NiImaqdxDll.IMAQdxReadAttributes(session, null);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void ReadAttributesFromFile(ImaqdxSessionHandle session, string fullPath)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(fullPath != null, "The fullPath parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxReadAttributes(session, fullPath);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static string WriteAttributesToString(ImaqdxSessionHandle session)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");

            IntPtr stringPtr = IntPtr.Zero;
            try
            {
                int status = NiImaqdxDll.IMAQdxWriteAttributesToString(session, ref stringPtr);
                ExceptionBuilder.CheckErrorAndThrow(status);
                string attributesString = Marshal.PtrToStringAnsi(stringPtr);
                return attributesString == null ? string.Empty : attributesString;
            }
            finally
            {
                if (stringPtr != IntPtr.Zero)
                {
                    int status = NiImaqdxDll.IMAQdxDispose(stringPtr);
                    ExceptionBuilder.CheckErrorAndThrow(status);
                }
            }
        }

        public static void ReadAttributesFromString(ImaqdxSessionHandle session, string attributesString)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(attributesString != null, "The attributesString parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxReadAttributesFromString(session, attributesString);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static string GetFullAttributeName(ImaqdxSessionHandle session, string partialName)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(partialName != null, "The partialName parameter cannot be null.");

            IntPtr stringPtr = IntPtr.Zero;
            try
            {
                int status = NiImaqdxDll.IMAQdxGetFullyQualifiedAttributeName(session, partialName, ref stringPtr);
                if (status == -1074360305)
                {
                    // This error is returned when the attribute is not supported by the camera
                    // In this case we do not want to return an exception
                    // Return an empty string to indicate the the attribute was not found
                    return string.Empty;
                }

                ExceptionBuilder.CheckErrorAndThrow(status);
                return Marshal.PtrToStringAnsi(stringPtr);
            }
            finally
            {
                if (stringPtr != IntPtr.Zero)
                {
                    int status = NiImaqdxDll.IMAQdxDispose(stringPtr);
                    ExceptionBuilder.CheckErrorAndThrow(status);
                }
            }
        }

        #endregion

        #region High-Level Acquisition Functions

        public static void Snap(ImaqdxSessionHandle session, VisionImage image)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(image != null, "The image parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxSnap(session, image._image);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void ConfigureGrab(ImaqdxSessionHandle session)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");

            int status = NiImaqdxDll.IMAQdxConfigureGrab(session);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static uint Grab(ImaqdxSessionHandle session, VisionImage image, bool waitForNextBuffer)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(image != null, "The image parameter cannot be null.");

            uint actualBufferNumber;
            int status = NiImaqdxDll.IMAQdxGrab(session, image._image, Convert.ToUInt32(waitForNextBuffer), out actualBufferNumber);
            ExceptionBuilder.CheckErrorAndThrow(status);
            return actualBufferNumber;
        }

        public static void Sequence(ImaqdxSessionHandle session, VisionImage[] images)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(images != null, "The images parameter cannot be null.");

            IntPtr[] imagePtrs = new IntPtr[images.Length];
            for (int i = 0; i < images.Length; i++)
            {
                imagePtrs[i] = images[i]._image;
            }
            int status = NiImaqdxDll.IMAQdxSequence(session, imagePtrs, (uint)imagePtrs.Length);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        #endregion

        #region Low-Level Acquisition Functions

        public static void ConfigureAcquisition(ImaqdxSessionHandle session, ImaqdxAcquisitionType acquisitionType, int bufferCount)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(bufferCount > 0, "The bufferCount parameter must be greater than zero.");

            bool isContinuous = (acquisitionType == ImaqdxAcquisitionType.Continuous);
            int status = NiImaqdxDll.IMAQdxConfigureAcquisition(session, Convert.ToUInt32(isContinuous), (uint)bufferCount);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void StartAcquisition(ImaqdxSessionHandle session)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");

            int status = NiImaqdxDll.IMAQdxStartAcquisition(session);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void StopAcquisition(ImaqdxSessionHandle session)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");

            int status = NiImaqdxDll.IMAQdxStopAcquisition(session);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void UnconfigureAcquisition(ImaqdxSessionHandle session)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");

            int status = NiImaqdxDll.IMAQdxUnconfigureAcquisition(session);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static uint GetImage(ImaqdxSessionHandle session, ImaqdxBufferNumberMode bufferNumberMode, VisionImage image)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(image != null, "The image parameter cannot be null.");

            uint actualBufferNumber;
            int status = NiImaqdxDll.IMAQdxGetImage(session, image._image, bufferNumberMode, (uint)0, out actualBufferNumber);
            ExceptionBuilder.CheckErrorAndThrow(status);
            return actualBufferNumber;
        }

        public static uint GetImage(ImaqdxSessionHandle session, uint bufferNumber, VisionImage image)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(image != null, "The image parameter cannot be null.");

            uint actualBufferNumber;
            uint truncatedBufferNumber = (bufferNumber & 0x0FFFFFFF);
            int status = NiImaqdxDll.IMAQdxGetImage(session, image._image, ImaqdxBufferNumberMode.BufferNumber, truncatedBufferNumber, out actualBufferNumber);
            ExceptionBuilder.CheckErrorAndThrow(status);
            return actualBufferNumber;
        }

        public static uint GetImageData(ImaqdxSessionHandle session, ImaqdxBufferNumberMode bufferNumberMode, ref byte[] data)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");

            CreateImageDataArray(session, ref data);

            uint actualBufferNumber;
            int status = NiImaqdxDll.IMAQdxGetImageData(session, data, (uint)data.Length, bufferNumberMode, (uint)0, out actualBufferNumber);
            ExceptionBuilder.CheckErrorAndThrow(status);
            return actualBufferNumber;
        }

        public static uint GetImageData(ImaqdxSessionHandle session, uint bufferNumber, ref byte[] data)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");

            CreateImageDataArray(session, ref data);

            uint actualBufferNumber;
            uint truncatedBufferNumber = (bufferNumber & 0x0FFFFFFF);
            int status = NiImaqdxDll.IMAQdxGetImageData(session, data, (uint)data.Length, ImaqdxBufferNumberMode.BufferNumber, truncatedBufferNumber, out actualBufferNumber);
            ExceptionBuilder.CheckErrorAndThrow(status);
            return actualBufferNumber;
        }

        private static void CreateImageDataArray(ImaqdxSessionHandle session, ref byte[] data)
        {
            uint bufferSize;
            int status = NiImaqdxDll.IMAQdxGetRawBufferSize(session, out bufferSize);
            ExceptionBuilder.CheckErrorAndThrow(status);

            if (data == null || data.Length != bufferSize)
            {
                data = new byte[bufferSize];
            }
        }

        #endregion

        #region Low-Level Register Functions

        public static void WriteRegister(ImaqdxSessionHandle session, ulong offset, uint value)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");

            int status = NiImaqdxDll.IMAQdxWriteRegister(session, (uint)offset, value);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static uint ReadRegister(ImaqdxSessionHandle session, ulong offset)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");

            uint value;
            int status = NiImaqdxDll.IMAQdxReadRegister(session, (uint)offset, out value);
            ExceptionBuilder.CheckErrorAndThrow(status);
            return value;
        }

        public static void WriteMemory(ImaqdxSessionHandle session, ulong offset, byte[] values)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(values != null, "The values parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxWriteMemory(session, (uint)offset, values, (uint)values.Length);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static byte[] ReadMemory(ImaqdxSessionHandle session, ulong offset, int count)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(count > 0, "The count parameter must be greater than zero.");

            byte[] values = new byte[count];
            int status = NiImaqdxDll.IMAQdxReadMemory(session, (uint)offset, values, (uint)count);
            ExceptionBuilder.CheckErrorAndThrow(status);
            return values;
        }

        #endregion
    }
}
