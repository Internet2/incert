using System;
using System.Reflection;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Verification;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Verification
{
    /// <summary>
    /// Verifies whether the executing .exe is signed 
    /// </summary>
    class VerifyApplicationSignature : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        public VerifyApplicationSignature(IEngine engine)
            : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var result = CertificateUtilities.IsFileSigned(Assembly.GetExecutingAssembly().Location);
                if (!result.Result)
                {
                    Log.WarnFormat("Cannot verifiy application signature: {0}", result.Result);
                    return new CouldNotVerifyApplicationSignature();
                }

                return new NextResult();
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return new ExceptionOccurred(e);
            }

        }

        public override string GetFriendlyName()
        {
            return "Verify application signature";
        }
    }
}
