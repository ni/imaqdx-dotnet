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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.ComponentModel;
using NationalInstruments.Vision.Acquisition.Imaqdx.Internal;

namespace NationalInstruments.Vision.Acquisition.Imaqdx
{
    //==============================================================================================
    /// <summary>
    /// Represents a strongly typed collection of <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxAttribute" crefType="Unqualified"/> objects.
    /// </summary>
    /// <threadsafety safety="safe"/>
    //==============================================================================================
    public sealed class ImaqdxAttributeCollection : ReadOnlyCollection<ImaqdxAttribute>
    {
        private ImaqdxSession _session;
        private Dictionary<string, int> _attributeLookupTable;

        internal ImaqdxAttributeCollection(ImaqdxSession session)
            : base(new List<ImaqdxAttribute>())
        {
            _session = session;
            _attributeLookupTable = new Dictionary<string, int>();

            // Initialize the attribute collection
            ImaqdxAttributeInformation[] attributeInfoArray = ImaqdxAttributeManager.EnumeratePublicAttributes(session.Handle);
            for (int i = 0; i < attributeInfoArray.Length; i++)
            {
                AddAttribute(attributeInfoArray[i]);
            }
        }

        private void AddAttribute(ImaqdxAttributeInformation attributeInfo)
        {
            ImaqdxAttribute attribute = null;
            switch (attributeInfo.Type)
            {
                case ImaqdxAttributeType.UInt32:
                    attribute = new ImaqdxUInt32Attribute(_session, attributeInfo);
                    break;
                case ImaqdxAttributeType.Int64:
                    attribute = new ImaqdxInt64Attribute(_session, attributeInfo);
                    break;
                case ImaqdxAttributeType.Double:
                    attribute = new ImaqdxDoubleAttribute(_session, attributeInfo);
                    break;
                case ImaqdxAttributeType.String:
                    attribute = new ImaqdxStringAttribute(_session, attributeInfo);
                    break;
                case ImaqdxAttributeType.Enum:
                    attribute = new ImaqdxEnumAttribute(_session, attributeInfo);
                    break;
                case ImaqdxAttributeType.Boolean:
                    attribute = new ImaqdxBoolAttribute(_session, attributeInfo);
                    break;
                case ImaqdxAttributeType.Command:
                    attribute = new ImaqdxCommandAttribute(_session, attributeInfo);
                    break;
                default:
                    attribute = new ImaqdxAttribute(_session, attributeInfo);
                    break;
            }
            Items.Add(attribute);
            int index = Items.Count - 1;
            _attributeLookupTable.Add(attribute.Name, index);
        }

