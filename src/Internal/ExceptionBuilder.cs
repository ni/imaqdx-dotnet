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
using System.Diagnostics;
using System.Globalization;
using NationalInstruments.Restricted;

namespace NationalInstruments.Vision.Acquisition.Imaqdx.Internal
{
    internal sealed class ExceptionBuilder : ExceptionBuilderBase
    {
        private ExceptionBuilder()
        { }

        public static void CheckErrorAndThrow(int status)
        {
            if (status < 0)
                throw ImaqdxError(status);
        }

        public static Exception ImaqdxError(int error)
        {
            Debug.Assert(error < 0);

            string message;
            try
            {
                int status = NiImaqdxDll.IMAQdxGetErrorStringCW(error, out message);
                if (status < 0)
                    message = String.Format(CultureInfo.InvariantCulture, "Unable to get error message from driver.");
            }
            catch (Exception)
            {
                message = String.Format(CultureInfo.InvariantCulture, "Unable to get error message from driver.");
            }

            return Trace(new ImaqdxException(message, (ImaqdxError)error));
        }

        public static Exception SessionDisposed()
        {
            return ExceptionBuilder.ObjectDisposed(String.Format(CultureInfo.InvariantCulture, "The IMAQdx session has been disposed."));
        }

        public static Exception ArgumentLessThanZero(string argumentName)
        {
            return ExceptionBuilder.ArgumentOutOfRange(String.Format(CultureInfo.InvariantCulture, "{0} is less than zero.", argumentName));
        }

        public static Exception ArgumentGreaterThanCount(string argumentName)
        {
            return ExceptionBuilder.ArgumentOutOfRange(String.Format(CultureInfo.InvariantCulture, "{0} is greater than the number of elements.", argumentName));
        }

        public static Exception CommandAttributesNotReadable()
        {
            return ExceptionBuilder.InvalidOperation(String.Format(CultureInfo.InvariantCulture, "Command attributes are not readable."));
        }

        public static Exception UnknownDataType()
        {
            return ExceptionBuilder.InvalidOperation(String.Format(CultureInfo.InvariantCulture, "Unknown IMAQdx data type."));
        }

        public static Exception NonexistentPrivateAttribute()
        {
            return ExceptionBuilder.InvalidOperation(String.Format(CultureInfo.InvariantCulture, "The private IMAQdx attribute does not exist."));
        }
    }
}
