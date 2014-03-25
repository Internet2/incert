using System;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class SetApplicationProperties:AbstractTask
    {

        public SetApplicationProperties(IEngine engine) : base (engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Title
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        [PropertyAllowedFromXml]
        public string Institution
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Institution))
                    AppearanceManager.ApplicationCompany = Institution;

                if (!string.IsNullOrWhiteSpace(Title))
                    AppearanceManager.ApplicationTitle = Title;

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return string.Format(
                "Set application properties (title = {0}, institution= {1})",
                Title.ToStringOrDefault("[not specified]"),
                Institution.ToStringOrDefault("[not specified]"));
        }
    }
}
