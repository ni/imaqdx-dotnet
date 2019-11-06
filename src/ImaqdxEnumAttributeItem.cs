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
using NationalInstruments.Vision.Acquisition.Imaqdx.Internal;

namespace NationalInstruments.Vision.Acquisition.Imaqdx
{
    //==============================================================================================
    /// <summary>
    /// Represents an enumeration item, which is a name/value pair.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <threadsafety safety="safe"/>
    //==============================================================================================
    public class ImaqdxEnumAttributeItem
    {
        private ImaqdxEnumItem _enumItem;
        private long _value;

        internal ImaqdxEnumAttributeItem(ImaqdxEnumItem enumItem)
        {
            _enumItem = enumItem;
            long valueLow = enumItem.Value;
            long valueHigh = enumItem.Reserved;
            valueHigh = valueHigh << 32;
            _value = valueLow | valueHigh;
        }

        //==========================================================================================
        /// <summary>
        /// Gets  the name of the enum item.
        /// </summary>
        /// <value>
        /// The name of the enum item.
        /// </value>
        //==========================================================================================
        public string Name
        {
            get { return _enumItem.Name; }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the value of the enum item.
        /// </summary>
        /// <value>
        /// The value of the enum item.
        /// </value>
        //==========================================================================================
        public long Value
        {
            get { return _value; }
        }

        internal ImaqdxEnumItem EnumItem
        {
            get { return _enumItem; }
        }

        //==========================================================================================
        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <returns>
        /// </returns>
        //==========================================================================================
        public override bool Equals(object obj)
        {
            if (!(obj is ImaqdxEnumAttributeItem))
                return false;

            return Equals((ImaqdxEnumAttributeItem)obj);
        }

        //==========================================================================================
        /// <summary>
        /// </summary>
        /// <param name="other">
        /// </param>
        /// <returns>
        /// </returns>
        //==========================================================================================
        public bool Equals(ImaqdxEnumAttributeItem other)
        {
            return ((_enumItem.Name == other._enumItem.Name) &&
                    (_value == other._value));
        }

        //==========================================================================================
        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        //==========================================================================================
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        //==========================================================================================
        /// <summary>
        /// Converts the value of this instance to its equivalent enumeration item name.
        /// </summary>
        /// <returns>
        /// An enumeration item name of the value of this instance.
        /// </returns>
        //==========================================================================================
        public override string ToString()
        {
            return Name;
        }
    }
}
