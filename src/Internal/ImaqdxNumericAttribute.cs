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

namespace NationalInstruments.Vision.Acquisition.Imaqdx.Internal
{
    internal class ImaqdxNumericAttribute<TValue>
    {
        private Type _dataType;
        private ImaqdxAttribute _attribute;

        public ImaqdxNumericAttribute(ImaqdxAttribute attribute)
        {
            _dataType = typeof(TValue);
            _attribute = attribute;
        }

        public TValue Minimum
        {
            get
            {
                object value;
                if (_dataType == typeof(uint))
                {
                    uint uintValue;
                    ImaqdxAttributeManager.GetAttributeMinimum(_attribute.Session.Handle, _attribute.Name, out uintValue);
                    value = uintValue;
                }
                else if (_dataType == typeof(long))
                {
                    long longValue;
                    ImaqdxAttributeManager.GetAttributeMinimum(_attribute.Session.Handle, _attribute.Name, out longValue);
                    value = longValue;
                }
                else if (_dataType == typeof(double))
                {
                    double doubleValue;
                    ImaqdxAttributeManager.GetAttributeMinimum(_attribute.Session.Handle, _attribute.Name, out doubleValue);
                    value = doubleValue;
                }
                else if (_dataType == typeof(decimal))
                {
                    double doubleValue;
                    ImaqdxAttributeManager.GetAttributeMinimum(_attribute.Session.Handle, _attribute.Name, out doubleValue);
                    value = (decimal)doubleValue;
                }
                else
                {
                    throw ExceptionBuilder.UnknownDataType();
                }
                return (TValue)value;
            }
        }

        public TValue Maximum
        {
            get
            {
                object value;
                if (_dataType == typeof(uint))
                {
                    uint uintValue;
                    ImaqdxAttributeManager.GetAttributeMaximum(_attribute.Session.Handle, _attribute.Name, out uintValue);
                    value = uintValue;
                }
                else if (_dataType == typeof(long))
                {
                    long longValue;
                    ImaqdxAttributeManager.GetAttributeMaximum(_attribute.Session.Handle, _attribute.Name, out longValue);
                    value = longValue;
                }
                else if (_dataType == typeof(double))
                {
                    double doubleValue;
                    ImaqdxAttributeManager.GetAttributeMaximum(_attribute.Session.Handle, _attribute.Name, out doubleValue);
                    value = doubleValue;
                }
                else if (_dataType == typeof(decimal))
                {
                    double doubleValue;
                    ImaqdxAttributeManager.GetAttributeMaximum(_attribute.Session.Handle, _attribute.Name, out doubleValue);
                    value = (decimal)doubleValue;
                }
                else
                {
                    throw ExceptionBuilder.UnknownDataType();
                }
                return (TValue)value;
            }
        }

        public TValue Increment
        {
            get
            {
                object value;
                if (_dataType == typeof(uint))
                {
                    uint uintValue;
                    ImaqdxAttributeManager.GetAttributeIncrement(_attribute.Session.Handle, _attribute.Name, out uintValue);
                    value = uintValue;
                }
                else if (_dataType == typeof(long))
                {
                    long longValue;
                    ImaqdxAttributeManager.GetAttributeIncrement(_attribute.Session.Handle, _attribute.Name, out longValue);
                    value = longValue;
                }
                else if (_dataType == typeof(double))
                {
                    double doubleValue;
                    ImaqdxAttributeManager.GetAttributeIncrement(_attribute.Session.Handle, _attribute.Name, out doubleValue);
                    value = doubleValue;
                }
                else if (_dataType == typeof(decimal))
                {
                    double doubleValue;
                    ImaqdxAttributeManager.GetAttributeIncrement(_attribute.Session.Handle, _attribute.Name, out doubleValue);
                    value = (decimal)doubleValue;
                }
                else
                {
                    throw ExceptionBuilder.UnknownDataType();
                }
                return (TValue)value;
            }
        }

        public TValue Value
        {
            get
            {
                object value;
                if (_dataType == typeof(uint))
                {
                    uint uintValue;
                    ImaqdxAttributeManager.GetAttribute(_attribute.Session.Handle, _attribute.Name, out uintValue);
                    value = uintValue;
                }
                else if (_dataType == typeof(long))
                {
                    long longValue;
                    ImaqdxAttributeManager.GetAttribute(_attribute.Session.Handle, _attribute.Name, out longValue);
                    value = longValue;
                }
                else if (_dataType == typeof(double))
                {
                    double doubleValue;
                    ImaqdxAttributeManager.GetAttribute(_attribute.Session.Handle, _attribute.Name, out doubleValue);
                    value = doubleValue;
                }
                else if (_dataType == typeof(decimal))
                {
                    double doubleValue;
                    ImaqdxAttributeManager.GetAttribute(_attribute.Session.Handle, _attribute.Name, out doubleValue);
                    value = (decimal)doubleValue;
                }
                else
                {
                    throw ExceptionBuilder.UnknownDataType();
                }
                return (TValue)value;
            }
            set
            {
                object val = value;
                if (_dataType == typeof(uint))
                {
                    uint uintValue = (uint)val;
                    ImaqdxAttributeManager.SetAttribute(_attribute.Session.Handle, _attribute.Name, uintValue);
                }
                else if (_dataType == typeof(long))
                {
                    long longValue = (long)val;
                    ImaqdxAttributeManager.SetAttribute(_attribute.Session.Handle, _attribute.Name, longValue);
                }
                else if (_dataType == typeof(double))
                {
                    double doubleValue = (double)val;
                    ImaqdxAttributeManager.SetAttribute(_attribute.Session.Handle, _attribute.Name, doubleValue);
                }
                else if (_dataType == typeof(decimal))
                {
                    decimal decimalValue = (decimal)val;
                    ImaqdxAttributeManager.SetAttribute(_attribute.Session.Handle, _attribute.Name, (double)decimalValue);
                }
                else
                {
                    throw ExceptionBuilder.UnknownDataType();
                }
            }
        }
    }
}
