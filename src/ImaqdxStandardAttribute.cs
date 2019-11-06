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
    /// Specifies the attributes you can use with <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession.Attributes" crefType="Unqualified"/>.
    /// </summary>
    //==============================================================================================
    public enum ImaqdxStandardAttribute : int
    {
        //==========================================================================================
        /// <summary>
        /// The base address of the camera registers.
        /// </summary>
        //==========================================================================================
        BaseAddress,
        //==========================================================================================
        /// <summary>
        /// The bus type of the camera.
        /// </summary>
        //==========================================================================================
        BusType,
        //==========================================================================================
        /// <summary>
        /// The model name of the camera.
        /// </summary>
        //==========================================================================================
        ModelName,
        //==========================================================================================
        /// <summary>
        /// The upper 32-bits of the camera 64-bit serial number.
        /// </summary>
        //==========================================================================================
        SerialNumberHigh,
        //==========================================================================================
        /// <summary>
        /// The lower 32-bits of the camera 64-bit serial number.
        /// </summary>
        //==========================================================================================
        SerialNumberLow,
        //==========================================================================================
        /// <summary>
        /// The vendor name of the camera.
        /// </summary>
        //==========================================================================================
        VendorName,
        //==========================================================================================
        /// <summary>
        /// The host adapter IP address.
        /// </summary>
        //==========================================================================================
        HostIPAddress,
        //==========================================================================================
        /// <summary>
        /// The IP address of the camera.
        /// </summary>
        //==========================================================================================
        IPAddress,
        //==========================================================================================
        /// <summary>
        /// The camera's primary URL string.
        /// </summary>
        //==========================================================================================
        PrimaryUrlString,
        //==========================================================================================
        /// <summary>
        /// The camera's secondary URL string.
        /// </summary>
        //==========================================================================================
        SecondaryUrlString,
        //==========================================================================================
        /// <summary>
        /// The current state of the acquisition.
        /// </summary>
        //==========================================================================================
        AcquisitionInProgress,
        //==========================================================================================
        /// <summary>
        /// The number of transferred buffers.
        /// </summary>
        //==========================================================================================
        LastBufferCount,
        //==========================================================================================
        /// <summary>
        /// The last cumulative buffer number transferred.
        /// </summary>
        //==========================================================================================
        LastBufferNumber,
        //==========================================================================================
        /// <summary>
        /// The number of lost buffers during an acquisition session.
        /// </summary>
        //==========================================================================================
        LostBufferCount,
        //==========================================================================================
        /// <summary>
        /// The number of lost packets during an acquisition session.
        /// </summary>
        //==========================================================================================
        LostPacketCount,
        //==========================================================================================
        /// <summary>
        /// The number of packets requested to be resent during an acquisition session.
        /// </summary>
        //==========================================================================================
        RequestedResendPacketCount,
        //==========================================================================================
        /// <summary>
        /// The number of packets that were requested to be resent during an acquisition session and were completed.
        /// </summary>
        //==========================================================================================
        ReceivedResendPackets,
        //==========================================================================================
        /// <summary>
        /// The number of handled events during an acquisition session.
        /// </summary>
        //==========================================================================================
        HandledEventCount,
        //==========================================================================================
        /// <summary>
        /// The number of lost events during an acquisition session.
        /// </summary>
        //==========================================================================================
        LostEventCount,
        //==========================================================================================
        /// <summary>
        /// The white balance gain for the blue component of the Bayer conversion.
        /// </summary>
        //==========================================================================================
        BayerGainB,
        //==========================================================================================
        /// <summary>
        /// The white balance gain for the green component of the Bayer conversion.
        /// </summary>
        //==========================================================================================
        BayerGainG,
        //==========================================================================================
        /// <summary>
        /// The white balance gain for the red component of the Bayer conversion.
        /// </summary>
        //==========================================================================================
        BayerGainR,
        //==========================================================================================
        /// <summary>
        /// The Bayer pattern to use.
        /// </summary>
        //==========================================================================================
        BayerPattern,
        //==========================================================================================
        /// <summary>
        /// The mode for allocating a FireWire stream channel.
        /// </summary>
        //==========================================================================================
        StreamChannelMode,
        //==========================================================================================
        /// <summary>
        /// The stream channel to manually allocate.
        /// </summary>
        //==========================================================================================
        DesiredStreamChannel,
        //==========================================================================================
        /// <summary>
        /// The duration in milliseconds between successive frames.
        /// </summary>
        //==========================================================================================
        FrameInterval,
        //==========================================================================================
        /// <summary>
        /// The video delay of one frame between starting the camera and receiving the video feed.
        /// </summary>
        //==========================================================================================
        IgnoreFirstFrame,
        //==========================================================================================
        /// <summary>
        /// The left offset of the image.
        /// </summary>
        //==========================================================================================
        OffsetX,
        //==========================================================================================
        /// <summary>
        /// The top offset of the image.
        /// </summary>
        //==========================================================================================
        OffsetY,
        //==========================================================================================
        /// <summary>
        /// The width of the image.
        /// </summary>
        //==========================================================================================
        Width,
        //==========================================================================================
        /// <summary>
        /// The height of the image.
        /// </summary>
        //==========================================================================================
        Height,
        //==========================================================================================
        /// <summary>
        /// The pixel format of the source sensor.
        /// </summary>
        //==========================================================================================
        PixelFormat,
        //==========================================================================================
        /// <summary>
        /// The packet size in bytes.
        /// </summary>
        //==========================================================================================
        PacketSize,
        //==========================================================================================
        /// <summary>
        /// The frame size in bytes.
        /// </summary>
        //==========================================================================================
        PayloadSize,
        //==========================================================================================
        /// <summary>
        /// The transfer speed in Mbps for a FireWire packet.
        /// </summary>
        //==========================================================================================
        Speed,
        //==========================================================================================
        /// <summary>
        /// The alignment of 16-bit cameras. Downshift the pixel bits if the camera returns most significant bit-aligned data.
        /// </summary>
        //==========================================================================================
        ShiftPixelBits,
        //==========================================================================================
        /// <summary>
        /// The endianness of 16-bit cameras. Swap the pixel bytes if the camera returns little endian data.
        /// </summary>
        //==========================================================================================
        SwapPixelBytes,
        //==========================================================================================
        /// <summary>
        /// The overwrite mode used to determine acquisition when an image transfer cannot be completed due to an overwritten internal buffer.
        /// </summary>
        //==========================================================================================
        OverwriteMode,
        //==========================================================================================
        /// <summary>
        /// The timeout value in milliseconds, used to abort an acquisition when the image transfer cannot be completed within the delay.
        /// </summary>
        //==========================================================================================
        Timeout,
        //==========================================================================================
        /// <summary>
        /// The video mode for a camera.
        /// </summary>
        //==========================================================================================
        VideoMode,
        //==========================================================================================
        /// <summary>
        /// The actual bits per pixel. For 16-bit components, this represents the actual bit depth (10-, 12-, 14-, or 16-bit).
        /// </summary>
        //==========================================================================================
        BitsPerPixel,
        //==========================================================================================
        /// <summary>
        /// The signedness of the pixel. For 16-bit components, this represents the actual pixel signedness (Signed, or Unsigned).
        /// </summary>
        //==========================================================================================
        PixelSignedness,
        //==========================================================================================
        /// <summary>
        /// Whether dual packets will be reserved for a very large FireWire packet.
        /// </summary>
        //==========================================================================================
        ReserveDualPackets,
        //==========================================================================================
        /// <summary>
        /// The mode for timestamping images received by the driver.
        /// </summary>
        //==========================================================================================
        ReceiveTimestampMode,
        //==========================================================================================
        /// <summary>
        /// The actual maximum peak bandwidth the camera will be configured to use.
        /// </summary>
        //==========================================================================================
        ActualPeakBandwidth,
        //==========================================================================================
        /// <summary>
        /// The desired maximum peak bandwidth the camera should use.
        /// </summary>
        //==========================================================================================
        DesiredPeakBandwidth,
        //==========================================================================================
        /// <summary>
        /// Location where the camera is instructed to send the image stream.
        /// </summary>
        //==========================================================================================
        DestinationMode,
        //==========================================================================================
        /// <summary>
        /// The multicast address the camera should send data in multicast mode.
        /// </summary>
        //==========================================================================================
        DestinationMulticastAddress,
        //==========================================================================================
        /// <summary>
        /// </summary>
        //==========================================================================================
        EventsEnabled,
        //==========================================================================================
        /// <summary>
        /// The maximum number of outstanding events to queue.
        /// </summary>
        //==========================================================================================
        MaxOutstandingEvents,
        //==========================================================================================
        /// <summary>
        /// The behavior when the user extracts a buffer that has missing packets.
        /// </summary>
        //==========================================================================================
        LostPacketMode,
        //==========================================================================================
        /// <summary>
        /// The size of the memory window of the camera in kilobytes. Should match the camera's internal buffer size.
        /// </summary>
        //==========================================================================================
        MemoryWindowSize,
        //==========================================================================================
        /// <summary>
        /// Resends issued for missing packets.
        /// </summary>
        //==========================================================================================
        ResendsEnabled,
        //==========================================================================================
        /// <summary>
        /// The threshold of the packet processing window that will trigger packets to be resent.
        /// </summary>
        //==========================================================================================
        ResendThresholdPercentage,
        //==========================================================================================
        /// <summary>
        /// The percent of the packet resend threshold that will be issued as one group past the initial threshold sent in a single request.
        /// </summary>
        //==========================================================================================
        ResendBatchingPercentage,
        //==========================================================================================
        /// <summary>
        /// The time to wait for a resend request to be satisfied before sending another.
        /// </summary>
        //==========================================================================================
        MaxResendsPerPacket,
        //==========================================================================================
        /// <summary>
        /// The time to wait for a resend request to be satisfied before sending another.
        /// </summary>
        //==========================================================================================
        ResendResponseTimeout,
        //==========================================================================================
        /// <summary>
        /// The time to wait for new packets to arrive in a partially completed image before assuming the rest of the image was lost.
        /// </summary>
        //==========================================================================================
        NewPacketTimeout,
        //==========================================================================================
        /// <summary>
        /// The time to wait for a missing packet before issuing a resend.
        /// </summary>
        //==========================================================================================
        MissingPacketTimeout,
        //==========================================================================================
        /// <summary>
        /// The resolution of the packet processing system that is used for all packet-related timeouts.
        /// </summary>
        //==========================================================================================
        ResendTimerResolution
    }
}
