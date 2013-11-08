using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Importables;
using RestSharp;
using RestSharp.Deserializers;

namespace Org.InCommon.InCert.Engine.WebServices.Deserializers
{
    class ImportableDeserializer : IDeserializer
    {
        public T Deserialize<T>(IRestResponse response)
        {
            var result = Activator.CreateInstance<T>();
            var importable = result as AbstractImportable;
            if (importable == null)
                return result;

            using (var stream = new MemoryStream(response.RawBytes))
            {
                var reader = new XmlTextReader(stream);
                var document = XDocument.Load(reader);
                importable.ConfigureFromNode(document.Root);
            }

            return result;
        }

        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }
    }
}
