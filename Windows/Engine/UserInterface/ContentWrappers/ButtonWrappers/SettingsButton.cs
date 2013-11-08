using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ButtonWrappers
{
    class SettingsButton:AbstractButton
    {
        public SettingsButton(IEngine engine):base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Key { get; set; }

        public override bool Initialized()
        {
            if (!base.Initialized())
                return false;

            if (string.IsNullOrWhiteSpace(Key))
                return false;

            return true;
        }



    }
}
