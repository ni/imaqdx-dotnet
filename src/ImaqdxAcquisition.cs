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
using System.Threading;
using NationalInstruments.Restricted;
using NationalInstruments.Vision.Acquisition.Imaqdx.Internal;

namespace NationalInstruments.Vision.Acquisition.Imaqdx
{
    //==============================================================================================
    /// <summary>
    /// Defines methods for configuring a low-level acquisition.
    /// </summary>
    /// <threadsafety safety="safe"/>
    //==============================================================================================
    public sealed class ImaqdxAcquisition : IDisposable, ISupportSynchronizationContext
    {
        private const bool DefaultSynchronizeCallbacks = true;
        private const string ImageAcquiredEventKey = "ImageAcquired";

        private readonly object ImageAcquiredLockObject = new object();

        private ImaqdxSession _session;
        private CallbackManager _callbackManager = null;
        private EventHandler<ImaqdxImageAcquiredEventArgs> _frameDoneEventHandler;
        private List<EventHandler<ImaqdxImageAcquiredEventArgs>> _frameDoneEventHandlers;
        private ImaqdxFrameDoneEventHandler _frameDoneDriverCallback;
        private bool _frameDoneDriverCallbackInstalled;

        internal ImaqdxAcquisition(ImaqdxSession session)
        {
            _session = session;
            _callbackManager = new CallbackManager();
            _callbackManager.SynchronizeCallbacks = DefaultSynchronizeCallbacks;
            _frameDoneEventHandler = new EventHandler<ImaqdxImageAcquiredEventArgs>(OnImageAcquired);
            _frameDoneEventHandlers = new List<EventHandler<ImaqdxImageAcquiredEventArgs>>();
            _frameDoneDriverCallback = new ImaqdxFrameDoneEventHandler(ImageAcquiredDriverCallback);
            _frameDoneDriverCallbackInstalled = false;
        }

        //==========================================================================================
        /// <summary>
        /// Releases the resources used by <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxAcquisition" crefType="Unqualified"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxAcquisition.Dispose" crefType="Unqualified"/> method leaves the <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxAcquisition" crefType="Unqualified"/> in an unusable state. In an unusable state, you cannot call methods or properties on the object.
        /// </remarks>
        //==========================================================================================
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            lock (_session.CallbackLockObject)
            {
                if (disposing)
                {
                    if (_callbackManager != null)
                    {
                        _callbackManager.Dispose();
                        _callbackManager = null;
                    }
                }

                _frameDoneDriverCallbackInstalled = false;
            }
        }

        //==========================================================================================
        /// <summary>
        /// Gets or sets a value that indicates how events and callback delegates are invoked.
        /// </summary>
        /// <value>
        /// 	<see langword="true"/> if events and callbacks are invoked through the <see cref="System.Threading.SynchronizationContext.Send" crefType="Unqualified"/> or <see cref="System.Threading.SynchronizationContext.Post" crefType="Unqualified"/> methods; otherwise events and callbacks are invoked directly. The default value is <see langword="true"/>.
        /// </value>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public bool SynchronizeCallbacks
        {
            get
            {
                _session.ValidateSessionHandle();

                return _callbackManager.SynchronizeCallbacks;
            }
            set
            {
                _session.ValidateSessionHandle();

                _callbackManager.SynchronizeCallbacks = value;
            }
        }

        //==========================================================================================
        /// <summary>
        /// Configures a low-level acquisition.
        /// </summary>
        /// <param name="acquisitionType">
        /// Specifies whether the acquisition is <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxAcquisitionType.Continuous" crefType="Unqualified"/> or <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxAcquisitionType.SingleShot" crefType="Unqualified"/>.
        /// </param>
        /// <param name="bufferCount">
        /// For a <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxAcquisitionType.SingleShot" crefType="Unqualified"/>, this parameter specifies the number of images to acquire. For a continuous acquisition, this parameter specifies the number of buffers the driver uses internally.
        /// </param>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// 	<paramref name="bufferCount"/> is less than or equal to zero.
        /// </exception>
        //==========================================================================================
        public void Configure(ImaqdxAcquisitionType acquisitionType, int bufferCount)
        {
            _session.ValidateSessionHandle();
            if (!Enum.IsDefined(typeof(ImaqdxAcquisitionType), acquisitionType))
                throw ExceptionBuilder.InvalidEnumArgument("acquisitionType", (int)acquisitionType, typeof(ImaqdxAcquisitionType));
            if (bufferCount <= 0)
                throw ExceptionBuilder.ArgumentOutOfRange("bufferCount");

            ImaqdxSessionManager.ConfigureAcquisition(_session.Handle, acquisitionType, bufferCount);
        }

