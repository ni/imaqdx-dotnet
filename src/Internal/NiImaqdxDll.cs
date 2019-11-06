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
using System.Security;
using System.Text;

namespace NationalInstruments.Vision.Acquisition.Imaqdx.Internal
{
    [SuppressUnmanagedCodeSecurityAttribute]
    internal static class NiImaqdxDll
    {
        private const string ImaqdxCDll = "niimaqdx.dll";

        #region Camera Functions

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxOpenCamera", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxOpenCamera(string name, ImaqdxCameraControlMode mode, out ImaqdxSessionHandle id);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxCloseCamera")]
        public static extern int IMAQdxCloseCamera(ImaqdxSessionHandle id);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxCloseCamera")]
        public static extern int IMAQdxCloseCamera(HandleRef id);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxResetCamera", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxResetCamera(string name, uint resetAll);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxResetEthernetCameraAddress", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxResetEthernetCameraAddress(string name, string address, string subnet, string gateway, int timeout);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxEnumerateCameras")]
        public static extern int IMAQdxEnumerateCameras([In, Out] Internal.ImaqdxCameraInformation[] cameraInformationArray, out uint count, uint connectedOnly);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxDiscoverEthernetCameras", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxDiscoverEthernetCameras(string address, int timeout);

        #endregion

        #region Attribute Functions

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxEnumerateAttributes2", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxEnumerateAttributes2(ImaqdxSessionHandle id, [In, Out] ImaqdxAttributeInformation[] attributeInformationArray, out uint count, string root, ImaqdxAttributeVisibility visibility);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetAttributeDescriptionCW")]
        public static extern int IMAQdxGetAttributeDescriptionCW(ImaqdxSessionHandle id, string name, [MarshalAs(UnmanagedType.BStr)] out string description);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetAttributeDisplayNameCW")]
        public static extern int IMAQdxGetAttributeDisplayNameCW(ImaqdxSessionHandle id, string name, [MarshalAs(UnmanagedType.BStr)] out string displayName);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetAttributeTooltipCW")]
        public static extern int IMAQdxGetAttributeTooltipCW(ImaqdxSessionHandle id, string name, [MarshalAs(UnmanagedType.BStr)] out string toolTip);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetAttributeUnitsCW")]
        public static extern int IMAQdxGetAttributeUnitsCW(ImaqdxSessionHandle id, string name, [MarshalAs(UnmanagedType.BStr)] out string units);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetAttributeVisibility", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxGetAttributeVisibility(ImaqdxSessionHandle id, string name, out ImaqdxAttributeVisibility visibility);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxIsAttributeReadable", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxIsAttributeReadable(ImaqdxSessionHandle id, string name, out uint readable);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxIsAttributeWritable", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxIsAttributeWritable(ImaqdxSessionHandle id, string name, out uint writable);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetAttributeMinimum", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxGetAttributeMinimum(ImaqdxSessionHandle id, string name, ImaqdxAttributeType type, out uint val);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetAttributeMinimum", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxGetAttributeMinimum(ImaqdxSessionHandle id, string name, ImaqdxAttributeType type, out long val);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetAttributeMinimum", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxGetAttributeMinimum(ImaqdxSessionHandle id, string name, ImaqdxAttributeType type, out double val);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetAttributeMaximum", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxGetAttributeMaximum(ImaqdxSessionHandle id, string name, ImaqdxAttributeType type, out uint val);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetAttributeMaximum", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxGetAttributeMaximum(ImaqdxSessionHandle id, string name, ImaqdxAttributeType type, out long val);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetAttributeMaximum", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxGetAttributeMaximum(ImaqdxSessionHandle id, string name, ImaqdxAttributeType type, out double val);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetAttributeIncrement", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxGetAttributeIncrement(ImaqdxSessionHandle id, string name, ImaqdxAttributeType type, out uint val);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetAttributeIncrement", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxGetAttributeIncrement(ImaqdxSessionHandle id, string name, ImaqdxAttributeType type, out long val);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetAttributeIncrement", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxGetAttributeIncrement(ImaqdxSessionHandle id, string name, ImaqdxAttributeType type, out double val);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxEnumerateAttributeValues", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxEnumerateAttributeValues(ImaqdxSessionHandle id, string name, [In, Out] ImaqdxEnumItem[] list, out uint size);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetAttribute", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxGetAttribute(ImaqdxSessionHandle id, string name, ImaqdxAttributeType type, out uint val);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetAttribute", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxGetAttribute(ImaqdxSessionHandle id, string name, ImaqdxAttributeType type, out long val);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetAttribute", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxGetAttribute(ImaqdxSessionHandle id, string name, ImaqdxAttributeType type, out double val);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetAttributeCW", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxGetAttributeCW(ImaqdxSessionHandle id, string name, ImaqdxAttributeType type, out object val);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetAttribute", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxGetAttribute(ImaqdxSessionHandle id, string name, ImaqdxAttributeType type, out ImaqdxEnumItem val);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetAttribute", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxGetAttribute(ImaqdxSessionHandle id, string name, ImaqdxAttributeType type, out byte val);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxSetAttribute", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxSetAttribute(ImaqdxSessionHandle id, string name, ImaqdxAttributeType type, uint val);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxSetAttribute", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxSetAttribute(ImaqdxSessionHandle id, string name, ImaqdxAttributeType type, long val);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxSetAttribute", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxSetAttribute(ImaqdxSessionHandle id, string name, ImaqdxAttributeType type, double val);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxSetAttribute", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxSetAttribute(ImaqdxSessionHandle id, string name, ImaqdxAttributeType type, string val);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxSetAttribute", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxSetAttribute(ImaqdxSessionHandle id, string name, ImaqdxAttributeType type, ImaqdxEnumItem val);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxSetAttribute", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxSetAttribute(ImaqdxSessionHandle id, string name, ImaqdxAttributeType type, byte val);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxWriteAttributes", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxWriteAttributes(ImaqdxSessionHandle id, string filename);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxReadAttributes", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxReadAttributes(ImaqdxSessionHandle id, string filename);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxWriteAttributesToString")]
        public static extern int IMAQdxWriteAttributesToString(ImaqdxSessionHandle id, ref IntPtr attributesString);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxReadAttributesFromString", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxReadAttributesFromString(ImaqdxSessionHandle id, string attributesString);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetFullyQualifiedAttributeName", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int IMAQdxGetFullyQualifiedAttributeName(ImaqdxSessionHandle id, string partialName, ref IntPtr fullName);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxDispose")]
        public static extern int IMAQdxDispose(IntPtr buffer);

        #endregion

        #region High-Level Acquisition Functions

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxSnap")]
        public static extern int IMAQdxSnap(ImaqdxSessionHandle id, IntPtr image);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxConfigureGrab")]
        public static extern int IMAQdxConfigureGrab(ImaqdxSessionHandle id);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGrab")]
        public static extern int IMAQdxGrab(ImaqdxSessionHandle id, IntPtr image, uint waitForNextBuffer, out uint actualBufferNumber);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxSequence")]
        public static extern int IMAQdxSequence(ImaqdxSessionHandle id, IntPtr[] images, uint count);

        #endregion

        #region Low-Level Acquisition Functions

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxConfigureAcquisition")]
        public static extern int IMAQdxConfigureAcquisition(ImaqdxSessionHandle id, uint continuous, uint bufferCount);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxStartAcquisition")]
        public static extern int IMAQdxStartAcquisition(ImaqdxSessionHandle id);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxStopAcquisition")]
        public static extern int IMAQdxStopAcquisition(ImaqdxSessionHandle id);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxUnconfigureAcquisition")]
        public static extern int IMAQdxUnconfigureAcquisition(ImaqdxSessionHandle id);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetImage")]
        public static extern int IMAQdxGetImage(ImaqdxSessionHandle id, IntPtr image, ImaqdxBufferNumberMode mode, uint desiredBufferNumber, out uint actualBufferNumber);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetImageData")]
        public static extern int IMAQdxGetImageData(ImaqdxSessionHandle id, [In, Out] byte[] buffer, uint bufferSize, ImaqdxBufferNumberMode mode, uint desiredBufferNumber, out uint actualBufferNumber);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetRawBufferSize")]
        public static extern int IMAQdxGetRawBufferSize(ImaqdxSessionHandle id, out uint bufferSize);

        #endregion

        #region Low-Level Register Functions

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxWriteRegister")]
        public static extern int IMAQdxWriteRegister(ImaqdxSessionHandle id, uint offset, uint value);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxReadRegister")]
        public static extern int IMAQdxReadRegister(ImaqdxSessionHandle id, uint offset, out uint value);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxWriteMemory")]
        public static extern int IMAQdxWriteMemory(ImaqdxSessionHandle id, uint offset, [In] byte[] values, uint count);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxReadMemory")]
        public static extern int IMAQdxReadMemory(ImaqdxSessionHandle id, uint offset, [In, Out] byte[] values, uint count);

        #endregion

        #region Event Functions

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxRegisterFrameDoneEvent")]
        public static extern int IMAQdxRegisterFrameDoneEvent(ImaqdxSessionHandle id, uint bufferInterval, ImaqdxFrameDoneEventHandler callbackFunction, IntPtr callbackData);

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxRegisterPnpEvent")]
        public static extern int IMAQdxRegisterPnpEvent(ImaqdxSessionHandle id, ImaqdxPnpEvent pnpEvent, ImaqdxPnpEventHandler callbackFunction, IntPtr callbackData);

        #endregion

        [DllImport(ImaqdxCDll, EntryPoint = "IMAQdxGetErrorStringCW")]
        public static extern int IMAQdxGetErrorStringCW(int error, [MarshalAs(UnmanagedType.BStr)] out string errorMessage);

    }
}
