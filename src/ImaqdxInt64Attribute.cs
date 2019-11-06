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
    /// Defines 64-bit integer camera attributes.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <threadsafety safety="safe"/>
    //==============================================================================================
    public class ImaqdxInt64Attribute : ImaqdxNumericAttribute
    {
        private ImaqdxNumericAttribute<long> _numericAttribute;

        internal ImaqdxInt64Attribute(ImaqdxSession session, ImaqdxAttributeInformation attributeInfo)
            : base(session, attributeInfo)
        {
            _numericAttribute = new ImaqdxNumericAttribute<long>(this);
        }

        //==========================================================================================
        /// <summary>
        /// Gets the current minimum as a 64-bit integer for the active attribute.
        /// </summary>
        /// <value>
        /// The minimum value for the active attribute.
        /// </value>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public new long Minimum
        {
            get
            {
                Session.ValidateSessionHandle();

                return _numericAttribute.Minimum;
            }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the current maximum as a 64-bit integer for the active attribute.
        /// </summary>
        /// <value>The maximum value for the active attribute.
        /// </value>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public new long Maximum
        {
            get
            {
                Session.ValidateSessionHandle();

                return _numericAttribute.Maximum;
            }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the increment as a 64-bit integer for the active attribute.
        /// </summary>
        /// <value>
        /// The increment for the active attribute.
        /// </value>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public new long Increment
        {
            get
            {
                Session.ValidateSessionHandle();

                return _numericAttribute.Increment;
            }
        }

        //==========================================================================================
        /// <summary>
        /// Gets or sets the value as a 64-bit integer for the current attribute.
        /// </summary>
        /// <value>
        /// The current value for the attribute.
        /// </value>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public new long Value
        {
            get
            {
                Session.ValidateSessionHandle();

                return _numericAttribute.Value;
            }
            set
            {
                Session.ValidateSessionHandle();

                _numericAttribute.Value = value;
            }
        }
    }
}
