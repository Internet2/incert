using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Org.InCommon.InCert.Engine.Extensions
{
    static class XmlExtensions
    {
        /// <summary>
        /// Converts serializable object to XElement
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="instance">object in question</param>
        /// <returns>XElement version of serialize object</returns>
        /// <remarks>adapted from http://stackoverflow.com/questions/8373552/serialize-an-object-to-xelement-and-deserialize-it-in-memory</remarks>
        public static XElement ToXElement<T>(this object instance)
        {
            var stream = new MemoryStream();
            using (TextWriter writer = new StreamWriter(stream))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, instance);
                return XElement.Parse(Encoding.ASCII.GetString(stream.ToArray()));
            }

        }
    }
}
