using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Org.InCommon.InCert.Engine.AdvancedMenu;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;

namespace Org.InCommon.InCert.Engine.Dynamics
{
    [DataContract]
    [KnownType(typeof(AdvancedMenuItem))]
    public class AbstractDynamicPropertyContainer : AbstractImportable
    {
        private readonly Dictionary<string, string> _dynamicProperties = new Dictionary<string, string>();

        protected AbstractDynamicPropertyContainer(IEngine engine)
            : base(engine)
        {
        }

        protected void SetDynamicValue(string value, [CallerMemberName] string key = "")
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            _dynamicProperties[key] = value;
        }

        protected string GetRawValue([CallerMemberName] string key = "")
        {
            if (string.IsNullOrWhiteSpace(key))
                return "";

            return !_dynamicProperties.ContainsKey(key)
                       ? ""
                       : _dynamicProperties[key];
        }

        protected string GetDynamicValue([CallerMemberName] string key = "", bool resolveTokens = true)
        {
            if (string.IsNullOrWhiteSpace(key))
                return "";

            return !_dynamicProperties.ContainsKey(key)
                ? ""
                : ValueResolver.Resolve(_dynamicProperties[key], resolveTokens);
        }

        protected bool IsPropertySet(string key)
        {
            return _dynamicProperties.ContainsKey(key);
        }

        public static string ToJson<T>(IEnumerable<T> values) 
        {
            var serializer = new DataContractJsonSerializer(typeof(IEnumerable<T>));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, values);
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

      
    }

    
}
