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
using System.Threading;
using NationalInstruments.Restricted;
using NationalInstruments.Vision.Acquisition.Imaqdx.Internal;

namespace NationalInstruments.Vision.Acquisition.Imaqdx
{
    //==============================================================================================
    /// <summary>
    /// Provides properties and methods that open a session to a camera.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <threadsafety safety="safe"/>
    /// <exception cref="System.ObjectDisposedException">
    /// The session has been disposed.
    /// </exception>
    //==============================================================================================
    public class ImaqdxSession : IDisposable, ISupportSynchronizationContext
    {
        private const bool DefaultSynchronizeCallbacks = true;
        private const bool DefaultWaitForPendingCallbacks = true;
        private const bool DefaultProcessMessages = true;
        private const ImaqdxCameraControlMode DefaultCameraControlMode = ImaqdxCameraControlMode.Controller;
        private const string SnapCompletedEventKey = "SnapCompleted";
        private const string GrabCompletedEventKey = "GrabCompleted";
        private const string SequenceCompletedEventKey = "SequenceCompleted";
        private const string CameraAttachedEventKey = "CameraAttached";
        private const string CameraDetachedEventKey = "CameraDetached";
        private const string BusResetCompletedEventKey = "BusResetCompleted";
        private const string ImageTypeAttributeName = "AcquisitionAttributes::TypeOfImageInUse";

        internal readonly object CallbackLockObject = new object();
        private readonly object AttributesLockObject = new object();
        private readonly object SnapCompletedLockObject = new object();
        private readonly object GrabCompletedLockObject = new object();
        private readonly object SequenceCompletedLockObject = new object();
        private readonly object CameraAttachedLockObject = new object();
        private readonly object CameraDetachedLockObject = new object();
        private readonly object BusResetCompletedLockObject = new object();

        private string _cameraName;
        private ImaqdxSessionHandle _sessionHandle = null;
        private ImaqdxAttributeCollection _attributes = null;
        private ImaqdxAcquisition _acquisition = null;
        private CallbackManager _callbackManager = null;
        private EventHandler<ImaqdxSnapEventArgs> _snapCompletedEventHandler;
        private EventHandler<ImaqdxGrabEventArgs> _grabCompletedEventHandler;
        private EventHandler<ImaqdxSequenceEventArgs> _sequenceCompletedEventHandler;
        private EventHandler<EventArgs> _cameraAttachedEventHandler;
        private EventHandler<EventArgs> _cameraDetachedEventHandler;
        private EventHandler<EventArgs> _busResetCompletedEventHandler;
        private List<EventHandler<ImaqdxSnapEventArgs>> _snapCompletedEventHandlers;
        private List<EventHandler<ImaqdxGrabEventArgs>> _grabCompletedEventHandlers;
        private List<EventHandler<ImaqdxSequenceEventArgs>> _sequenceCompletedEventHandlers;
        private List<EventHandler<EventArgs>> _cameraAttachedEventHandlers;
        private List<EventHandler<EventArgs>> _cameraDetachedEventHandlers;
        private List<EventHandler<EventArgs>> _busResetCompletedEventHandlers;
        private ImaqdxPnpEventHandler _cameraAttachedDriverCallback;
        private ImaqdxPnpEventHandler _cameraDetachedDriverCallback;
        private ImaqdxPnpEventHandler _busResetCompletedDriverCallback;
        private bool _cameraAttachedDriverCallbackInstalled;
        private bool _cameraDetachedDriverCallbackInstalled;
        private bool _busResetCompletedDriverCallbackInstalled;

        //==========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession" crefType="Unqualified"/> class with the specified camera.
        /// </summary>
        /// <param name="cameraName">
        /// The name of the camera.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// 	<paramref name="cameraName"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// 	<paramref name="cameraName"/> is an empty string.
        /// </exception>
        //==========================================================================================
        public ImaqdxSession(string cameraName)
            : this(cameraName, DefaultCameraControlMode)
        { }

