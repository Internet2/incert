using System.IO;
using System.Runtime.Serialization.Json;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.EventWrappers
{
    public abstract class AbstractEventData
    {
        public string ToJson()
        {
            var serializer = new DataContractJsonSerializer(GetType());
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, this);
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