        //==========================================================================================
        /// <summary>
        /// Starts an acquisition that was previously configured with <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxAcquisition.Configure" crefType="Unqualified"/>.
        ///
        /// </summary>
        /// <remarks>
        /// Use <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxAcquisition.Stop" crefType="Unqualified"/> to stop the acquisition.
        /// </remarks>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public void Start()
        {
            _session.ValidateSessionHandle();

            ImaqdxSessionManager.StartAcquisition(_session.Handle);
        }

        //==========================================================================================
        /// <summary>
        /// Stops an acquisition previously started with <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxAcquisition.Start" crefType="Unqualified"/>.
        ///
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public void Stop()
        {
            _session.ValidateSessionHandle();

            ImaqdxSessionManager.StopAcquisition(_session.Handle);
        }

        //==========================================================================================
        /// <summary>
        /// Unconfigures an acquisition previously configured with <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxAcquisition.Configure" crefType="Unqualified"/>.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public void Unconfigure()
        {
            _session.ValidateSessionHandle();

            ImaqdxSessionManager.UnconfigureAcquisition(_session.Handle);
        }

        //==========================================================================================
        /// <summary>
        /// Occurs when the image is acquired.
        /// </summary>
        /// <remarks>
        /// This event corresponds to the IMAQdxRegisterFrameDoneEvent in the NI-IMAQdx Function Reference Help for the C API.
        /// </remarks>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public event EventHandler<ImaqdxImageAcquiredEventArgs> ImageAcquired
        {
            add
            {
                _session.ValidateSessionHandle();

                lock (ImageAcquiredLockObject)
                {
                    if (_frameDoneEventHandlers.Count == 0)
                    {
                        _frameDoneDriverCallbackInstalled = true;
                        ImaqdxEventManager.InstallImageAcquiredEventHandler(_session.Handle, _frameDoneDriverCallback);
                        _callbackManager.AddEventHandler(ImageAcquiredEventKey, _frameDoneEventHandler);
                    }
                    _frameDoneEventHandlers.Add(value);
                }
            }
            remove
            {
                _session.ValidateSessionHandle();

                lock (ImageAcquiredLockObject)
                {
                    _frameDoneEventHandlers.Remove(value);
                    if (_frameDoneEventHandlers.Count == 0)
                    {
                        _callbackManager.RemoveEventHandler(ImageAcquiredEventKey, _frameDoneEventHandler);
                        _frameDoneDriverCallbackInstalled = false;
                    }
                }
            }
        }

        private uint ImageAcquiredDriverCallback(uint session, int bufferNumber, IntPtr callbackData)
        {
            ImaqdxImageAcquiredEventArgs eventArgs = new ImaqdxImageAcquiredEventArgs((uint)bufferNumber);
            RaiseEvent<ImaqdxImageAcquiredEventArgs>(ImageAcquiredEventKey, eventArgs);
            return (uint)(_frameDoneDriverCallbackInstalled ? 1 : 0);
        }

        private void OnImageAcquired(object sender, ImaqdxImageAcquiredEventArgs e)
        {
            if (_session.Handle == null)
                return;

            lock (ImageAcquiredLockObject)
            {
                foreach (EventHandler<ImaqdxImageAcquiredEventArgs> frameDone in _frameDoneEventHandlers)
                {
                    frameDone(sender, e);
                }
            }
        }

