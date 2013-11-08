using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Conditions.Certificates
{
    class CertificateThumbprintDoesNotExist:CertificateThumbprintExists
    {
        public CertificateThumbprintDoesNotExist(IEngine engine):base(engine)
        {
        }

        public override Results.Misc.BooleanReason Evaluate()
        {
            return base.Evaluate().Invert();
        }
    }
}
