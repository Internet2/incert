using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Conditions.UserInterface
{
    class InvalidColorString : ValidColorString
    {
        public InvalidColorString(IEngine engine)
            : base(engine)
        {
        }

        public override Results.Misc.BooleanReason Evaluate()
        {
            return base.Evaluate().Invert();
        }
    }
}
