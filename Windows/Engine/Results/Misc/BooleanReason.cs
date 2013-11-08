using System;

namespace Org.InCommon.InCert.Engine.Results.Misc
{
    public class BooleanReason
    {
        public bool Result { get; set; }
        public string Reason { get; set; }

        public BooleanReason(bool result, string reason)
        {
            Result = result;
            Reason = reason;
        }

        public BooleanReason(bool result, string reason, params object[] parameters)
        {
            Result = result;
            Reason = string.Format(reason, parameters);
        }

        public BooleanReason(Exception e)
        {
            Result = false;
            Reason = string.Format("An exception occurred while evaluating the condition: {0}", e.Message);
        }
        
        public BooleanReason Invert()
        {
            Result = !Result;
            return this;
        }

        public static BooleanReason ExceptionReason(Exception issue, string functionSummary)
        {
            return ExceptionReason(false, issue, functionSummary);
        }

        public static BooleanReason ExceptionReason(bool defaultResult, Exception issue, string functionSummary)
        {
            return new BooleanReason(defaultResult, "An exception occurred while " + functionSummary + ": " + issue.Message);
        }

    }
}
