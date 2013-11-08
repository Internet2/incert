using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Org.InCommon.InCert.Engine.Settings.SerializableDictionaries
{
    /// <summary>
    /// Serializable dictionary class that allows timestamp entries to be persisted
    /// </summary>
    /// <see>http://stackoverflow.com/questions/922047/store-dictionarystring-string-in-application-settings</see>
    [Serializable]
    public class SerializableTimestampDictionary : Dictionary<String, DateTime>, IXmlSerializable
    {
        protected SerializableTimestampDictionary(SerializationInfo info, StreamingContext context):base(info,context)
        {

        }

        protected SerializableTimestampDictionary()
        {
            
        }
        
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
                if (string.IsNullOrWhiteSpace(value))
                    continue;

                long longValue;
                if (!long.TryParse(value, out longValue))
                    continue;
                
                this[name] = DateTime.FromFileTime(longValue);
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (var key in Keys)
            {
                writer.WriteStartElement("Pair");
                writer.WriteAttributeString("Name", key);
                writer.WriteAttributeString("Value", this[key].ToFileTime().ToString(CultureInfo.InvariantCulture));
                writer.WriteEndElement();
            }
        }

        public override string ToString()
        {
            var serializer = new XmlSerializer(typeof(SerializableTimestampDictionary));
            var buffer = new StringBuilder();
            using (var writer = new StringWriter(buffer))
            {
                serializer.Serialize(writer, this);
            }

            return buffer.ToString();
        }

        public static SerializableTimestampDictionary LoadFromString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new SerializableTimestampDictionary();

            var serializer = new XmlSerializer(typeof(SerializableTimestampDictionary));
            using (var reader = new StringReader(value))
            {
                return serializer.Deserialize(reader) as SerializableTimestampDictionary;
            }
        }
    }
}
