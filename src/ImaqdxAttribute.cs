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
    /// Defines camera attributes.
    /// </summary>
    /// <threadsafety safety="safe"/>
    //==============================================================================================
    public class ImaqdxAttribute
    {
        private string _name;
        private ImaqdxAttributeType _type;
        private ImaqdxSession _session;

        internal ImaqdxAttribute(ImaqdxSession session, ImaqdxAttributeInformation attributeInfo)
        {
            _session = session;
            _name = attributeInfo.Name;
            _type = attributeInfo.Type;
        }

        internal ImaqdxSession Session
        {
            get { return _session; }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the name of the camera attribute.
        /// </summary>
        /// <value>
        /// The name of the camera attribute.
        /// </value>
        //==========================================================================================
        public string Name
        {
            get { return _name; }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the attribute type for a camera attribute.
        /// </summary>
        /// <value>
        /// The type of attribute for a camera attribute.
        /// </value>
        //==========================================================================================
        public ImaqdxAttributeType Type
        {
            get { return _type; }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the display name for the camera attribute. The display name is a human readable version of the attribute name that includes formatting, such as spaces.
        /// </summary>
        /// <value>
        /// The display name for the camera attribute.
        /// </value>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public string DisplayName
        {
            get
            {
                _session.ValidateSessionHandle();

                return ImaqdxAttributeManager.GetAttributeDisplayName(_session.Handle, _name);
            }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the description for a camera attribute.
        /// </summary>
        /// <value>
        /// The description for a camera attribute.
        /// </value>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public string Description
        {
            get
            {
                _session.ValidateSessionHandle();

                return ImaqdxAttributeManager.GetAttributeDescription(_session.Handle, _name);
            }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the tooltip for the camera attribute.
        /// </summary>
        /// <value>
        /// The tooltip for the camera attribute.
        /// </value>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public string Tooltip
        {
            get
            {
                _session.ValidateSessionHandle();

                return ImaqdxAttributeManager.GetAttributeTooltip(_session.Handle, _name);
            }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the attribute units for a camera.
        /// </summary>
        /// <value>
        /// The units of the camera attribute.
        /// </value>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public string Units
        {
            get
            {
                _session.ValidateSessionHandle();

                return ImaqdxAttributeManager.GetAttributeUnits(_session.Handle, _name);
            }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the visibility for the camera attribute.
        /// </summary>
        /// <value>
        /// The visibility for the camera attribute.
        /// </value>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public ImaqdxAttributeVisibility Visibility
        {
            get
            {
                _session.ValidateSessionHandle();

                return ImaqdxAttributeManager.GetAttributeVisibility(_session.Handle, _name);
            }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the read permissions for a camera attribute.
        /// </summary>
        /// <value>
        /// 	<see langword="true"/> if the attribute is readable, otherwise <see langword="false"/>.
        /// </value>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public bool Readable
        {
            get
            {
                _session.ValidateSessionHandle();

                return ImaqdxAttributeManager.GetIsAttributeReadable(_session.Handle, _name);
            }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the write permissions for a camera attribute.
        /// </summary>
        /// <value>
        /// 	<see langword="true"/> if the attribute is writable, otherwise <see langword="false"/>.
        /// </value>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public bool Writable
        {
            get
            {
                _session.ValidateSessionHandle();

                return ImaqdxAttributeManager.GetIsAttributeWritable(_session.Handle, _name);
            }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the current value for a camera attribute.
        /// </summary>
        /// <returns>
        /// The value of the specified attribute. The value is returned as an <see cref="System.Object" crefType="Unqualified"/>.
        /// </returns>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The type of the attribute is invalid or not supported.
        /// </exception>
        //==========================================================================================
        public object GetValue()
        {
            _session.ValidateSessionHandle();

            object value;
            switch (_type)
            {
                case ImaqdxAttributeType.UInt32:
                    uint uintValue;
                    ImaqdxAttributeManager.GetAttribute(_session.Handle, _name, out uintValue);
                    value = uintValue;
                    break;
                case ImaqdxAttributeType.Int64:
                    long longValue;
                    ImaqdxAttributeManager.GetAttribute(_session.Handle, _name, out longValue);
                    value = longValue;
                    break;
                case ImaqdxAttributeType.Double:
                    double doubleValue;
                    ImaqdxAttributeManager.GetAttribute(_session.Handle, _name, out doubleValue);
                    value = doubleValue;
                    break;
                case ImaqdxAttributeType.String:
                    string stringValue;
                    ImaqdxAttributeManager.GetAttribute(_session.Handle, _name, out stringValue);
                    value = stringValue;
                    break;
                case ImaqdxAttributeType.Enum:
                    ImaqdxEnumAttributeItem enumValue;
                    ImaqdxAttributeManager.GetAttribute(_session.Handle, _name, out enumValue);
                    value = enumValue;
                    break;
                case ImaqdxAttributeType.Boolean:
                    bool boolValue;
                    ImaqdxAttributeManager.GetAttribute(_session.Handle, _name, out boolValue);
                    value = boolValue;
                    break;
                case ImaqdxAttributeType.Command:
                    throw ExceptionBuilder.CommandAttributesNotReadable();
                default:
                    throw ExceptionBuilder.UnknownDataType();
            }

            return value;
        }

        //==========================================================================================
        /// <summary>
        /// Sets the value for a camera attribute.
        /// </summary>
        /// <param name="value">
        /// The value to set the camera attribute to.
        /// </param>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// 	<paramref name="value"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The type of the value is invalid or not supported.
        /// </exception>
        //==========================================================================================
        public void SetValue(object value)
        {
            _session.ValidateSessionHandle();
            if (value == null)
                throw ExceptionBuilder.ArgumentNull("value");

            Type dataType = value.GetType();
            if (dataType == typeof(sbyte))
            {
                sbyte sbyteValue = (sbyte)value;
                ImaqdxAttributeManager.SetAttribute(_session.Handle, _name, (uint)sbyteValue);
            }
            else if (dataType == typeof(byte))
            {
                byte byteValue = (byte)value;
                ImaqdxAttributeManager.SetAttribute(_session.Handle, _name, (uint)byteValue);
            }
            else if (dataType == typeof(short))
            {
                short shortValue = (short)value;
                ImaqdxAttributeManager.SetAttribute(_session.Handle, _name, (uint)shortValue);
            }
            else if (dataType == typeof(ushort))
            {
                ushort ushortValue = (ushort)value;
                ImaqdxAttributeManager.SetAttribute(_session.Handle, _name, (uint)ushortValue);
            }
            else if (dataType == typeof(int))
            {
                int intValue = (int)value;
                ImaqdxAttributeManager.SetAttribute(_session.Handle, _name, (uint)intValue);
            }
            else if (dataType == typeof(uint))
            {
                uint uintValue = (uint)value;
                ImaqdxAttributeManager.SetAttribute(_session.Handle, _name, uintValue);
            }
            else if (dataType == typeof(long))
            {
                long longValue = (long)value;
                ImaqdxAttributeManager.SetAttribute(_session.Handle, _name, longValue);
            }
            else if (dataType == typeof(ulong))
            {
                ulong ulongValue = (ulong)value;
                ImaqdxAttributeManager.SetAttribute(_session.Handle, _name, (long)ulongValue);
            }
            else if (dataType == typeof(decimal))
            {
                decimal decimalValue = (decimal)value;
                ImaqdxAttributeManager.SetAttribute(_session.Handle, _name, (double)decimalValue);
            }
            else if (dataType == typeof(float))
            {
                float floatValue = (float)value;
                ImaqdxAttributeManager.SetAttribute(_session.Handle, _name, (double)floatValue);
            }
            else if (dataType == typeof(double))
            {
                double doubleValue = (double)value;
                ImaqdxAttributeManager.SetAttribute(_session.Handle, _name, doubleValue);
            }
            else if (dataType == typeof(string))
            {
                string stringValue = (string)value;
                ImaqdxAttributeManager.SetAttribute(_session.Handle, _name, stringValue);
            }
            else if (dataType == typeof(ImaqdxEnumAttributeItem))
            {
                ImaqdxEnumAttributeItem enumItemValue = (ImaqdxEnumAttributeItem)value;
                ImaqdxAttributeManager.SetAttribute(_session.Handle, _name, enumItemValue);
            }
            else if (dataType == typeof(bool))
            {
                bool boolValue = (bool)value;
                ImaqdxAttributeManager.SetAttribute(_session.Handle, _name, boolValue);
            }
            else
            {
                throw ExceptionBuilder.UnknownDataType();
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
            return String.Format(CultureInfo.InvariantCulture, "{0}: Name={1}, Type={2}", GetType().Name, _name, _type);
        }
    }
}
