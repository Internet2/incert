using System;
using System.Collections.Generic;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Importables.PropertySetters;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Settings;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class UpdateBoundText:AbstractTask
    {
       private readonly List<KeyedDynamicStringPropertyEntry> _setters = new List<KeyedDynamicStringPropertyEntry>();

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

        public UpdateBoundText(IEngine engine)
            : base(engine)
        {
            
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                foreach (var setter in _setters)
                {
                    SettingsManager.BindingProxy.SettingProperty = new StringSettingWrapper(setter.Key, setter.Value, null);
                    AppearanceManager.ChangeTimedMessage(setter.Key, setter.Value);
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
            return "Update user-interface bound text";
        }
    }
}
