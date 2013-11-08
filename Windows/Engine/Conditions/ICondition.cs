using Org.InCommon.InCert.Engine.Results.Misc;

namespace Org.InCommon.InCert.Engine.Conditions
{
    public interface ICondition
    {
        BooleanReason Evaluate();
        bool IsInitialized();
        string SkipText { get; set; }
        bool Cumulative { get; set; }
        bool CachePreviousResults { get; set; }
    }
}
