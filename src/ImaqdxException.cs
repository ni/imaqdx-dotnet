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
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace NationalInstruments.Vision.Acquisition.Imaqdx
{
    //==============================================================================================
    /// <summary>
    /// Represents an error received from the underlying driver.
    ///
    /// </summary>
    /// <remarks>
    /// Occasionally, errors occur when this API makes calls into the underlying driver. When an error occurs, the error is exposed through this exception class.
    /// </remarks>
    /// <threadsafety safety="safe"/>
    //==============================================================================================
    [Serializable]
    public class ImaqdxException : SystemException, ISerializable
    {
        private const string ErrorKey = "Error";

        private ImaqdxError _error;

        //==========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxException" crefType="Unqualified"/> class.
        /// </summary>
        //==========================================================================================
        public ImaqdxException()
            : base()
        { }

        //==========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxException" crefType="Unqualified"/> class using the given error message.
        /// </summary>
        /// <param name="message">
        /// An error message describing this exception.
        /// </param>
        //==========================================================================================
        public ImaqdxException(string message)
            : this(message, null)
        { }

        //==========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxException" crefType="Unqualified"/> class using the given error message and inner exception.
        /// </summary>
        /// <param name="message">
        /// An error message describing this exception.
        /// </param>
        /// <param name="inner">
        /// The inner exception that causes this exception to be thrown.
        /// </param>
        //==========================================================================================
        public ImaqdxException(string message, Exception inner)
            : this(message, inner, 0)
        { }

        //==========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxException" crefType="Unqualified"/> class using the given error message and error code.
        /// </summary>
        /// <param name="message">
        /// An error message describing this exception.
        /// </param>
        /// <param name="error">
        /// The <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxError" crefType="Unqualified"/>  associated with this exception.
        /// </param>
        //==========================================================================================
        public ImaqdxException(string message, ImaqdxError error)
            : this(message, null, error)
        { }

        //==========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxException" crefType="Unqualified"/> class using the given error message, inner exception, and error code.
        /// </summary>
        /// <param name="message">
        /// An error message describing this exception.
        /// </param>
        /// <param name="inner">
        /// The inner exception that caused this exception to be thrown.
        /// </param>
        /// <param name="error">
        /// The <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxError" crefType="Unqualified"/> associated with this exception.
        /// </param>
        //==========================================================================================
        public ImaqdxException(string message, Exception inner, ImaqdxError error)
            : base(message, inner)
        {
            _error = error;
        }

        //==========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxException" crefType="Unqualified"/> class using serialized data.
        /// </summary>
        /// <param name="info">
        /// Object that holds the serialized object data.
        /// </param>
        /// <param name="context">
        /// Contextual information about the source or destination.
        /// </param>
        //==========================================================================================
        protected ImaqdxException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            _error = (ImaqdxError)info.GetInt32(ErrorKey);
        }

        //==========================================================================================
        /// <summary>
        /// Sets the <see cref="System.Runtime.Serialization.SerializationInfo" crefType="Unqualified"/> object with information about the exception.
        /// </summary>
        /// <param name="info">
        /// The object that holds the serialized object data.
        /// </param>
        /// <param name="context">
        /// Contextual information about the source or destination.
        /// </param>
        //==========================================================================================
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected virtual new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            if (info == null)
                throw new ArgumentNullException("info");

            info.AddValue(ErrorKey, _error);
        }

        //==========================================================================================
        /// <summary>
        /// Gets the error associated with the exception.
        /// </summary>
        /// <value>
        /// The value of the error.
        /// </value>
        //==========================================================================================
        public ImaqdxError Error
        {
            get { return _error; }
        }
    }
}
