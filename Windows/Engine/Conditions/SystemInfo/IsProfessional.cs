using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.SystemInfo
{
    class IsProfessional:AbstractCondition
    {
        public IsProfessional(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            return !SystemUtilities.IsProfessional() 
                ? new BooleanReason(false, "this is not a professional grade operating system") :
                new BooleanReason(true, "this is a professional grade operating system");
        }

        public override bool IsInitialized()
        {
            return true;
        }
    }
}
