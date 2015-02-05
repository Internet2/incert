using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.EventWrappers
{
    [DataContract]
    public abstract class AbstractEventData
    {
        public string ToJson()
        {
            var serializer = new DataContractJsonSerializer(GetType());
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, this);
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }


        }


    }

}
