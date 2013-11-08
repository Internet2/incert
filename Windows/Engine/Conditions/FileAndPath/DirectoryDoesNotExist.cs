using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Conditions.FileAndPath
{
    class DirectoryDoesNotExist:DirectoryExists
    {
        public DirectoryDoesNotExist(IEngine engine):base(engine)
        {
        }

        public override Results.Misc.BooleanReason Evaluate()
        {
            return base.Evaluate().Invert();
        }
    }
}
