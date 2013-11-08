using System;
using Org.InCommon.InCert.Engine.Extensions;

namespace Org.InCommon.InCert.Engine.Results.Errors.Certificates
{
    class UserCertWillExpire : ErrorResult
    {
        public UserCertWillExpire()
        {

        }

        [ResultExtensions.IncludeInErrorDetails]
        public string Estimate { get; set; }

        public UserCertWillExpire(TimeSpan? timespan)
        {
            if (!timespan.HasValue)
                return;

            Estimate = timespan.Value.Duration().TotalDays <= 1 ? "in less than one day"
                : string.Format("in less than {0} days", timespan.Value.Duration().TotalDays);
        }
    }
}
