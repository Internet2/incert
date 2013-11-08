using System;
using System.Collections.Generic;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Importables.PropertySetters;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.RegistryTasks
{
    class SetStringValues:AbstractRegistryTask
    {
        private readonly List<KeyedDynamicStringPropertyEntry> _setters = new List<KeyedDynamicStringPropertyEntry>();
        
        public SetStringValues(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public KeyedDynamicStringPropertyEntry Setter
        {
            set
            {
                if (value == null)
                    return;

                _setters.Add(value);
            }
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                using (var key = RegistryRoot.OpenRegistryKey(RegistryKey, true))
                {
                    if (key == null)
                        throw  new Exception(string.Format("The registry key {0} {1} does not exist", RegistryRoot, RegistryKey));

                    foreach (var setter in _setters)
                        key.SetValue(setter.Key, setter.Value);
                }

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return string.Format("Set string registry values");
        }
    }
}
