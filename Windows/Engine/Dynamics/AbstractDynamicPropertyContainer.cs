using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;

namespace Org.InCommon.InCert.Engine.Dynamics
{
    public class AbstractDynamicPropertyContainer : AbstractImportable
    {
        private readonly Dictionary<string, string> _dynamicProperties = new Dictionary<string, string>();

        public AbstractDynamicPropertyContainer(IEngine engine)
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
    }
}