        //==========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession" crefType="Unqualified"/> class with the specified camera and mode.
        /// </summary>
        /// <param name="cameraName">
        /// The name of the camera.
        /// </param>
        /// <param name="mode">
        /// 	<see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxCameraControlMode" crefType="Unqualified"/> of the camera.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// 	<paramref name="cameraName"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// 	<paramref name="cameraName"/> is an empty string.
        /// </exception>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">
        /// 	<paramref name="mode"/> is invalid.
        /// </exception>
        //==========================================================================================
        public ImaqdxSession(string cameraName, ImaqdxCameraControlMode mode)
        {
            if (cameraName == null)
                throw ExceptionBuilder.ArgumentNull("cameraName");
            if (cameraName.Length == 0)
                throw ExceptionBuilder.EmptyString("cameraName");
            if (!Enum.IsDefined(typeof(ImaqdxCameraControlMode), mode))
                throw ExceptionBuilder.InvalidEnumArgument("mode", (int)mode, typeof(ImaqdxCameraControlMode));

            _cameraName = cameraName;
            _sessionHandle = ImaqdxSessionManager.Open(cameraName, mode);
            _callbackManager = new CallbackManager();
            _callbackManager.SynchronizeCallbacks = DefaultSynchronizeCallbacks;
            _snapCompletedEventHandler = new EventHandler<ImaqdxSnapEventArgs>(OnSnapCompleted);
            _grabCompletedEventHandler = new EventHandler<ImaqdxGrabEventArgs>(OnGrabCompleted);
            _sequenceCompletedEventHandler = new EventHandler<ImaqdxSequenceEventArgs>(OnSequenceCompleted);
            _cameraAttachedEventHandler = new EventHandler<EventArgs>(OnCameraAttached);
            _cameraDetachedEventHandler = new EventHandler<EventArgs>(OnCameraDetached);
            _busResetCompletedEventHandler = new EventHandler<EventArgs>(OnBusResetCompleted);
            _snapCompletedEventHandlers = new List<EventHandler<ImaqdxSnapEventArgs>>();
            _grabCompletedEventHandlers = new List<EventHandler<ImaqdxGrabEventArgs>>();
            _sequenceCompletedEventHandlers = new List<EventHandler<ImaqdxSequenceEventArgs>>();
            _cameraAttachedEventHandlers = new List<EventHandler<EventArgs>>();
            _cameraDetachedEventHandlers = new List<EventHandler<EventArgs>>();
            _busResetCompletedEventHandlers = new List<EventHandler<EventArgs>>();
            _cameraAttachedDriverCallback = new ImaqdxPnpEventHandler(CameraAttachedDriverCallback);
            _cameraDetachedDriverCallback = new ImaqdxPnpEventHandler(CameraDetachedDriverCallback);
            _busResetCompletedDriverCallback = new ImaqdxPnpEventHandler(BusResetCompletedDriverCallback);
            _cameraAttachedDriverCallbackInstalled = false;
            _cameraDetachedDriverCallbackInstalled = false;
            _busResetCompletedDriverCallbackInstalled = false;

            _acquisition = new ImaqdxAcquisition(this);
        }

        ~ImaqdxSession()
        {
            Dispose(false);
        }

        //==========================================================================================
        /// <summary>
        /// Releases the resources used by <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession" crefType="Unqualified"/>.
        /// </summary>
        /// <remarks>
        /// Call <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession.Dispose" crefType="Unqualified"/> when you are finished using <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession" crefType="Unqualified"/>. The <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession.Dispose" crefType="Unqualified"/> method leaves <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession" crefType="Unqualified"/> in an unusable state. In an unusable state, you cannot call methods or properties on the object.
        /// </remarks>
        //==========================================================================================
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            lock (CallbackLockObject)
            {
                CleanUp(disposing);
            }
        }

        //==========================================================================================
        /// <summary>
        /// Stops an acquisition in progress, releases resources associated with an acquisition, and closes the specified session.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public void Close()
        {
            ValidateSessionHandle();

            Close(DefaultWaitForPendingCallbacks, DefaultProcessMessages);
        }

        //==========================================================================================
        /// <summary>
        /// </summary>
        /// <param name="waitForPendingCallbacks">
        /// </param>
        //==========================================================================================
        public void Close(bool waitForPendingCallbacks)
        {
            ValidateSessionHandle();

            Close(waitForPendingCallbacks, DefaultProcessMessages);
        }

        //==========================================================================================
        /// <summary>
        /// </summary>
        /// <param name="waitForPendingCallbacks">
        /// </param>
        /// <param name="processMessages">
        /// </param>
        //==========================================================================================
        public void Close(bool waitForPendingCallbacks, bool processMessages)
        {
            ValidateSessionHandle();

            if (waitForPendingCallbacks)
            {
                while (!Monitor.TryEnter(CallbackLockObject))
                {
                    if (processMessages)
                    {
                        System.Windows.Forms.Application.DoEvents();
                    }
                }

                lock (CallbackLockObject)
                {
                    CleanUp(true);
                }
            }
            else
            {
                CleanUp(true);
            }
        }

        private void CleanUp(bool disposing)
        {
            if (disposing)
            {
                // Free managed resources
                if (_acquisition != null)
                {
                    _acquisition.Dispose();
                    _acquisition = null;
                }
                if (_callbackManager != null)
                {
                    _callbackManager.Dispose();
                    _callbackManager = null;
                }
            }

            // Free unmanaged resources
            if (_sessionHandle != null)
            {
                ImaqdxSessionManager.Close(_sessionHandle);
                _sessionHandle.Dispose();
                _sessionHandle = null;
            }

            _cameraAttachedDriverCallbackInstalled = false;
            _cameraDetachedDriverCallbackInstalled = false;
            _busResetCompletedDriverCallbackInstalled = false;
        }

        internal ImaqdxSessionHandle Handle
        {
            get { return _sessionHandle; }
        }

