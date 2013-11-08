using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.WebServices
{
    class SetDefaultEndpointUrl:AbstractTask
    {
        public SetDefaultEndpointUrl(IEngine engine) : base(engine)
        {
            
        }

        [PropertyAllowedFromXml]
        public string Value
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }
        
        public override IResult Execute(IResult previousResults)
        {
            EndpointManager.SetDefaultEndpoint(Value);
            return new NextResult();
        }

        public override string GetFriendlyName()
        {
            return "Set default endpoint url";
        }
    }
}
