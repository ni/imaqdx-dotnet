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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace NationalInstruments.Vision.Acquisition.Imaqdx.Internal
{
    internal static class ImaqdxAttributeManager
    {
        private const ImaqdxAttributeVisibility PrivateVisibility = (ImaqdxAttributeVisibility)0x08000000;
        private static Dictionary<ImaqdxStandardAttribute, string> _attributeNamePairs = new Dictionary<ImaqdxStandardAttribute, string>();

        static ImaqdxAttributeManager()
        {
            _attributeNamePairs.Add(ImaqdxStandardAttribute.BaseAddress, "CameraInformation::BaseAddress");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.BusType, "CameraInformation::BusType");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.ModelName, "CameraInformation::ModelName");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.SerialNumberHigh, "CameraInformation::SerialNumberHigh");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.SerialNumberLow, "CameraInformation::SerialNumberLow");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.VendorName, "CameraInformation::VendorName");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.HostIPAddress, "CameraInformation::HostIPAddress");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.IPAddress, "CameraInformation::IPAddress");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.PrimaryUrlString, "CameraInformation::PrimaryURLString");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.SecondaryUrlString, "CameraInformation::SecondaryURLString");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.AcquisitionInProgress, "StatusInformation::AcqInProgress");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.LastBufferCount, "StatusInformation::LastBufferCount");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.LastBufferNumber, "StatusInformation::LastBufferNumber");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.LostBufferCount, "StatusInformation::LostBufferCount");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.LostPacketCount, "StatusInformation::LostPacketCount");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.RequestedResendPacketCount, "StatusInformation::RequestedResendPacketCount");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.ReceivedResendPackets, "StatusInformation::ReceivedResendPackets");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.HandledEventCount, "StatusInformation::HandledEventCount");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.LostEventCount, "StatusInformation::LostEventCount");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.BayerGainB, "AcquisitionAttributes::Bayer::GainB");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.BayerGainG, "AcquisitionAttributes::Bayer::GainG");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.BayerGainR, "AcquisitionAttributes::Bayer::GainR");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.BayerPattern, "AcquisitionAttributes::Bayer::Pattern");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.StreamChannelMode, "AcquisitionAttributes::Controller::StreamChannelMode");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.DesiredStreamChannel, "AcquisitionAttributes::Controller::DesiredStreamChannel");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.FrameInterval, "AcquisitionAttributes::FrameInterval");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.IgnoreFirstFrame, "AcquisitionAttributes::IgnoreFirstFrame");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.OffsetX, "AcquisitionAttributes::OffsetX");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.OffsetY, "AcquisitionAttributes::OffsetY");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.Width, "AcquisitionAttributes::Width");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.Height, "AcquisitionAttributes::Height");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.PixelFormat, "AcquisitionAttributes::PixelFormat");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.PacketSize, "AcquisitionAttributes::PacketSize");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.PayloadSize, "AcquisitionAttributes::PayloadSize");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.Speed, "AcquisitionAttributes::Speed");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.ShiftPixelBits, "AcquisitionAttributes::ShiftPixelBits");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.SwapPixelBytes, "AcquisitionAttributes::SwapPixelBytes");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.OverwriteMode, "AcquisitionAttributes::OverwriteMode");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.Timeout, "AcquisitionAttributes::Timeout");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.VideoMode, "AcquisitionAttributes::VideoMode");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.BitsPerPixel, "AcquisitionAttributes::BitsPerPixel");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.PixelSignedness, "AcquisitionAttributes::PixelSignedness");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.ReserveDualPackets, "AcquisitionAttributes::ReserveDualPackets");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.ReceiveTimestampMode, "AcquisitionAttributes::ReceiveTimestampMode");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.ActualPeakBandwidth, "AcquisitionAttributes::AdvancedEthernet::BandwidthControl::ActualPeakBandwidth");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.DesiredPeakBandwidth, "AcquisitionAttributes::AdvancedEthernet::BandwidthControl::DesiredPeakBandwidth");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.DestinationMode, "AcquisitionAttributes::AdvancedEthernet::Controller::DestinationMode");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.DestinationMulticastAddress, "AcquisitionAttributes::AdvancedEthernet::Controller::DestinationMulticastAddress");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.EventsEnabled, "AcquisitionAttributes::AdvancedEthernet::EventParameters::EventsEnabled");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.MaxOutstandingEvents, "AcquisitionAttributes::AdvancedEthernet::EventParameters::MaxOutstandingEvents");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.LostPacketMode, "AcquisitionAttributes::AdvancedEthernet::LostPacketMode");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.MemoryWindowSize, "AcquisitionAttributes::AdvancedEthernet::ResendParameters::MemoryWindowSize");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.ResendsEnabled, "AcquisitionAttributes::AdvancedEthernet::ResendParameters::ResendsEnabled");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.ResendThresholdPercentage, "AcquisitionAttributes::AdvancedEthernet::ResendParameters::ResendThresholdPercentage");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.ResendBatchingPercentage, "AcquisitionAttributes::AdvancedEthernet::ResendParameters::ResendBatchingPercentage");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.MaxResendsPerPacket, "AcquisitionAttributes::AdvancedEthernet::ResendParameters::MaxResendsPerPacket");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.ResendResponseTimeout, "AcquisitionAttributes::AdvancedEthernet::ResendParameters::ResendResponseTimeout");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.NewPacketTimeout, "AcquisitionAttributes::AdvancedEthernet::ResendParameters::NewPacketTimeout");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.MissingPacketTimeout, "AcquisitionAttributes::AdvancedEthernet::ResendParameters::MissingPacketTimeout");
            _attributeNamePairs.Add(ImaqdxStandardAttribute.ResendTimerResolution, "AcquisitionAttributes::AdvancedEthernet::ResendParameters::ResendTimerResolution");
        }

        public static Dictionary<ImaqdxStandardAttribute, string> AttributeNamePairs
        {
            get { return _attributeNamePairs; }
        }

        #region Attribute Functions

        public static ImaqdxAttributeInformation[] EnumeratePublicAttributes(ImaqdxSessionHandle session)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");

            uint count;
            int status = NiImaqdxDll.IMAQdxEnumerateAttributes2(session, null, out count, "", ImaqdxAttributeVisibility.Advanced);
            ExceptionBuilder.CheckErrorAndThrow(status);
            ImaqdxAttributeInformation[] attributeInfoArray = new ImaqdxAttributeInformation[count];
            status = NiImaqdxDll.IMAQdxEnumerateAttributes2(session, attributeInfoArray, out count, "", ImaqdxAttributeVisibility.Advanced);
            ExceptionBuilder.CheckErrorAndThrow(status);
            return attributeInfoArray;
        }

        public static ImaqdxAttributeInformation[] EnumeratePrivateAttributes(ImaqdxSessionHandle session)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");

            uint count;
            int status = NiImaqdxDll.IMAQdxEnumerateAttributes2(session, null, out count, "", PrivateVisibility);
            ExceptionBuilder.CheckErrorAndThrow(status);
            ImaqdxAttributeInformation[] attributeInfoArray = new ImaqdxAttributeInformation[count];
            status = NiImaqdxDll.IMAQdxEnumerateAttributes2(session, attributeInfoArray, out count, "", PrivateVisibility);
            ExceptionBuilder.CheckErrorAndThrow(status);
            return attributeInfoArray;
        }

        public static string GetAttributeDescription(ImaqdxSessionHandle session, string name)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            string description;
            int status = NiImaqdxDll.IMAQdxGetAttributeDescriptionCW(session, name, out description);
            ExceptionBuilder.CheckErrorAndThrow(status);
            return description == null ? string.Empty : description;
        }

        public static string GetAttributeDisplayName(ImaqdxSessionHandle session, string name)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            string displayName;
            int status = NiImaqdxDll.IMAQdxGetAttributeDisplayNameCW(session, name, out displayName);
            ExceptionBuilder.CheckErrorAndThrow(status);
            return displayName == null ? string.Empty : displayName;
        }

        public static string GetAttributeTooltip(ImaqdxSessionHandle session, string name)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            string tooltip;
            int status = NiImaqdxDll.IMAQdxGetAttributeTooltipCW(session, name, out tooltip);
            ExceptionBuilder.CheckErrorAndThrow(status);
            return tooltip == null ? string.Empty : tooltip;
        }

        public static string GetAttributeUnits(ImaqdxSessionHandle session, string name)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            string units;
            int status = NiImaqdxDll.IMAQdxGetAttributeUnitsCW(session, name, out units);
            ExceptionBuilder.CheckErrorAndThrow(status);
            return units == null ? string.Empty : units;
        }

        public static ImaqdxAttributeVisibility GetAttributeVisibility(ImaqdxSessionHandle session, string name)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            ImaqdxAttributeVisibility visibility;
            int status = NiImaqdxDll.IMAQdxGetAttributeVisibility(session, name, out visibility);
            ExceptionBuilder.CheckErrorAndThrow(status);
            return visibility;
        }

        public static bool GetIsAttributeReadable(ImaqdxSessionHandle session, string name)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            uint readable;
            int status = NiImaqdxDll.IMAQdxIsAttributeReadable(session, name, out readable);
            ExceptionBuilder.CheckErrorAndThrow(status);
            return Convert.ToBoolean(readable);
        }

        public static bool GetIsAttributeWritable(ImaqdxSessionHandle session, string name)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            uint writable;
            int status = NiImaqdxDll.IMAQdxIsAttributeWritable(session, name, out writable);
            ExceptionBuilder.CheckErrorAndThrow(status);
            return Convert.ToBoolean(writable);
        }

        public static void GetAttributeMinimum(ImaqdxSessionHandle session, string name, out uint value)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxGetAttributeMinimum(session, name, ImaqdxAttributeType.UInt32, out value);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void GetAttributeMinimum(ImaqdxSessionHandle session, string name, out long value)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxGetAttributeMinimum(session, name, ImaqdxAttributeType.Int64, out value);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void GetAttributeMinimum(ImaqdxSessionHandle session, string name, out double value)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxGetAttributeMinimum(session, name, ImaqdxAttributeType.Double, out value);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void GetAttributeMaximum(ImaqdxSessionHandle session, string name, out uint value)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxGetAttributeMaximum(session, name, ImaqdxAttributeType.UInt32, out value);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void GetAttributeMaximum(ImaqdxSessionHandle session, string name, out long value)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxGetAttributeMaximum(session, name, ImaqdxAttributeType.Int64, out value);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void GetAttributeMaximum(ImaqdxSessionHandle session, string name, out double value)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxGetAttributeMaximum(session, name, ImaqdxAttributeType.Double, out value);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void GetAttributeIncrement(ImaqdxSessionHandle session, string name, out uint value)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxGetAttributeIncrement(session, name, ImaqdxAttributeType.UInt32, out value);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void GetAttributeIncrement(ImaqdxSessionHandle session, string name, out long value)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxGetAttributeIncrement(session, name, ImaqdxAttributeType.Int64, out value);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void GetAttributeIncrement(ImaqdxSessionHandle session, string name, out double value)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxGetAttributeIncrement(session, name, ImaqdxAttributeType.Double, out value);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static ImaqdxEnumAttributeItem[] EnumerateAttributeValues(ImaqdxSessionHandle session, string name)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            uint count;
            int status = NiImaqdxDll.IMAQdxEnumerateAttributeValues(session, name, null, out count);
            ExceptionBuilder.CheckErrorAndThrow(status);
            ImaqdxEnumItem[] enumItemArray = new ImaqdxEnumItem[count];
            status = NiImaqdxDll.IMAQdxEnumerateAttributeValues(session, name, enumItemArray, out count);
            ExceptionBuilder.CheckErrorAndThrow(status);

            ImaqdxEnumAttributeItem[] enumItems = new ImaqdxEnumAttributeItem[count];
            for (int i = 0; i < count; i++)
            {
                enumItems[i] = new ImaqdxEnumAttributeItem(enumItemArray[i]);
            }
            return enumItems;
        }

        #endregion

        #region GetAttribute Functions

        public static void GetAttribute(ImaqdxSessionHandle session, string name, out uint value)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxGetAttribute(session, name, ImaqdxAttributeType.UInt32, out value);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void GetAttribute(ImaqdxSessionHandle session, string name, out long value)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxGetAttribute(session, name, ImaqdxAttributeType.Int64, out value);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void GetAttribute(ImaqdxSessionHandle session, string name, out double value)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxGetAttribute(session, name, ImaqdxAttributeType.Double, out value);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void GetAttribute(ImaqdxSessionHandle session, string name, out string value)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            object objectValue;
            int status = NiImaqdxDll.IMAQdxGetAttributeCW(session, name, ImaqdxAttributeType.String, out objectValue);
            ExceptionBuilder.CheckErrorAndThrow(status);
            value = (objectValue == null ? string.Empty : (string)objectValue);
        }

        public static void GetAttribute(ImaqdxSessionHandle session, string name, out ImaqdxEnumAttributeItem value)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            ImaqdxEnumItem enumItemValue;
            int status = NiImaqdxDll.IMAQdxGetAttribute(session, name, ImaqdxAttributeType.Enum, out enumItemValue);
            ExceptionBuilder.CheckErrorAndThrow(status);
            value = new ImaqdxEnumAttributeItem(enumItemValue);
        }

        public static void GetAttribute(ImaqdxSessionHandle session, string name, out bool value)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            byte byteValue;
            int status = NiImaqdxDll.IMAQdxGetAttribute(session, name, ImaqdxAttributeType.Boolean, out byteValue);
            ExceptionBuilder.CheckErrorAndThrow(status);
            value = Convert.ToBoolean(byteValue);
        }

        #endregion

        #region SetAttribute Functions

        public static void SetAttribute(ImaqdxSessionHandle session, string name, uint value)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxSetAttribute(session, name, ImaqdxAttributeType.UInt32, value);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void SetAttribute(ImaqdxSessionHandle session, string name, long value)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxSetAttribute(session, name, ImaqdxAttributeType.Int64, value);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void SetAttribute(ImaqdxSessionHandle session, string name, double value)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxSetAttribute(session, name, ImaqdxAttributeType.Double, value);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void SetAttribute(ImaqdxSessionHandle session, string name, string value)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            int status = NiImaqdxDll.IMAQdxSetAttribute(session, name, ImaqdxAttributeType.String, value);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void SetAttribute(ImaqdxSessionHandle session, string name, ImaqdxEnumAttributeItem value)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            ImaqdxEnumItem enumItemValue = value.EnumItem;
            int status = NiImaqdxDll.IMAQdxSetAttribute(session, name, ImaqdxAttributeType.Enum, enumItemValue);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        public static void SetAttribute(ImaqdxSessionHandle session, string name, bool value)
        {
            Debug.Assert(session != null, "The session parameter cannot be null.");
            Debug.Assert(!session.IsInvalid, "The session parameter must be a valid handle.");
            Debug.Assert(name != null, "The name parameter cannot be null.");

            byte byteValue = Convert.ToByte(value);
            int status = NiImaqdxDll.IMAQdxSetAttribute(session, name, ImaqdxAttributeType.Boolean, byteValue);
            ExceptionBuilder.CheckErrorAndThrow(status);
        }

        #endregion
    }
}
