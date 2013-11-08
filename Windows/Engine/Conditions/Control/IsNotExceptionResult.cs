using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Conditions.Control
{
    class IsNotExceptionResult: IsExceptionResult
    {
        public IsNotExceptionResult(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            var result =  base.Evaluate();
            
            return result;
        }
    }
}
