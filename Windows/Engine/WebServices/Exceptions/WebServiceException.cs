using System;

namespace Org.InCommon.InCert.Engine.WebServices.Exceptions
{
    [Serializable]
    class WebServiceException:Exception
    {
        public WebServiceException(string message) : base(message)
        {
            
        }

        public WebServiceException(Exception exception) : base(exception.Message)
        {
            
        }
    }
}
