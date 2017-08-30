using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ThinkSharp.FeatureTouring.Helper
{
    /// <summary>
    ///   XML serializer that can be used to (de)serialize variouse objects to / from xml, using the default <see cref="XmlSerializer"/>.
    ///   <threadsafety static="true" />
    /// </summary>
    public static class GenericXmlSerializer
    {
        private static readonly Dictionary<Type, XmlSerializer> theSerializers = new Dictionary<Type, XmlSerializer>();

        #region Deserialize

        /// <summary>
        ///   Deserializes the xml string using the default serializer for the specified type.
        /// </summary>
        /// <param name="xmlString">
        ///   The xml serialized object to deserialize.
        /// </param>
        /// <typeparam name="TItem">
        ///   The type of the object to deserialize.
        /// </typeparam>
        /// <returns>
        ///   The deserialized object.
        /// </returns>
        public static TItem Deserialize<TItem>(String xmlString)
        {
            XmlSerializer serializer = GetXmlSerializer<TItem>();

            using (StringReader reader = new StringReader(xmlString))
            {
                return (TItem)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        ///   Deserializes the xml using the default serializer for the specified type.
        /// </summary>
        /// <param name="xmlStream">
        ///   The xml stream that points to the serialized object to deserialize.
        /// </param>
        /// <typeparam name="TItem">
        ///   The type of the object to deserialize.
        /// </typeparam>
        /// <returns>
        ///   The deserialized object.
        /// </returns>
        public static TItem Deserialize<TItem>(Stream xmlStream)
        {
            XmlSerializer serializer = GetXmlSerializer<TItem>();

            using (var reader = new StreamReader(xmlStream))
            {
                return (TItem)serializer.Deserialize(reader);
            }
        }

        #endregion

        #region GetXmlSerializer

        /// <summary>
        ///   Gets the XmlSerializer for the specified type.
        /// </summary>
        /// <typeparam name="TItem">
        ///   The type to get the xml serializer for.
        /// </typeparam>
        /// <returns>
        ///   The XmlSerializer for the specified type.
        /// </returns>
        private static XmlSerializer GetXmlSerializer<TItem>()
        {
            return GetXmlSerializer(typeof(TItem));
        }

        /// <summary>
        ///   Gets the XmlSerializer for the specified type.
        /// </summary>
        /// <returns>
        ///   The XmlSerializer for the specified type.
        /// </returns>
        private static XmlSerializer GetXmlSerializer(Type type)
        {
            lock (theSerializers)
            {
                XmlSerializer serializer;
                if (!theSerializers.TryGetValue(type, out serializer))
                {
                    serializer = new XmlSerializer(type, String.Empty);
                    theSerializers.Add(type, serializer);
                }

                return serializer;
            }
        }

        #endregion

        #region Serialize

        /// <summary>
        ///   Serializes the specified object to XML using the default serializer for the specified type.
        /// </summary>
        /// <param name="obj">
        ///   The object to serialize.
        /// </param>
        /// <typeparam name="TItem">
        ///   The type of the object to serialize.
        /// </typeparam>
        /// <returns>
        ///   The xml serialized object.
        /// </returns>
        public static String Serialize<TItem>(TItem obj)
        {
            return Serialize(obj, typeof(TItem));
        }

        /// <summary>
        ///   Serializes the specified object to XML using the default serializer for the specified type.
        /// </summary>
        /// <param name="obj">
        ///   The object to serialize.
        /// </param>
        /// <returns>
        ///   The xml serialized object.
        /// </returns>
        public static String Serialize(Object obj, Type type)
        {
            XmlSerializer serializer = GetXmlSerializer(type);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            using (StringWriter writer = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
                {
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add(String.Empty, String.Empty);

                    serializer.Serialize(xmlWriter, obj, ns);
                    return writer.ToString();
                }
            }
        }

        /// <summary>
        ///   Serializes the specified object to XML using the default serializer for the specified type.
        /// </summary>
        /// <param name="obj">
        ///   The object to serialize.
        /// </param>
        /// <typeparam name="TItem">
        ///   The type of the object to serialize.
        /// </typeparam>
        /// <param name="stream">
        ///   The stream to write the serialized data in.
        /// </param>
        public static void Serialize<TItem>(TItem obj, Stream stream)
        {
            XmlSerializer serializer = GetXmlSerializer<TItem>();

            serializer.Serialize(stream, obj);
        }

        #endregion
    }
}