        //==========================================================================================
        /// <summary>
        /// Acquires the last frame into the image parameter.
        /// </summary>
        /// <param name="image">
        /// The image that receives the captured pixel data.
        /// </param>
        /// <returns>
        /// The image with the captured pixel data.
        /// </returns>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <remarks>
        /// If the image type does not match the video format of the camera, this method changes the image type to a suitable format.
        /// The returned image is the same image you pass in. If <paramref name="image"/> is <see langword="null"/>, an image is created and returned.</remarks>
        //==========================================================================================
        public VisionImage GetLastImage(VisionImage image)
        {
            _session.ValidateSessionHandle();

            image = _session.CreateImage(image);
            ImaqdxSessionManager.GetImage(_session.Handle, ImaqdxBufferNumberMode.Last, image);
            return image;
        }

        //==========================================================================================
        /// <summary>
        /// Acquires the last frame into the image parameter with the specified buffer number.
        /// </summary>
        /// <param name="image">
        /// The image that receives the captured pixel data.
        /// </param>
        /// <param name="actualBufferNumber">
        /// The actual cumulative buffer number of the image retrieved.
        /// </param>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <returns>
        /// The image with the captured pixel data.
        /// </returns>
        /// <remarks>
        /// If the image type does not match the video format of the camera, this method changes the image type to a suitable format.
        /// The returned image is the same image you pass in. If <paramref name="image"/> is <see langword="null"/>, an image is created and returned.</remarks>
        //==========================================================================================
        [CLSCompliant(false)]
        public VisionImage GetLastImage(VisionImage image, out uint actualBufferNumber)
        {
            _session.ValidateSessionHandle();

            image = _session.CreateImage(image);
            actualBufferNumber = ImaqdxSessionManager.GetImage(_session.Handle, ImaqdxBufferNumberMode.Last, image);
            return image;
        }

        //==========================================================================================
        /// <summary>
        /// Acquires the next frame into the image parameter.
        /// </summary>
        /// <param name="image">
        /// The image that receives the captured pixel data.
        /// </param>
        /// <returns>
        /// The image with the captured pixel data.
        /// </returns>
        /// <remarks>
        /// If the image type does not match the video format of the camera, this method changes the image type to a suitable format.
        /// </remarks>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <remarks>
        /// If the image type does not match the video format of the camera, this method changes the image type to a suitable format.
        /// The returned image is the same image you pass in. If <paramref name="image"/> is <see langword="null"/>, an image is created and returned.</remarks>
        //==========================================================================================
        public VisionImage GetNextImage(VisionImage image)
        {
            _session.ValidateSessionHandle();

            image = _session.CreateImage(image);
            ImaqdxSessionManager.GetImage(_session.Handle, ImaqdxBufferNumberMode.Next, image);
            return image;
        }

        //==========================================================================================
        /// <summary>
        /// Acquires the next frame into the image parameter.
        /// </summary>
        /// <param name="image">
        /// The image that receives the captured pixel data.
        /// </param>
        /// <param name="actualBufferNumber">
        /// The actual cumulative buffer number of the image retrieved.
        /// </param>
        /// <returns>
        /// The image with the captured pixel data.
        /// </returns>
        /// <remarks>
        /// If the image type does not match the video format of the camera, this method changes the image type to a suitable format.
        /// </remarks>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <remarks>
        /// If the image type does not match the video format of the camera, this method changes the image type to a suitable format.
        /// The returned image is the same image you pass in. If <paramref name="image"/> is <see langword="null"/>, an image is created and returned.</remarks>
        //==========================================================================================
        [CLSCompliant(false)]
        public VisionImage GetNextImage(VisionImage image, out uint actualBufferNumber)
        {
            _session.ValidateSessionHandle();

            image = _session.CreateImage(image);
            actualBufferNumber = ImaqdxSessionManager.GetImage(_session.Handle, ImaqdxBufferNumberMode.Next, image);
            return image;
        }

