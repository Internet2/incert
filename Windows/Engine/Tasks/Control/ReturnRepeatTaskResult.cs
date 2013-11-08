using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Control
{
    class ReturnRepeatTaskResult:AbstractTask
    {
        public ReturnRepeatTaskResult(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            return new RepeatCurrentTaskResult();
        }

        public override string GetFriendlyName()
        {
            return "Return repeat current task result";
        }
    }
}
