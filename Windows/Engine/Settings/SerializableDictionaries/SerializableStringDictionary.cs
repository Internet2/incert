using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Org.InCommon.InCert.Engine.Settings.SerializableDictionaries
{
    /// <summary>
    /// Serializable dictionary class that allows settings entries to be persisted
    /// </summary>
    /// <see>http://stackoverflow.com/questions/922047/store-dictionarystring-string-in-application-settings</see>
    public class SerializableStringDictionary : StringDictionary, IXmlSerializable
    {
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            while (reader.Read() &&
                   !(reader.NodeType == XmlNodeType.EndElement && reader.LocalName == GetType().Name))
            {
                var name = reader["Name"];
                if (name == null)
                    throw new FormatException();

                var value = reader["Value"];
                this[name] = value;
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (var key in Keys)
            {
                writer.WriteStartElement("Pair");
                writer.WriteAttributeString("Name", (string)key);
                writer.WriteAttributeString("Value", this[(string)key]);
                writer.WriteEndElement();
            }
        }

        public override string ToString()
        {
            var serializer = new XmlSerializer(typeof(SerializableStringDictionary));
            var buffer = new StringBuilder();
            using (var writer = new StringWriter(buffer))
            {
                serializer.Serialize(writer, this);
            }

            return buffer.ToString();
        }

        public static SerializableStringDictionary LoadFromString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new SerializableStringDictionary();

            var serializer = new XmlSerializer(typeof(SerializableStringDictionary));
            using (var reader = new StringReader(value))
            {
                return serializer.Deserialize(reader) as SerializableStringDictionary;
            }
        }
    }
}