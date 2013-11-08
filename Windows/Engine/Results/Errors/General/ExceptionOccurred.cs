using System;
using Org.InCommon.InCert.Engine.Extensions;

namespace Org.InCommon.InCert.Engine.Results.Errors.General
{
    class ExceptionOccurred:ErrorResult
    {
        [ResultExtensions.IncludeInErrorDetails]
        public Exception Exception { get; private set; }
        
        public ExceptionOccurred()
        {
        }

        public override string ErrorName
        {
            get
            {
                return Exception == null ? base.ErrorName : Exception.GetType().Name;
            }
        }
        public ExceptionOccurred(Exception issue)
        {
            Exception = issue;
            Issue = issue.Message;
        }


    }
}