        //==========================================================================================
        /// <summary>
        /// Acquires the specified frame into the image parameter.
        /// </summary>
        /// <param name="image">
        /// The image that receives the captured pixel data.
        /// </param>
        /// <param name="bufferNumber">
        /// The cumulative buffer number of the image to retrieve.
        /// </param>
        /// <returns>
        /// The image with the captured pixel data.
        /// </returns>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <remarks>
        /// If the image type does not match the video format of the camera, this method changes the image type to a suitable format.
        /// The returned image is the same image you pass in. If <paramref name="image"/> is <see langword="null"/>, an image is created and returned.</remarks>
        //==========================================================================================
        [CLSCompliant(false)]
        public VisionImage GetImageAt(VisionImage image, uint bufferNumber)
        {
            _session.ValidateSessionHandle();

            image = _session.CreateImage(image);
            ImaqdxSessionManager.GetImage(_session.Handle, bufferNumber, image);
            return image;
        }

        //==========================================================================================
        /// <summary>
        /// Acquires the specified frame into the image parameter with the specified buffer.
        /// </summary>
        /// <param name="image">
        /// The image that receives the captured pixel data.
        /// </param>
        /// <param name="bufferNumber">
        /// The cumulative buffer number of the image to retrieve.
        /// </param>
        /// <param name="actualBufferNumber">
        /// The actual cumulative buffer number of the image retrieved.
        /// </param>
        /// <returns>
        /// The image with the captured pixel data.
        /// </returns>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <remarks>
        /// If the image type does not match the video format of the camera, this method changes the image type to a suitable format.
        /// The returned image is the same image you pass in. If <paramref name="image"/> is <see langword="null"/>, an image is created and returned.</remarks>
        //==========================================================================================
        [CLSCompliant(false)]
        public VisionImage GetImageAt(VisionImage image, uint bufferNumber, out uint actualBufferNumber)
        {
            _session.ValidateSessionHandle();

            image = _session.CreateImage(image);
            actualBufferNumber = ImaqdxSessionManager.GetImage(_session.Handle, bufferNumber, image);
            return image;
        }

        //==========================================================================================
        /// <summary>
        /// Acquires the last raw image data.
        /// </summary>
        /// <param name="data">
        /// Raw data of the specified frame.
        /// </param>
        /// <returns>
        /// The image with the captured pixel data.
        /// </returns>
        /// <remarks>
        /// If the image type does not match the video format of the camera, this method changes the image type to a suitable format.
        /// The returned array is the same array you pass in. If <paramref name="data"/> is <see langword="null"/>, an array is created and returned.</remarks>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public byte[] GetLastImageData(ref byte[] data)
        {
            _session.ValidateSessionHandle();

            ImaqdxSessionManager.GetImageData(_session.Handle, ImaqdxBufferNumberMode.Last, ref data);
            return data;
        }

        //==========================================================================================
        /// <summary>
        /// Acquires the last raw image data.
        /// </summary>
        /// <param name="data">
        /// Raw data of the specified frame.
        /// </param>
        /// <param name="actualBufferNumber">
        /// The actual cumulative buffer number of the image retrieved.
        /// </param>
        /// <returns>
        /// The image with the captured pixel data.
        /// </returns>
        /// <remarks>
        ///
        /// The returned array is the same array you pass in. If <paramref name="data"/> is <see langword="null"/>, an array is created and returned.</remarks>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        [CLSCompliant(false)]
        public byte[] GetLastImageData(ref byte[] data, out uint actualBufferNumber)
        {
            _session.ValidateSessionHandle();

            actualBufferNumber = ImaqdxSessionManager.GetImageData(_session.Handle, ImaqdxBufferNumberMode.Last, ref data);
            return data;
        }

        //==========================================================================================
        /// <summary>
        /// Acquires the next frame into the image parameter.
        /// </summary>
        /// <param name="data">
        /// Raw data of the specified frame.
        /// </param>
        /// <returns>
        /// The image with the captured pixel data.
        /// </returns>
        /// <remarks>
        /// If the image type does not match the video format of the camera, this method changes the image type to a suitable format.
        /// </remarks>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <remarks>
        ///
        /// The returned array is the same array you pass in. If <paramref name="data"/> is <see langword="null"/>, an array is created and returned.</remarks>
        //==========================================================================================
        public byte[] GetNextImageData(ref byte[] data)
        {
            _session.ValidateSessionHandle();

            ImaqdxSessionManager.GetImageData(_session.Handle, ImaqdxBufferNumberMode.Next, ref data);
            return data;
        }

