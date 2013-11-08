using System;

namespace Org.InCommon.InCert.Engine.WebServices.Exceptions
{
    [Serializable]
    class AuthenticateUserRequestException:WebServiceException
    {
        public AuthenticateUserRequestException(string message) : base(message)
        {
        }

        public AuthenticateUserRequestException(Exception exception) : base(exception)
        {
        }
    }
}
