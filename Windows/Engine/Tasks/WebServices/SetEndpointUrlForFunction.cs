using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.WebServices.Managers;

namespace Org.InCommon.InCert.Engine.Tasks.WebServices
{
    internal class SetEndpointUrlForFunction : AbstractTask
    {
        public SetEndpointUrlForFunction(IEngine engine) : base(engine)
        {
            
        }

        [PropertyAllowedFromXml]
        public string Url
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public EndPointFunctions Function { get; set; }
    
        public override IResult Execute(IResult previousResults)
        {
            EndpointManager.SetEndpointForFunction(Function, Url);
            return new NextResult();
        }

        public override string GetFriendlyName()
        {
            return "Set endpoint url for function";
        }
    }
}