        //==========================================================================================
        /// <summary>
        /// Acquires the next frame into the image parameter.
        /// </summary>
        /// <param name="data">
        /// Raw data of the specified frame.
        /// </param>
        /// <param name="actualBufferNumber">
        /// The actual cumulative buffer number of the image retrieved.
        /// </param>
        /// <returns>
        /// The image with the captured pixel data.
        /// </returns>
        /// <remarks>
        ///
        /// The returned array is the same array you pass in. If <paramref name="data"/> is <see langword="null"/>, an array is created and returned.</remarks>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        [CLSCompliant(false)]
        public byte[] GetNextImageData(ref byte[] data, out uint actualBufferNumber)
        {
            _session.ValidateSessionHandle();

            actualBufferNumber = ImaqdxSessionManager.GetImageData(_session.Handle, ImaqdxBufferNumberMode.Next, ref data);
            return data;
        }

        //==========================================================================================
        /// <summary>
        /// Copies the raw data of the specified frame into the buffer.
        /// </summary>
        /// <param name="data">
        /// Raw data of the specified frame.
        /// </param>
        /// <param name="bufferNumber">
        /// The cumulative buffer number of the image to retrieve.
        /// </param>
        /// <returns>
        /// The buffer that contains the raw data for the image.
        /// </returns>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <remarks>
        /// If the image type does not match the video format of the camera, this method changes the image type to a suitable format.
        /// The returned image is the same image you pass in. If <paramref name="data"/> is <see langword="null"/>, an image is created and returned.</remarks>
        //==========================================================================================
        [CLSCompliant(false)]
        public byte[] GetImageDataAt(ref byte[] data, uint bufferNumber)
        {
            _session.ValidateSessionHandle();

            ImaqdxSessionManager.GetImageData(_session.Handle, bufferNumber, ref data);
            return data;
        }

        //==========================================================================================
        /// <summary>
        /// Copies the raw data of the specified frame into the buffer.
        /// </summary>
        /// <param name="data">
        /// Raw data of the specified frame.
        /// </param>
        /// <param name="bufferNumber">
        /// The cumulative buffer number of the image to retrieve.
        /// </param>
        /// <param name="actualBufferNumber">
        /// The actual cumulative buffer number of the image retrieved.
        /// </param>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <returns>
        /// The buffer that contains the raw data for the image.
        /// </returns>
        /// <remarks>
        /// If the image type does not match the video format of the camera, this method changes the image type to a suitable format.
        /// The returned image is the same image you pass in. If <paramref name="data"/> is <see langword="null"/>, an image is created and returned.</remarks>
        //==========================================================================================
        [CLSCompliant(false)]
        public byte[] GetImageDataAt(ref byte[] data, uint bufferNumber, out uint actualBufferNumber)
        {
            _session.ValidateSessionHandle();

            actualBufferNumber = ImaqdxSessionManager.GetImageData(_session.Handle, bufferNumber, ref data);
            return data;
        }

        private void RaiseEvent<TEventArgs>(string eventKey, TEventArgs eventArgs) where TEventArgs : EventArgs
        {
            try
            {
                lock (_session.CallbackLockObject)
                {
                    if (_session.Handle == null)
                        return;

                    if (_callbackManager.GetHandlerCount(eventKey) == 0)
                        return;

                    if (_callbackManager.SynchronizeCallbacks)
                    {
                        _callbackManager.RaiseGenericEventAsync<TEventArgs>(eventKey, this, eventArgs);
                    }
                    else
                    {
                        _callbackManager.RaiseGenericEvent<TEventArgs>(eventKey, this, eventArgs);
                    }
                }
            }
            catch (NullReferenceException)
            {
                throw ExceptionBuilder.SessionDisposed();
            }
        }
    }
}
