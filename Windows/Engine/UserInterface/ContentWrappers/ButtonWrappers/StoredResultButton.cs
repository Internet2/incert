using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ButtonWrappers
{
    class StoredResultButton : AbstractButton
    {
        public StoredResultButton(IEngine engine):base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Key { get; set; }

        public override bool Initialized()
        {
            if (!base.Initialized())
                return false;

            return !string.IsNullOrWhiteSpace(Key);
        }
    }
}
