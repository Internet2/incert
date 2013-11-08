using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;

namespace Org.InCommon.InCert.Engine.Conditions.UserInterface
{
    class TimedMessageDoesNotExistCondition : TimedMessageExists
    {
        public TimedMessageDoesNotExistCondition(IEngine engine) :
            base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            return base.Evaluate().Invert();
        }
    }
}
