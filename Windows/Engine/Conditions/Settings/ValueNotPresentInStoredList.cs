using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Conditions.Settings
{
    class ValueNotPresentInStoredList:ValuePresentInStoredList
    {
        public ValueNotPresentInStoredList(IEngine engine):base(engine)
        {
        }

        public override Results.Misc.BooleanReason Evaluate()
        {
            return base.Evaluate().Invert();
        }
    }
}
