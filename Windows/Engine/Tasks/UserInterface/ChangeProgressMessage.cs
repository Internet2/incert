using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class ChangeProgressMessage:AbstractTask
    {
        private static readonly ILog Log = Logger.Create();
        
        [PropertyAllowedFromXml]
        public string SettingKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string Value
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public ChangeProgressMessage(IEngine engine)
            : base(engine)
        {
           
        }
        
        public override IResult Execute(IResult previousResults)
        {
            try
            {
                AppearanceManager.ChangeTimedMessage(SettingKey, Value);
                return new NextResult();
            }
            catch(Exception e)
            {
                Log.Warn(e);
                return new NextResult();
            }
        }

        public override string GetFriendlyName()
        {
            return "Change progress message";
        }
    }
}
