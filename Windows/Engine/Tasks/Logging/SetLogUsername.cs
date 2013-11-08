using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Logging
{
    class SetLogUsername:AbstractTask
    {
       
        [PropertyAllowedFromXml]
        public string Username
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        public  SetLogUsername(IEngine engine) : base(engine)
        {
        }
        
        public override IResult Execute(IResult previousResults)
        {
            var value = Username;
            if (string.IsNullOrWhiteSpace(value))
                value = "[Unknown]";

            // value can't contain spaces
            value = value.Replace(" ", "");

            ThreadContext.Properties["UserId"] = value;
            return new NextResult();
        }

        public override string GetFriendlyName()
        {
            return "Set log username";
        }
    }
}
