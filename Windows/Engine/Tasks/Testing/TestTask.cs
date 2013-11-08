using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;

namespace Org.InCommon.InCert.Engine.Tasks.Testing
{
    public class TestTask:AbstractTask
    {
        public TestTask(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            return new NextResult();
        }

        public override string GetFriendlyName()
        {
            return "testing";
        }
    }
}
