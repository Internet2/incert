using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Conditions.Certificates
{
    class NoValidCertsExistForEmail:ValidCertExistsForEmail
    {
        public NoValidCertsExistForEmail(IEngine engine):base(engine)
        {
        }

        public override Results.Misc.BooleanReason Evaluate()
        {
            return  base.Evaluate().Invert();
        }
    }
}
