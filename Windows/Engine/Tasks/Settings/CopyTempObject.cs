using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Settings
{
    class CopyTempObject:AbstractTask
    {
        
        [PropertyAllowedFromXml]
        public string SourceKey
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        [PropertyAllowedFromXml]
        public string TargetKey
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }
        
        public CopyTempObject(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SourceKey))
                    throw new Exception("Source key required");

                if (string.IsNullOrWhiteSpace(TargetKey))
                    throw new Exception("Target key required");

                var value = SettingsManager.GetTemporaryObject(SourceKey);
                if (value == null)
                    throw new Exception("Source object cannot be null");

                SettingsManager.SetTemporaryObject(TargetKey, value);
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Copy temporary settings object";
        }
    }
}
