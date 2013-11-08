using System;
using System.Collections.Generic;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Importables.PropertySetters;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.Control
{
    class PreserveArbitraryResult : AbstractTask
    {
        private readonly List<KeyedDynamicStringPropertyEntry> _propertySetters = new List<KeyedDynamicStringPropertyEntry>();

        [PropertyAllowedFromXml]
        public string Result
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ObjectKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public KeyedDynamicStringPropertyEntry PropertySetter
        {
            set
            {
                if (value == null)
                    return;

                _propertySetters.Add(value);
            }
        }

        public PreserveArbitraryResult(IEngine engine)
            : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ObjectKey))
                    throw new Exception("Object key is required");

/*
                var type = typeof(AbstractTaskResult);
                var typeName = ReflectionUtilities.ResolveTypeForName<AbstractTaskResult>(type.Namespace + "." + Result);
*/
                var taskResult = ReflectionUtilities.LoadFromAssembly<AbstractTaskResult>(Result);
                if (taskResult == null)
                    throw new Exception("Invalid result specified");

                foreach (var setter in _propertySetters)
                    ReflectionUtilities.SetStringPropertyValue(taskResult, setter.Key, setter.Value);
                
                SettingsManager.SetTemporaryObject(ObjectKey, taskResult);

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Preserve arbitrary result";
        }
    }
}