        //==========================================================================================
        /// <summary>
        /// Gets the <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxAttribute" crefType="Unqualified"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxAttribute" crefType="Unqualified"/> at the specified index.</value>
        /// <param name="index">
        /// The zero-based index of the entry to locate in the collection.
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is less than zero.
        /// <para>
        /// -or-
        /// </para>
        /// 	<paramref name="index"/> is greater than or equal to the collection's Count.
        /// </exception>
        //==========================================================================================
        public new ImaqdxAttribute this[int index]
        {
            get
            {
                if (index < 0)
                    throw ExceptionBuilder.ArgumentLessThanZero("index");
                if (index >= Count)
                    throw ExceptionBuilder.ArgumentGreaterThanCount("index");

                return Items[index];
            }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxAttribute" crefType="Unqualified"/> with the specified name.
        /// </summary>
        /// <value>
        /// The <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxAttribute" crefType="Unqualified"/> with the specified name; <see langword="null"/> if an <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxAttribute" crefType="Unqualified"/> with the specified name does not exist in the collection.
        ///
        /// </value>
        /// <param name="name">
        /// The name of the entry to locate in the collection.
        ///
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// 	<paramref name="name"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="name"/> is an empty string.
        /// </exception>
        //==========================================================================================
        public ImaqdxAttribute this[string name]
        {
            get
            {
                _session.ValidateSessionHandle();
                if (name == null)
                    throw ExceptionBuilder.ArgumentNull("name");
                if (name.Length == 0)
                    throw ExceptionBuilder.EmptyString("name");

                string fullName = ImaqdxSessionManager.GetFullAttributeName(_session.Handle, name);
                if (_attributeLookupTable.ContainsKey(fullName))
                {
                    int index = _attributeLookupTable[fullName];
                    return Items[index];
                }

                return null;
            }
        }

        //==========================================================================================
        /// <summary>
        /// Gets the <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxAttribute" crefType="Unqualified"/> with the specified <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxStandardAttribute" crefType="Unqualified"/> value.
        /// </summary>
        /// <value>
        /// The <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxAttribute" crefType="Unqualified"/> with the specified  <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxStandardAttribute" crefType="Unqualified"/> value; <see langword="null"/> if an <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxAttribute" crefType="Unqualified"/> with the specified  <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxStandardAttribute" crefType="Unqualified"/> value does not exist in the collection.
        ///
        /// </value>
        /// <param name="attribute">
        /// The <see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxStandardAttribute" crefType="Unqualified"/> value of the entry to locate in the collection.
        /// </param>
        /// <remarks>
        /// Implements <see cref="System.Collections.IList.IsReadOnly" crefType="Unqualified"/>.
        /// </remarks>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">
        /// 	<paramref name="attribute"/> is an invalid enum value.
        /// </exception>
        //==========================================================================================
        public ImaqdxAttribute this[ImaqdxStandardAttribute attribute]
        {
            get
            {
                if (!Enum.IsDefined(typeof(ImaqdxStandardAttribute), attribute))
                    throw ExceptionBuilder.InvalidEnumArgument("attribute", (int)attribute, typeof(ImaqdxStandardAttribute));

                string fullName = ImaqdxAttributeManager.AttributeNamePairs[attribute];
                return this[fullName];
            }
        }

        //==========================================================================================
        /// <summary>
        /// Determines if the specified name element is in the collection.
        /// </summary>
        /// <param name="name">
        /// Name of the attribute to locate in the collection.
        /// </param>
        /// <returns>
        /// 	<see langword="true"/> if the collection contains <paramref name="name"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// 	<paramref name="name"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// 	<paramref name="name"/> is empty.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The session is disposed.
        /// </exception>
        //==========================================================================================
        public bool Contains(string name)
        {
            _session.ValidateSessionHandle();
            if (name == null)
                throw ExceptionBuilder.ArgumentNull("name");
            if (name.Length == 0)
                throw ExceptionBuilder.EmptyString("name");

            string fullName = ImaqdxSessionManager.GetFullAttributeName(_session.Handle, name);
            return _attributeLookupTable.ContainsKey(fullName);
        }

        //==========================================================================================
        /// <summary>
        /// Determines if the specified attribute element is in the collection.
        /// </summary>
        /// <param name="attribute">
        /// 	<see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxStandardAttribute" crefType="Unqualified"/> to locate in the collection.
        /// </param>
        /// <returns>
        /// 	<see langword="true"/> if the collection contains <paramref name="attribute"/>.
        /// </returns>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">
        /// 	<paramref name="attribute"/> is not a valid value.
        /// </exception>
        //==========================================================================================
        public bool Contains(ImaqdxStandardAttribute attribute)
        {
            if (!Enum.IsDefined(typeof(ImaqdxStandardAttribute), attribute))
                throw ExceptionBuilder.InvalidEnumArgument("attribute", (int)attribute, typeof(ImaqdxStandardAttribute));

            string fullName = ImaqdxAttributeManager.AttributeNamePairs[attribute];
            return Contains(fullName);
        }

        //==========================================================================================
        /// <summary>
        /// Returns the zero-based index of the first occurrence of  a name in the collection.
        /// </summary>
        /// <param name="name">
        /// Name of the attribute to search for.
        /// </param>
        /// <returns>
        /// Index of the name. If the name is not found, returns -1.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// 	<paramref name="name"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        /// The session is disposed.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// 	<paramref name="name"/> is empty.
        /// </exception>
        //==========================================================================================
        public int IndexOf(string name)
        {
            _session.ValidateSessionHandle();
            if (name == null)
                throw ExceptionBuilder.ArgumentNull("name");
            if (name.Length == 0)
                throw ExceptionBuilder.EmptyString("name");

            string fullName = ImaqdxSessionManager.GetFullAttributeName(_session.Handle, name);
            if (_attributeLookupTable.ContainsKey(fullName))
                return _attributeLookupTable[fullName];

            return -1;
        }

        //==========================================================================================
        /// <summary>
        /// Returns the zero-based index of the first occurrence of an attribute in the collection.
        /// </summary>
        /// <param name="attribute">
        /// 	<see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxStandardAttribute" crefType="Unqualified"/> to search for.
        /// </param>
        /// <returns>
        /// Index of the attribute. If the attribute is not found, returns –1.
        /// </returns>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">
        /// 	<see cref="NationalInstruments.Vision.Acquisition.Imaqdx.ImaqdxStandardAttribute" crefType="Unqualified"/> is not a valid value.
        /// </exception>
        //==========================================================================================
        public int IndexOf(ImaqdxStandardAttribute attribute)
        {
            if (!Enum.IsDefined(typeof(ImaqdxStandardAttribute), attribute))
                throw ExceptionBuilder.InvalidEnumArgument("attribute", (int)attribute, typeof(ImaqdxStandardAttribute));

            string fullName = ImaqdxAttributeManager.AttributeNamePairs[attribute];
            return IndexOf(fullName);
        }

        //==========================================================================================
        /// <summary>
        /// Writes current attributes to the camera file. This method is only required if you wish to save parameters.
        ///
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public void WriteAttributesToCameraFile()
        {
            _session.ValidateSessionHandle();

            ImaqdxSessionManager.WriteAttributesToCameraFile(_session.Handle);
        }

        //==========================================================================================
        /// <summary>
        /// Reads attributes from the current camera file and applies to the current session.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        //==========================================================================================
        public void ReadAttributesFromCameraFile()
        {
            _session.ValidateSessionHandle();

            // Reading in attributes loads values for the current attributes
            // The attribute collection does not need to be repopulated
            ImaqdxSessionManager.ReadAttributesFromCameraFile(_session.Handle);
        }

        //==========================================================================================
        /// <summary>
        /// Writes current attributes to the specified file.</summary>
        /// <param name="fullPath">
        /// The full path of the file.
        /// </param>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// 	<paramref name="fullPath"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// 	<paramref name="fullPath"/> is an empty string.
        /// </exception>
        //==========================================================================================
        public void WriteAttributesToFile(string fullPath)
        {
            _session.ValidateSessionHandle();
            if (fullPath == null)
                throw ExceptionBuilder.ArgumentNull("fullPath");
            if (fullPath.Length == 0)
                throw ExceptionBuilder.EmptyString("fullPath");

            ImaqdxSessionManager.WriteAttributesToFile(_session.Handle, fullPath);
        }

        //==========================================================================================
        /// <summary>
        /// Reads attributes from the file parameter and applies to the current session.
        /// </summary>
        /// <param name="fullPath">
        /// The full path of the file.
        /// </param>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// 	<paramref name="fullPath"/> is <see langword="null"/>.
        /// <para>
        /// -or-
        /// </para>
        /// 	<paramref name="fullPath"/>  is an empty string.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// 	<paramref name="fullPath"/> is an empty string.
        /// </exception>
        //==========================================================================================
        public void ReadAttributesFromFile(string fullPath)
        {
            _session.ValidateSessionHandle();
            if (fullPath == null)
                throw ExceptionBuilder.ArgumentNull("fullPath");
            if (fullPath.Length == 0)
                throw ExceptionBuilder.EmptyString("fullPath");

            // Reading in attributes loads values for the current attributes
            // The attribute collection does not need to be repopulated
            ImaqdxSessionManager.ReadAttributesFromFile(_session.Handle, fullPath);
        }

        //==========================================================================================
        /// <summary>
        /// Writes attributes to the string parameter and returns it.
        /// </summary>
        /// <returns>
        /// The string containing the written attributes.
        /// </returns>
        /// <exception cref="System.ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        //==========================================================================================
        public string WriteAttributesToString()
        {
            _session.ValidateSessionHandle();

            return ImaqdxSessionManager.WriteAttributesToString(_session.Handle);
        }

        //==========================================================================================
        /// <summary>
        /// Reads attributes from the string parameter and applies to the current session.
        /// </summary>
        /// <param name="attributesString">
        /// The string that contains attribute information.
        /// </param>
        /// <exception cref="System.ObjectDisposedException">
        /// The session has been disposed.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// 	<paramref name="attributesString"/> is <see langword="null"/>.
        /// <para>
        /// -or-
        /// </para>
        /// 	<paramref name="attributesString"/>  is an empty string.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// 	<paramref name="attributesString"/> is an empty string.
        /// </exception>
        //==========================================================================================
        public void ReadAttributesFromString(string attributesString)
        {
            _session.ValidateSessionHandle();
            if (attributesString == null)
                throw ExceptionBuilder.ArgumentNull("attributesString");
            if (attributesString.Length == 0)
                throw ExceptionBuilder.EmptyString("attributesString");

            // Reading in attributes loads values for the current attributes
            // The attribute collection does not need to be repopulated
            ImaqdxSessionManager.ReadAttributesFromString(_session.Handle, attributesString);
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
            return String.Format(CultureInfo.InvariantCulture, "{0}: Count={1}", GetType().Name, Count);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void AddPrivateAttribute(string name)
        {
            _session.ValidateSessionHandle();
            if (name == null)
                throw ExceptionBuilder.ArgumentNull("name");
            if (name.Length == 0)
                throw ExceptionBuilder.EmptyString("name");

            ImaqdxAttributeInformation[] attributeInfoArray = ImaqdxAttributeManager.EnumeratePrivateAttributes(_session.Handle);
            for (int i = 0; i < attributeInfoArray.Length; i++)
            {
                if (attributeInfoArray[i].Name == name)
                {
                    AddAttribute(attributeInfoArray[i]);
                    return;
                }
            }

            throw ExceptionBuilder.NonexistentPrivateAttribute();
        }
    }
}
