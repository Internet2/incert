using System;
using System.Collections.Generic;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Importables.PropertySetters;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.Control
{
    class ReturnErrorResult:AbstractTask
    {
        private readonly List<KeyedDynamicStringPropertyEntry> _propertySetters = new List<KeyedDynamicStringPropertyEntry>();

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

        [PropertyAllowedFromXml]
        public string Result
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }
        
        public ReturnErrorResult(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var result = ErrorResult.FromTypeName(Result);
                if (result ==null)
                    throw new Exception(string.Format("Could not instatiate error result for type {0}", Result));

                foreach (var setter in _propertySetters)
                    ReflectionUtilities.SetStringPropertyValue(result, setter.Key, setter.Value);

                return result;
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Return error result";
        }
    }
}
