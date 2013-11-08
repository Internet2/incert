using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Conditions.Grouping
{
    public class AllTrue : AbstractGroupCondition
    {
        public AllTrue(IEngine engine)
            : base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            if (Conditions.Count == 0)
                return new BooleanReason(false, "No conditions are present, so none can be true.");

            foreach (var condition in Conditions)
            {
                var result = condition.Evaluate();
                if (!result.Result)
                {
                    LogFormat("{0} condition failed: {1}", condition.GetType().Name, result.Reason);
                    return new BooleanReason(false, "{0} condition failed: {1}", condition.GetType().Name, result.Reason);
                }
                
                LogFormat("{0} condition passed: {1}", condition.GetType().Name, result.Reason);
            }
               
            return new BooleanReason(true, "All conditions satisfied");
        }
    }
}
