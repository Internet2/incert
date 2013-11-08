using System;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.Authentication;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Authentication
{
    class VerifyLocalPasswordSet : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        public VerifyLocalPasswordSet(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (!SystemUtilities.IsCurrentUserLocalAccount())
                {
                    Log.InfoFormat("Current user account {0} is not a local account; assuming password set.",
                                   SystemUtilities.GetCurrentUserUsernameAndDomain());
                    return new NextResult();
                }

                var result = SystemUtilities.AuthenticateUser(SystemUtilities.GetCurrentUserUsername(), ".", null);

                Log.DebugFormat("Authenticate with blank password result is {0}", result);

                // 1327 is returned if no password is set for a username -- you can't logon as batch to an account
                // if not password is set, so you get 1327 - account restriction issue if you try
                if (result == 1327)
                {
                    Log.WarnFormat("Result {0} indicates that user has no admin password set", result);
                    return new LocalAdminPasswordNotSet
                        {
                            Username = SystemUtilities.GetCurrentUserUsername(),
                            DisplayName = SystemUtilities.GetDisplayNameForAccount(SystemUtilities.GetCurrentUserUsername())
                        };
                }

                return new NextResult();

            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }

        }

        public override string GetFriendlyName()
        {
            return "Verify local password set";
        }
    }
}
