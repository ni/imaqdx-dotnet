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
using System.Text;
using NationalInstruments.Vision.Acquisition.Imaqdx.Internal;

namespace NationalInstruments.Vision.Acquisition.Imaqdx
{
    //==============================================================================================
    /// <summary>
    /// Provides properties and methods that open a session to an ethernet camera.
    /// </summary>
    /// <threadsafety safety="safe"/>
    //==============================================================================================
    public sealed class ImaqdxEthernetSession : ImaqdxSession
    {
        //==========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxEthernetSession" crefType="Unqualified"/> class with the specified camera.
        /// </summary>
        /// <param name="cameraName">
        /// The name of the camera.
        /// </param>
        //==========================================================================================
        public ImaqdxEthernetSession(string cameraName)
            : base(cameraName)
        { }

        //==========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxEthernetSession" crefType="Unqualified"/> class with the specified camera and mode.
        /// </summary>
        /// <param name="cameraName">
        /// The name of the camera.
        /// </param>
        /// <param name="mode">
        /// The control mode of the camera used during image broadcasting.
        /// </param>
        /// <remarks>
        /// Open a camera in <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxCameraControlMode.Controller" crefType="Unqualified"/> mode to actively configure and acquire image data. Open a camera in <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxCameraControlMode.Listener" crefType="Unqualified"/> mode to passively acquire image data from a session that was opened in <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxCameraControlMode.Controller" crefType="Unqualified"/> mode on a different host or target computer. The default value is <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxCameraControlMode.Controller" crefType="Unqualified"/>.
        /// </remarks>
        //==========================================================================================
        public ImaqdxEthernetSession(string cameraName, ImaqdxCameraControlMode mode)
            : base(cameraName, mode)
        { }

        //==========================================================================================
        /// <summary>
        /// Reset Ethernet camera address.
        /// </summary>
        /// <param name="name">
        /// The name of the camera you want to open. <paramref name="name"/> must match the configuration file name you used to configure the camera in MAX. You can also open a camera using its 64-bit serial number (<format type="monospace">uuid:XXXXXXXXXXXXXXXX</format>), where the number following uuid must be a 64-bit hexadecimal number representing the internal serial number of the camera.
        /// <note type="note">
        /// Specify <format type="monospace">uuid:serial number in hexadecimal representation</format> for the camera name when opening in listening mode. The serial number must match the serial number used in MAX.
        /// </note>
        /// </param>
        /// <param name="address">
        /// Network address for the camera.
        /// </param>
        /// <param name="subnet">
        /// Subnet mask for the camera.
        /// </param>
        /// <param name="gateway">
        /// Gateway for the camera.
        /// </param>
        /// <param name="timeout">
        /// Time, in milliseconds, allowed for the Ethernet camera to reset its network address. The default timeout is 1000 ms.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// 	<paramref name="name"/> is <see langword="null"/>.
        /// <para>
        /// -or-
        /// </para>
        /// 	<paramref name="address"/> is <see langword="null"/>.
        /// <para>
        /// -or-
        /// </para>
        /// 	<paramref name="subnet"/> is <see langword="null"/>.
        /// <para>
        /// -or-
        /// </para>
        /// 	<paramref name="gateway"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// 	<paramref name="timeout"/> is less than zero.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// 	<paramref name="name"/> is an empty string.
        /// </exception>
        //==========================================================================================
        public static void ResetEthernetCameraAddress(string name, IPAddress address, IPAddress subnet, IPAddress gateway, int timeout)
        {
            if (name == null)
                throw ExceptionBuilder.ArgumentNull("name");
            if (name.Length == 0)
                throw ExceptionBuilder.EmptyString("name");
            if (address == null)
                throw ExceptionBuilder.ArgumentNull("address");
            if (subnet == null)
                throw ExceptionBuilder.ArgumentNull("subnet");
            if (gateway == null)
                throw ExceptionBuilder.ArgumentNull("gateway");
            if (timeout < 0)
                throw ExceptionBuilder.ArgumentOutOfRange("timeout");

            ImaqdxSessionManager.ResetEthernetCameraAddress(name, address, subnet, gateway, timeout);
        }

        //==========================================================================================
        /// <summary>
        /// Detects Ethernet cameras on a network.
        /// </summary>
        /// <param name="address">
        /// Specifies the destination address for the discovery command. The default address is 255.255.255.255.
        /// </param>
        /// <param name="timeout">
        /// Specifies the time, in milliseconds, allowed for the Ethernet camera discovery to complete. The default timeout is 1000 ms.
        /// </param>
        /// <remarks>
        /// Use this method to detect Ethernet cameras on a network with a remote subnet. During discovery, this method is blocked and returns after the specified timeout. The address specifies the destination address for the discovery command. The default address is 255.255.255.255.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">
        /// 	<paramref name="address"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// 	<paramref name="timeout"/> is less than zero.
        /// </exception>
        //==========================================================================================
        public static void DiscoverEthernetCameras(IPAddress address, int timeout)
        {
            if (address == null)
                throw ExceptionBuilder.ArgumentNull("address");
            if (timeout < 0)
                throw ExceptionBuilder.ArgumentOutOfRange("timeout");

            ImaqdxSessionManager.DiscoverEthernetCameras(address, timeout);
        }
    }
}
