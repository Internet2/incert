using System.Linq;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Conditions.Grouping
{
    public class AnyTrue:AbstractGroupCondition
    {
        public AnyTrue(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            if (!Conditions.Any())
                return new BooleanReason(false, "No conditions are present, so none can be true!");

            foreach (var condition in Conditions)
            {
                var result = condition.Evaluate();
                if (result.Result == false)
                {
                    LogFormat("{0} condition failed: {1}", condition.GetType().Name, result.Reason);
                    continue;
                }

                LogFormat("{0} condition passed: {1}", condition.GetType().Name, result.Reason);
                return result;
            }

            return new BooleanReason(false, "no conditions were met");
        }
    }
}