        internal ImageType ImageType
        {
            get
            {
                uint imageTypeValue;
                ImaqdxAttributeManager.GetAttribute(_sessionHandle, ImageTypeAttributeName, out imageTypeValue);
                return (ImageType)imageTypeValue;
            }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the name of the camera.
        /// </summary>
        /// <value>
        /// The camera name.
        /// </value>
        //==========================================================================================
        public string CameraName
        {
            get
            {
                return _cameraName;
            }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the methods needed to perform a low-level acquisition.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public ImaqdxAcquisition Acquisition
        {
            get
            {
                ValidateSessionHandle();

                return _acquisition;
            }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the collection of attributes for the camera.
        /// </summary>
        /// <value>
        /// The collection of attributes for the camera.
        /// </value>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public ImaqdxAttributeCollection Attributes
        {
            get
            {
                ValidateSessionHandle();

                lock (AttributesLockObject)
                {
                    if (_attributes == null)
                        _attributes = new ImaqdxAttributeCollection(this);

                    return _attributes;
                }
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
                ValidateSessionHandle();

                return _callbackManager.SynchronizeCallbacks;
            }
            set
            {
                ValidateSessionHandle();

                _callbackManager.SynchronizeCallbacks = value;
            }
        }

        //==========================================================================================
        /// <summary>
        /// Configures, starts, acquires, stops, and unconfigures a snap acquisition.
        /// </summary>
        /// <param name="image">
        /// The image that receives the captured pixel data.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <returns>
        /// The image that contains the captured pixel data.
        /// </returns>
        /// <remarks>
        /// Use a snap for low-speed or single-capture applications where ease of programming is essential. If you call this method before calling <see cref="M:NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession.#ctor" crefType="Unqualified"/>, <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession.Snap" crefType="Unqualified"/> uses <format type="monospace">cam0</format>  by default.
        /// If the image type does not match the video format of the camera, this method changes the image type to a suitable format.
        /// The returned image is the same image you pass in. If <paramref name="image"/> is <see langword="null"/>, an image is created and returned.</remarks>
        //==========================================================================================
        public VisionImage Snap(VisionImage image)
        {
            ValidateSessionHandle();

            image = CreateImage(image);
            ImaqdxSessionManager.Snap(_sessionHandle, image);
            return image;
        }

        //==========================================================================================
        /// <param name="userState">
        /// An object used to associate client state, such as a task ID, with this particular asynchronous operation.
        /// </param>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <summary>
        /// Configures, starts, acquires, stops, and unconfigures an asynchronous snap acquisition.
        /// </summary>
        /// <param name="image">
        /// The image that receives the captured pixel data.
        /// </param>
        /// <returns>
        /// The image that contains the captured pixel data.
        /// </returns>
        /// <remarks>
        /// Use a snap for low-speed or single-capture applications where ease of programming is essential. If you call this method before calling <see cref="M:NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession.#ctor" crefType="Unqualified"/>, <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession.Snap" crefType="Unqualified"/> uses <format type="monospace">cam0</format>  by default.
        /// If the image type does not match the video format of the camera, this method changes the image type to a suitable format.
        /// The returned image is the same image you pass in. If <paramref name="image"/> is <see langword="null"/>, an image is created and returned.
        /// Use <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession.SnapCompleted" crefType="Unqualified"/> for notification when this operation is done.
        /// </remarks>
        //==========================================================================================
        public void SnapAsync(VisionImage image, object userState)
        {
            ValidateSessionHandle();

            image = CreateImage(image);
            ImaqdxSnapAsyncState state = new ImaqdxSnapAsyncState(image, userState);
            ThreadPool.QueueUserWorkItem(new WaitCallback(SnapAsyncWorkItem), state);
        }

        private void SnapAsyncWorkItem(object data)
        {
            Exception error = null;
            ImaqdxSnapAsyncState state = (ImaqdxSnapAsyncState)data;

            try
            {
                ImaqdxSessionManager.Snap(_sessionHandle, state.Image);
            }
            catch (Exception e)
            {
                error = e;
            }
            finally
            {
                ImaqdxSnapEventArgs eventArgs = new ImaqdxSnapEventArgs(error, false, state.UserState, state.Image);
                RaiseEvent<ImaqdxSnapEventArgs>(SnapCompletedEventKey, eventArgs);
            }
        }

        private void OnSnapCompleted(object sender, ImaqdxSnapEventArgs e)
        {
            if (_sessionHandle == null)
                return;

            lock (SnapCompletedLockObject)
            {
                foreach (EventHandler<ImaqdxSnapEventArgs> snapCompleted in _snapCompletedEventHandlers)
                {
                    snapCompleted(sender, e);
                }
            }
        }

        //==========================================================================================
        /// <summary>
        /// </summary>
        /// <param name="eventArgs">
        /// </param>
        //==========================================================================================
        protected virtual void OnSnapCompleted(ImaqdxSnapEventArgs eventArgs)
        {
            ValidateSessionHandle();

            RaiseEvent<ImaqdxSnapEventArgs>(SnapCompletedEventKey, eventArgs);
        }

        //==========================================================================================
        /// <summary>
        /// Occurs when a <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession.SnapAsync" crefType="Unqualified"/> is completed.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public event EventHandler<ImaqdxSnapEventArgs> SnapCompleted
        {
            add
            {
                ValidateSessionHandle();

                lock (SnapCompletedLockObject)
                {
                    if (_snapCompletedEventHandlers.Count == 0)
                    {
                        _callbackManager.AddEventHandler(SnapCompletedEventKey, _snapCompletedEventHandler);
                    }
                    _snapCompletedEventHandlers.Add(value);
                }
            }
            remove
            {
                ValidateSessionHandle();

                lock (SnapCompletedLockObject)
                {
                    _snapCompletedEventHandlers.Remove(value);
                    if (_snapCompletedEventHandlers.Count == 0)
                    {
                        _callbackManager.RemoveEventHandler(SnapCompletedEventKey, _snapCompletedEventHandler);
                    }
                }
            }
        }

        //==========================================================================================
        /// <summary>
        /// Configures and starts an acquisition. A grab performs an acquisition that loops continually on a ring of buffers.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <remarks>
        /// Use a grab for high-speed image acquisition.
        /// Use <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession.Grab" crefType="Unqualified"/>  to copy an image out of the buffer. If you call this method before calling <see cref="M:NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession.#ctor" crefType="Unqualified"/>,  <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession.ConfigureGrab" crefType="Unqualified"/> uses <format type="monospace">cam0</format>  by default. Use <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxAcquisition.Unconfigure" crefType="Unqualified"/> to unconfigure the acquisition.
        /// </remarks>
        //==========================================================================================
        public void ConfigureGrab()
        {
            ValidateSessionHandle();

            ImaqdxSessionManager.ConfigureGrab(_sessionHandle);
        }

        //==========================================================================================
        /// <summary>
        /// Acquires the most current frame into the specified image.
        /// </summary>
        /// <param name="image">
        /// The image that receives the captured pixel data.
        /// </param>
        /// <param name="waitForNextBuffer">
        /// If the <paramref name="waitForNextBuffer"/> value is <see langword="true"/>, the driver will wait for the next available buffer. If the <paramref name="waitForNextBuffer"/> value is <see langword="false"/>, the driver will not wait for the next available buffer, and will instead return the last acquired buffer.
        /// </param>
        /// <returns>
        /// The image that contains the captured pixel data.
        /// </returns>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <remarks>
        /// Call this method only after calling <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession.ConfigureGrab" crefType="Unqualified"/>. If the image type does not match the video format of the camera, this method changes the image type to a suitable format.
        /// The returned image is the same image you pass in. If <paramref name="image"/> is <see langword="null"/>, an image is created and returned.</remarks>
        //==========================================================================================
        public VisionImage Grab(VisionImage image, bool waitForNextBuffer)
        {
            ValidateSessionHandle();

            image = CreateImage(image);
            ImaqdxSessionManager.Grab(_sessionHandle, image, waitForNextBuffer);
            return image;
        }

        //==========================================================================================
        /// <summary>
        /// Acquires the most current frame into the specified image with the specified buffers.
        /// </summary>
        /// <param name="image">
        /// The image that receives the captured pixel data.
        /// </param>
        /// <param name="waitForNextBuffer">
        /// If the <paramref name="waitForNextBuffer"/> value is <see langword="true"/>, the driver will wait for the next available buffer. If the <paramref name="waitForNextBuffer"/> value is <see langword="false"/>, the driver will not wait for the next available buffer, and will instead return the last acquired buffer.
        /// </param>
        /// <param name="actualBufferNumber">
        /// The actual cumulative buffer number of the image retrieved.
        /// </param>
        /// <returns>
        /// The image that contains the captured pixel data.
        /// </returns>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <remarks>
        /// Call this method only after calling <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession.ConfigureGrab" crefType="Unqualified"/>. If the image type does not match the video format of the camera, this method changes the image type to a suitable format.
        /// The returned image is the same image you pass in. If <paramref name="image"/> is <see langword="null"/>, an image is created and returned.</remarks>
        //==========================================================================================
        [CLSCompliant(false)]
        public VisionImage Grab(VisionImage image, bool waitForNextBuffer, out uint actualBufferNumber)
        {
            ValidateSessionHandle();

            image = CreateImage(image);
            actualBufferNumber = ImaqdxSessionManager.Grab(_sessionHandle, image, waitForNextBuffer);
            return image;
        }

        //==========================================================================================
        /// <summary>
        /// Acquires the most current frame into the specified image asynchronously.
        /// </summary>
        /// <param name="image">
        /// The image that receives the captured pixel data.
        /// </param>
        /// <param name="waitForNextBuffer">
        /// If the <paramref name="waitForNextBuffer"/> value is <see langword="true"/>, the driver will wait for the next available buffer. If the <paramref name="waitForNextBuffer"/> value is <see langword="false"/>, the driver will not wait for the next available buffer, and will instead return the last acquired buffer.
        /// </param>
        /// <param name="userState">
        /// An object used to associate client state, such as a task ID, with this particular asynchronous operation.
        /// </param>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <returns>
        /// The image that contains the captured pixel data.
        /// </returns>
        /// <remarks>
        /// Use <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession.GrabCompleted" crefType="Unqualified"/> for notification when the operation is done.
        /// Call this method only after calling <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession.ConfigureGrab" crefType="Unqualified"/>. If the image type does not match the video format of the camera, this method changes the image type to a suitable format.
        /// The returned image is the same image you pass in. If <paramref name="image"/> is <see langword="null"/>, an image is created and returned.</remarks>
        //==========================================================================================
        public void GrabAsync(VisionImage image, bool waitForNextBuffer, object userState)
        {
            ValidateSessionHandle();

            image = CreateImage(image);
            ImaqdxGrabAsyncState state = new ImaqdxGrabAsyncState(image, waitForNextBuffer, userState);
            ThreadPool.QueueUserWorkItem(new WaitCallback(GrabAsyncWorkItem), state);
        }

        private void GrabAsyncWorkItem(object data)
        {
            Exception error = null;
            uint actualBufferNumber = 0;
            ImaqdxGrabAsyncState state = (ImaqdxGrabAsyncState)data;

            try
            {
                actualBufferNumber = ImaqdxSessionManager.Grab(_sessionHandle, state.Image, state.WaitForNextBuffer);
            }
            catch (Exception e)
            {
                error = e;
            }
            finally
            {
                ImaqdxGrabEventArgs eventArgs = new ImaqdxGrabEventArgs(error, false, state.UserState, actualBufferNumber, state.Image);
                RaiseEvent<ImaqdxGrabEventArgs>(GrabCompletedEventKey, eventArgs);
            }
        }

        private void OnGrabCompleted(object sender, ImaqdxGrabEventArgs e)
        {
            if (_sessionHandle == null)
                return;

            lock (GrabCompletedLockObject)
            {
                foreach (EventHandler<ImaqdxGrabEventArgs> grabCompleted in _grabCompletedEventHandlers)
                {
                    grabCompleted(sender, e);
                }
            }
        }

        protected virtual void OnGrabCompleted(ImaqdxGrabEventArgs eventArgs)
        {
            ValidateSessionHandle();

            RaiseEvent<ImaqdxGrabEventArgs>(GrabCompletedEventKey, eventArgs);
        }

        //==========================================================================================
        /// <summary>
        /// Occurs when a <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession.GrabAsync" crefType="Unqualified"/> is completed.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public event EventHandler<ImaqdxGrabEventArgs> GrabCompleted
        {
            add
            {
                ValidateSessionHandle();

                lock (GrabCompletedLockObject)
                {
                    if (_grabCompletedEventHandlers.Count == 0)
                    {
                        _callbackManager.AddEventHandler(GrabCompletedEventKey, _grabCompletedEventHandler);
                    }
                    _grabCompletedEventHandlers.Add(value);
                }
            }
            remove
            {
                ValidateSessionHandle();

                lock (GrabCompletedLockObject)
                {
                    _grabCompletedEventHandlers.Remove(value);
                    if (_grabCompletedEventHandlers.Count == 0)
                    {
                        _callbackManager.RemoveEventHandler(GrabCompletedEventKey, _grabCompletedEventHandler);
                    }
                }
            }
        }

        //==========================================================================================
        /// <summary>
        /// Configures, starts, acquires, stops, and unconfigures a sequence acquisition.
        /// </summary>
        /// <param name="images">
        /// The image array that receives the captured pixel data.
        /// </param>
        /// <param name="numberOfImages">
        /// The number of images in the image array. This value must be less than or equal to the number of allocated images in the image array.
        /// </param>
        /// <returns>
        /// The image array that contains the captured pixel data.
        /// </returns>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// 	<paramref name="numberOfImages"/> is less than or equal to 0.
        /// </exception>
        /// <remarks>
        /// Use this method to capture multiple images. If you call this method before calling <see cref="M:NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession.#ctor" crefType="Unqualified"/>, <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession.Sequence" crefType="Unqualified"/> uses <format type="monospace">cam0</format>  by default.
        /// If the image type does not match the video format of the camera, this method changes the image type to a suitable format.
        /// The returned image is the same image you pass in. If <paramref name="image"/> is <see langword="null"/>, an image is created and returned. If the length of the images array you pass in is different than <paramref name="numberOfImages"/>, an array the size of <paramref name="numberOfImages"/> is created.
        /// </remarks>
        //==========================================================================================
        public VisionImage[] Sequence(VisionImage[] images, int numberOfImages)
        {
            ValidateSessionHandle();
            if (numberOfImages <= 0)
                throw ExceptionBuilder.ArgumentOutOfRange("numberOfImages");

            images = CreateImages(images, numberOfImages);
            ImaqdxSessionManager.Sequence(_sessionHandle, images);
            return images;
        }

        //==========================================================================================
        /// <summary>
        /// Configures, starts, acquires, stops, and unconfigures a sequence acquisition asynchronously.
        /// </summary>
        /// <param name="images">
        /// The image array that receives the captured pixel data.
        /// </param>
        /// <param name="userState">
        /// An object used to associate client state, such as a task ID, with this particular asynchronous operation.
        /// </param>
        /// <param name="numberOfImages">
        /// The number of images in the image array. This value must be less than or equal to the number of allocated images in the image array.
        /// </param>
        /// <returns>
        /// The image array that contains the captured pixel data.
        /// </returns>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// 	<paramref name="numberOfImages"/> is less than or equal to 0.
        /// </exception>
        /// <remarks>
        /// Use this method to capture multiple images. If you call this method before calling <see cref="M:NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession.#ctor" crefType="Unqualified"/>, <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession.Sequence" crefType="Unqualified"/> uses <format type="monospace">cam0</format>  by default.
        /// If the image type does not match the video format of the camera, this method changes the image type to a suitable format.
        /// The returned image is the same image you pass in. If <paramref name="image"/> is <see langword="null"/>, an image is created and returned. If the length of the images array you pass in is different than <paramref name="numberOfImages"/>, an array the size of <paramref name="numberOfImages"/> is created. Use <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession.SequenceCompleted" crefType="Unqualified"/> for notification when this operation is done.
        /// </remarks>
        //==========================================================================================
        public void SequenceAsync(VisionImage[] images, int numberOfImages, object userState)
        {
            ValidateSessionHandle();
            if (numberOfImages <= 0)
                throw ExceptionBuilder.ArgumentOutOfRange("numberOfImages");

            images = CreateImages(images, numberOfImages);
            ImaqdxSequenceAsyncState state = new ImaqdxSequenceAsyncState(images, userState);
            ThreadPool.QueueUserWorkItem(new WaitCallback(SequenceAsyncWorkItem), state);
        }

        private void SequenceAsyncWorkItem(object data)
        {
            Exception error = null;
            ImaqdxSequenceAsyncState state = (ImaqdxSequenceAsyncState)data;

            try
            {
                ImaqdxSessionManager.Sequence(_sessionHandle, state.Images);
            }
            catch (Exception e)
            {
                error = e;
            }
            finally
            {
                ImaqdxSequenceEventArgs eventArgs = new ImaqdxSequenceEventArgs(error, false, state.UserState, state.Images);
                RaiseEvent<ImaqdxSequenceEventArgs>(SequenceCompletedEventKey, eventArgs);
            }
        }

        private void OnSequenceCompleted(object sender, ImaqdxSequenceEventArgs e)
        {
            if (_sessionHandle == null)
                return;

            lock (SequenceCompletedLockObject)
            {
                foreach (EventHandler<ImaqdxSequenceEventArgs> sequenceCompleted in _sequenceCompletedEventHandlers)
                {
                    sequenceCompleted(sender, e);
                }
            }
        }

        //==========================================================================================
        /// <summary>
        /// </summary>
        /// <param name="eventArgs">
        /// </param>
        //==========================================================================================
        protected virtual void OnSequenceCompleted(ImaqdxSequenceEventArgs eventArgs)
        {
            ValidateSessionHandle();

            RaiseEvent<ImaqdxSequenceEventArgs>(SequenceCompletedEventKey, eventArgs);
        }

        //==========================================================================================
        /// <summary>
        /// Occurs when a <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxSession.SequenceAsync" crefType="Unqualified"/> is completed.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public event EventHandler<ImaqdxSequenceEventArgs> SequenceCompleted
        {
            add
            {
                ValidateSessionHandle();

                lock (SequenceCompletedLockObject)
                {
                    if (_sequenceCompletedEventHandlers.Count == 0)
                    {
                        _callbackManager.AddEventHandler(SequenceCompletedEventKey, _sequenceCompletedEventHandler);
                    }
                    _sequenceCompletedEventHandlers.Add(value);
                }
            }
            remove
            {
                ValidateSessionHandle();

                lock (SequenceCompletedLockObject)
                {
                    _sequenceCompletedEventHandlers.Remove(value);
                    if (_sequenceCompletedEventHandlers.Count == 0)
                    {
                        _callbackManager.RemoveEventHandler(SequenceCompletedEventKey, _sequenceCompletedEventHandler);
                    }
                }
            }
        }

        //==========================================================================================
        /// <summary>
        /// Occurs when a new camera is attached.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public event EventHandler<EventArgs> CameraAttached
        {
            add
            {
                ValidateSessionHandle();

                lock (CameraAttachedLockObject)
                {
                    if (_cameraAttachedEventHandlers.Count == 0)
                    {
                        _cameraAttachedDriverCallbackInstalled = true;
                        ImaqdxEventManager.InstallPnpEventHandler(_sessionHandle, ImaqdxPnpEvent.CameraAttached, _cameraAttachedDriverCallback);
                        _callbackManager.AddEventHandler(CameraAttachedEventKey, _cameraAttachedEventHandler);
                    }
                    _cameraAttachedEventHandlers.Add(value);
                }
            }
            remove
            {
                ValidateSessionHandle();

                lock (CameraAttachedLockObject)
                {
                    _cameraAttachedEventHandlers.Remove(value);
                    if (_cameraAttachedEventHandlers.Count == 0)
                    {
                        _callbackManager.RemoveEventHandler(CameraAttachedEventKey, _cameraAttachedEventHandler);
                        _cameraAttachedDriverCallbackInstalled = false;
                    }
                }
            }
        }

        private uint CameraAttachedDriverCallback(uint session, ImaqdxPnpEvent pnpEvent, IntPtr callbackData)
        {
            RaiseEvent<EventArgs>(CameraAttachedEventKey, EventArgs.Empty);
            return (uint)(_cameraAttachedDriverCallbackInstalled ? 1 : 0);
        }

        private void OnCameraAttached(object sender, EventArgs e)
        {
            if (_sessionHandle == null)
                return;

            lock (CameraAttachedLockObject)
            {
                foreach (EventHandler<EventArgs> cameraAttached in _cameraAttachedEventHandlers)
                {
                    cameraAttached(sender, e);
                }
            }
        }

        //==========================================================================================
        /// <summary>
        /// </summary>
        /// <param name="eventArgs">
        /// </param>
        //==========================================================================================
        protected virtual void OnCameraAttached(EventArgs eventArgs)
        {
            ValidateSessionHandle();

            RaiseEvent<EventArgs>(CameraAttachedEventKey, eventArgs);
        }

        //==========================================================================================
        /// <summary>
        /// Occurs when the camera is detached.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public event EventHandler<EventArgs> CameraDetached
        {
            add
            {
                ValidateSessionHandle();

                lock (CameraDetachedLockObject)
                {
                    if (_cameraDetachedEventHandlers.Count == 0)
                    {
                        _cameraDetachedDriverCallbackInstalled = true;
                        ImaqdxEventManager.InstallPnpEventHandler(_sessionHandle, ImaqdxPnpEvent.CameraDetached, _cameraDetachedDriverCallback);
                        _callbackManager.AddEventHandler(CameraDetachedEventKey, _cameraDetachedEventHandler);
                    }
                    _cameraDetachedEventHandlers.Add(value);
                }
            }
            remove
            {
                ValidateSessionHandle();

                lock (CameraDetachedLockObject)
                {
                    _cameraDetachedEventHandlers.Remove(value);
                    if (_cameraDetachedEventHandlers.Count == 0)
                    {
                        _callbackManager.RemoveEventHandler(CameraDetachedEventKey, _cameraDetachedEventHandler);
                        _cameraDetachedDriverCallbackInstalled = false;
                    }
                }
            }
        }

        private uint CameraDetachedDriverCallback(uint session, ImaqdxPnpEvent pnpEvent, IntPtr callbackData)
        {
            RaiseEvent<EventArgs>(CameraDetachedEventKey, EventArgs.Empty);
            return (uint)(_cameraDetachedDriverCallbackInstalled ? 1 : 0);
        }

        private void OnCameraDetached(object sender, EventArgs e)
        {
            if (_sessionHandle == null)
                return;

            lock (CameraDetachedLockObject)
            {
                foreach (EventHandler<EventArgs> cameraDetached in _cameraDetachedEventHandlers)
                {
                    cameraDetached(sender, e);
                }
            }
        }

        //==========================================================================================
        /// <summary>
        /// </summary>
        /// <param name="eventArgs">
        /// </param>
        //==========================================================================================
        protected virtual void OnCameraDetached(EventArgs eventArgs)
        {
            ValidateSessionHandle();

            RaiseEvent<EventArgs>(CameraDetachedEventKey, eventArgs);
        }

        //==========================================================================================
        /// <summary>
        /// Occurs when a FireWire bus reset occurs.
        /// .
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public event EventHandler<EventArgs> BusResetCompleted
        {
            add
            {
                ValidateSessionHandle();

                lock (BusResetCompletedLockObject)
                {
                    if (_busResetCompletedEventHandlers.Count == 0)
                    {
                        _busResetCompletedDriverCallbackInstalled = true;
                        ImaqdxEventManager.InstallPnpEventHandler(_sessionHandle, ImaqdxPnpEvent.BusResetCompleted, _busResetCompletedDriverCallback);
                        _callbackManager.AddEventHandler(BusResetCompletedEventKey, _busResetCompletedEventHandler);
                    }
                    _busResetCompletedEventHandlers.Add(value);
                }
            }
            remove
            {
                ValidateSessionHandle();

                lock (BusResetCompletedLockObject)
                {
                    _busResetCompletedEventHandlers.Remove(value);
                    if (_busResetCompletedEventHandlers.Count == 0)
                    {
                        _callbackManager.RemoveEventHandler(BusResetCompletedEventKey, _busResetCompletedEventHandler);
                        _busResetCompletedDriverCallbackInstalled = false;
                    }
                }
            }
        }

        private uint BusResetCompletedDriverCallback(uint session, ImaqdxPnpEvent pnpEvent, IntPtr callbackData)
        {
            RaiseEvent<EventArgs>(BusResetCompletedEventKey, EventArgs.Empty);
            return (uint)(_busResetCompletedDriverCallbackInstalled ? 1 : 0);
        }

        private void OnBusResetCompleted(object sender, EventArgs e)
        {
            if (_sessionHandle == null)
                return;

            lock (BusResetCompletedLockObject)
            {
                foreach (EventHandler<EventArgs> busResetCompleted in _busResetCompletedEventHandlers)
                {
                    busResetCompleted(sender, e);
                }
            }
        }

        //==========================================================================================
        /// <summary>
        /// </summary>
        /// <param name="eventArgs">
        /// </param>
        //==========================================================================================
        protected virtual void OnBusResetCompleted(EventArgs eventArgs)
        {
            ValidateSessionHandle();

            RaiseEvent<EventArgs>(BusResetCompletedEventKey, eventArgs);
        }

        //==========================================================================================
        /// <summary>
        /// Accesses registers on the camera and writes a 32-bit value to the camera. Data is byte-swapped for big endian alignment before transfer.
        /// </summary>
        /// <param name="offset">
        /// The register location to access. Refer to the camera documentation for more information about camera-specific register ranges. Use attribute <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxStandardAttribute.BaseAddress" crefType="Unqualified"/> to obtain the base address for the camera.
        /// </param>
        /// <param name="value">
        /// Specifies the value to write to the memory offset.
        /// </param>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        [CLSCompliant(false)]
        public void WriteRegister(ulong offset, uint value)
        {
            ValidateSessionHandle();

            ImaqdxSessionManager.WriteRegister(_sessionHandle, offset, value);
        }

        //==========================================================================================
        /// <summary>
        /// Accesses registers on the camera and reads a 32-bit value from the camera. Data is byte-swapped for little endian alignment after transfer.
        /// </summary>
        /// <param name="offset">
        /// The register location to access. Refer to the camera documentation for more information about camera-specific register ranges.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        [CLSCompliant(false)]
        public uint ReadRegister(ulong offset)
        {
            ValidateSessionHandle();

            return ImaqdxSessionManager.ReadRegister(_sessionHandle, offset);
        }

        //==========================================================================================
        /// <summary>
        /// Accesses registers on the camera and writes a string to the camera.
        /// </summary>
        /// <param name="offset">
        /// The register location to access. Refer to the camera documentation for more information about camera-specific register ranges. Use attribute <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxStandardAttribute.BaseAddress" crefType="Unqualified"/> to obtain the base address for the camera.
        /// </param>
        /// <param name="values">
        /// Specifies the string to write to the memory offset.
        /// </param>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// 	<paramref name="values"/> is <see langword="null"/>.
        /// </exception>
        //==========================================================================================
        [CLSCompliant(false)]
        public void WriteMemory(ulong offset, byte[] values)
        {
            ValidateSessionHandle();
            if (values == null)
                throw ExceptionBuilder.ArgumentNull("values");

            ImaqdxSessionManager.WriteMemory(_sessionHandle, offset, values);
        }

        //==========================================================================================
        /// <summary>
        /// Accesses registers on the camera and reads a string from the camera.
        /// </summary>
        /// <param name="offset">
        /// The register location to access. Refer to the camera documentation for more information about camera-specific register ranges.
        /// </param>
        /// <param name="count">
        /// Specifies the maximum length of the string read from the memory offset.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// 	<paramref name="count"/> is less than or equal to 0.
        /// </exception>
        //==========================================================================================
        [CLSCompliant(false)]
        public byte[] ReadMemory(ulong offset, int count)
        {
            ValidateSessionHandle();
            if (count <= 0)
                throw ExceptionBuilder.ArgumentOutOfRange("count");

            return ImaqdxSessionManager.ReadMemory(_sessionHandle, offset, count);
        }

        private void RaiseEvent<TEventArgs>(string eventKey, TEventArgs eventArgs) where TEventArgs : EventArgs
        {
            try
            {
                lock (CallbackLockObject)
                {
                    if (_sessionHandle == null)
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
            return String.Format(CultureInfo.InvariantCulture, "{0}: Name={1}", GetType().Name, _cameraName);
        }

        internal void ValidateSessionHandle()
        {
            if (_sessionHandle == null)
                throw ExceptionBuilder.SessionDisposed();
        }

        internal VisionImage CreateImage(VisionImage image)
        {
            if (image == null)
            {
                image = new VisionImage();
            }

            ImageType imageType = ImageType;
            if (image.Type != imageType)
            {
                image.Type = imageType;
            }
            return image;
        }

        private VisionImage[] CreateImages(VisionImage[] images, int numberOfImages)
        {
            if (images == null || numberOfImages != images.Length)
            {
                images = new VisionImage[numberOfImages];
            }
            for (int i = 0; i < images.Length; i++)
            {
                if (images[i] == null)
                {
                    images[i] = new VisionImage();
                }

                ImageType imageType = ImageType;
                if (images[i].Type != imageType)
                {
                    images[i].Type = imageType;
                }
            }
            return images;
        }
    }
}
