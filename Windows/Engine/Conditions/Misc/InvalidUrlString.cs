using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Conditions.Misc
{
    class InvalidUrlString:ValidUrlString
    {
        public InvalidUrlString(IEngine engine):base(engine)
        {
        }

        public override Results.Misc.BooleanReason Evaluate()
        {
            var result= base.Evaluate();
            
            return result;
        }
    }
}
