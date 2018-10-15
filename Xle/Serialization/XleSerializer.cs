//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace ERY.Xle.Serialization
{
    /// <summary>
    /// Class used to serialize data to a compact XML format.
    /// </summary>
    public class XleSerializer
    {
        private Type objectType;
        private XleTypeSerializerCollection mTypeSerializers = new XleTypeSerializerCollection();

        /// <summary>
        /// An object which implements the ERY.Xle.Serialization.ITypeBinder interface.
        /// This object is used to convert strings to System.Type objects.  The default value
        /// for this property is an object of type ERY.Xle.Serialization.TypeBinder, but
        /// it may be replaced for custom type binding.
        /// </summary>
        public ITypeBinder Binder { get; set; }
        /// <summary>
        /// Gets the collection of type serializers that can be used to serialize arbitrary types.
        /// </summary>
        public XleTypeSerializerCollection TypeSerializers { get { return mTypeSerializers; } }

        /// <summary>
        /// Constructs the XleSerializer.  Pass in the type of the object which is 
        /// the root of the object graph which needs to be serialized.  This type must
        /// implement the IXleSerializable interface.
        /// </summary>
        /// <param name="objectType">The type of the object to serialize.</param>
        /// <param name="objectConstructor">An object which can construct arbitrary types. If this is null
        /// it will be obtained from the platform factory.</param>
        public XleSerializer(Type objectType)
        {
            var typeBinder = new TypeBinder();

            typeBinder.AddAssembly(objectType.GetTypeInfo().Assembly);
            typeBinder.AddAssembly(typeof(XleSerializer).GetTypeInfo().Assembly);

            typeBinder.AddAssemblies(new[]
            {
                Assembly.GetEntryAssembly(),
                Assembly.GetAssembly(typeof(XleSerializer)),
                Assembly.GetAssembly(typeof(Point))
            });

            this.Binder = typeBinder;

            this.objectType = objectType;
        }

        /// <summary>
        /// Serializes an object which implements IXleSerializable to the specified stream.
        /// </summary>
        /// <param name="outStream">The stream to write the XML data to.</param>
        /// <param name="objectGraph">The object to serialize.</param>
        public void Serialize(Stream outStream, IXleSerializable objectGraph)
        {
            if (objectType.GetTypeInfo().IsAssignableFrom(objectGraph.GetType().GetTypeInfo()) == false)
                throw new ArgumentException("Object is not of type " + objectType.Name);

            XleSerializationInfo info = new XleSerializationInfo(Binder, TypeSerializers);

            info.BeginSerialize(objectGraph);

            info.XmlDoc.Save(outStream);
        }

        /// <summary>
        /// Serializes an object to the specified stream.
        /// </summary>
        /// <param name="outStream">The stream to write the XML data to.</param>
        /// <param name="objectGraph">The object to serialize.</param>
        public void Serialize(Stream outStream, object objectGraph)
        {
            if (objectType.GetTypeInfo().IsAssignableFrom(objectGraph.GetType().GetTypeInfo()) == false)
                throw new ArgumentException("Object is not of type " + objectType.Name);

            XleSerializationInfo info = new XleSerializationInfo(Binder, TypeSerializers);

            info.BeginSerialize(objectGraph);

            info.XmlDoc.Save(outStream);
        }

        /// <summary>
        /// Deserializes an object from the XML data in the specified stream.
        /// </summary>
        /// <param name="inStream">The stream containing the XML data.</param>
        /// <returns>The deserialized object.</returns>
        public object Deserialize(Stream inStream)
        {
            XDocument doc = XDocument.Load(XmlReader.Create(inStream));

            XleSerializationInfo info = new XleSerializationInfo(Binder, TypeSerializers, doc);

            return info.BeginDeserialize();
        }

        /// <summary>
        /// Deserializes an object from the XML data in the specified stream with the specified type.
        /// </summary>
        /// <typeparam name="T">Type to cast the return value to.</typeparam>
        /// <param name="inStream">The stream containing the XML data.</param>
        /// <returns>The deserialized object.</returns>
        public T Deserialize<T>(Stream inStream)
        {
            return (T)Deserialize(inStream);
        }
    }
}
