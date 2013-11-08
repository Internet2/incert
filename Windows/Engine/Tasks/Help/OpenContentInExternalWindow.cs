using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Help
{
    class OpenContentInExternalWindow:AbstractTask
    {
      
        public OpenContentInExternalWindow(IEngine engine) : base (engine)
        {
      
        }

        public override IResult Execute(IResult previousResults)
        {
            HelpManager.OpenCurrentViewInExternalWindow();
            return new NextResult();
        }

        public override string GetFriendlyName()
        {
            return "Open current help content in external window";
        }
    }
}
