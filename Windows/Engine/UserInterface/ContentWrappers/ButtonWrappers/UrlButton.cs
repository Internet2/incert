using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ButtonWrappers
{
    class UrlButton : AbstractButtonWrapper
    {
        public UrlButton(IEngine engine):base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Value { get; set; }

        public override bool Initialized()
        {
            if (!base.Initialized())
                return false;

            return !string.IsNullOrWhiteSpace(Value);
        }

    }


}
