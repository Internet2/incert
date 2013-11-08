using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Text
{
    public class TruncateStringLeft:AbstractTask
    {
        private static readonly ILog Log = Logger.Create();
        
        public TruncateStringLeft(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Value
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        [PropertyAllowedFromXml]
        public string SettingKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public int Length { get; set; }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Value))
                    throw new Exception("Cannot truncate value; value is empty.");
                
                if (string.IsNullOrWhiteSpace(SettingKey))
                    throw  new Exception("Setting key cannot be empty");
                
                if (Length<=0)
                    throw new Exception("Length must be greater than zero");

                var newValue = Value;
                if (Length < Value.Length)
                    newValue = Value.Substring(0, Length);

                SettingsManager.SetTemporarySettingString(SettingKey, newValue);

                return new NextResult();

            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to truncate a string: {0}", e.Message);
                return new NextResult();
            }
        }

        public override string GetFriendlyName()
        {
            return "Truncate string left";
        }
    }
}
