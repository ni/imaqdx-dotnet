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
    /// Defines Enumeration camera attributes.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <threadsafety safety="safe"/>
    /// <exception cref="System.ObjectDisposedException">
    /// The session has been disposed.
    /// </exception>
    //==============================================================================================
    public class ImaqdxEnumAttribute : ImaqdxAttribute
    {
        internal ImaqdxEnumAttribute(ImaqdxSession session, ImaqdxAttributeInformation attributeInfo)
            : base(session, attributeInfo)
        { }

        //==========================================================================================
        /// <summary>
        /// Gets the values supported by the camera attribute.
        /// </summary>
        /// <returns>
        /// The values supported by the camera attribute.
        /// </returns>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public ImaqdxEnumAttributeItem[] GetSupportedValues()
        {
            // This is a method to make it clear that it should not be called directly in a loop
            Session.ValidateSessionHandle();

            return ImaqdxAttributeManager.EnumerateAttributeValues(Session.Handle, Name);
        }

        //==========================================================================================
        /// <summary>
        /// Gets or sets the value as an enumeration for the current attribute.
        /// </summary>
        /// <value>
        /// The current value for the attribute.
        /// </value>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// 	<paramref name="value"/>   is <see langword="null"/>.
        /// </exception>
        //==========================================================================================
        public ImaqdxEnumAttributeItem Value
        {
            get
            {
                Session.ValidateSessionHandle();

                ImaqdxEnumAttributeItem value;
                ImaqdxAttributeManager.GetAttribute(Session.Handle, Name, out value);
                return value;
            }
            set
            {
                Session.ValidateSessionHandle();
                if (value == null)
                    throw ExceptionBuilder.ArgumentNull("value");

                ImaqdxAttributeManager.SetAttribute(Session.Handle, Name, value);
            }
        }
    }
}
