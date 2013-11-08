using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    public class StartMessageTimer:AbstractTask
    {
        [PropertyAllowedFromXml]
        public string SettingKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public StartMessageTimer(IEngine engine)
            : base(engine)
        {
            
        }

        public override IResult Execute(IResult previousResults)
        {
            AppearanceManager.ActivateTimedMessage(SettingKey);
           
            return new NextResult();
        }

        public override string GetFriendlyName()
        {
            return "Start message timer";
        }
    }
}
