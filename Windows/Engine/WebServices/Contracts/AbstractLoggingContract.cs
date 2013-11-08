using System;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.WebServices.Managers;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts
{
    public abstract class AbstractLoggingContract:AbstractContract
    {
        protected AbstractLoggingContract(IEndpointManager endpointManager) : base(endpointManager)
        {
        }

        public string Message { get; set; }
        public string User { get; set; }
        public Guid Session { get; set; }
        public string Event { get; set; }
        public string Machine { get; set; }

        public override IResult GetErrorResult()
        {
            return new ExceptionOccurred(new Exception("Could not upload log contents to server"));
        }


        protected override T ProcessResults<T>(T result)
        {
            return result ?? new T();
        }
        
    }
}
