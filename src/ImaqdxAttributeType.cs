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
    /// Specifies the type of attribute.
    /// </summary>
    //==============================================================================================
    public enum ImaqdxAttributeType : int
    {
        //==========================================================================================
        /// <summary>
        /// The IMAQdx attribute type is <see cref="System.UInt32" crefType="Unqualified"/>.</summary>
        //==========================================================================================
        UInt32,
        //==========================================================================================
        /// <summary>
        /// The IMAQdx attribute type is <see cref="System.Int64" crefType="Unqualified"/>.</summary>
        //==========================================================================================
        Int64,
        //==========================================================================================
        /// <summary>
        /// The IMAQdx attribute type is <see cref="System.Double" crefType="Unqualified"/>.</summary>
        //==========================================================================================
        Double,
        //==========================================================================================
        /// <summary>
        /// The IMAQdx attribute type is <see cref="System.String" crefType="Unqualified"/>.</summary>
        //==========================================================================================
        String,
        //==========================================================================================
        /// <summary>
        /// The IMAQdx attribute type is Enum.</summary>
        //==========================================================================================
        Enum,
        //==========================================================================================
        /// <summary>
        /// The IMAQdx attribute type is <see cref="System.Boolean" crefType="Unqualified"/>.</summary>
        //==========================================================================================
        Boolean,
        //==========================================================================================
        /// <summary>
        /// The IMAQdx attribute type is Command.</summary>
        //==========================================================================================
        Command
    }
}
