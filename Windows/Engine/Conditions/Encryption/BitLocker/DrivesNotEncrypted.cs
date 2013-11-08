using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Conditions.Encryption.BitLocker
{
    class DrivesNotEncrypted:DrivesEncrypted
    {
        public DrivesNotEncrypted(IEngine engine):base(engine)
        {
        }

        public override Results.Misc.BooleanReason Evaluate()
        {
            return base.Evaluate().Invert();
        }
    }
}
