using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class StopMessageTimer:AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        [PropertyAllowedFromXml]
        public string SettingKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public StopMessageTimer(IEngine engine)
            : base(engine)
        {
        
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                AppearanceManager.DeactiveTimedMessage(SettingKey);
                return new NextResult();
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to stop a timed message : {0}", e.Message);
                return new NextResult();
            }
            
        }

        public override string GetFriendlyName()
        {
            return "Stop message timer";
        }
    }
}
