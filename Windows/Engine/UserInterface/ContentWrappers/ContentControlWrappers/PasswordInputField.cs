using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{
    class PasswordInputField : AbstractContentWrapper
    {
        public PasswordInputField(IEngine engine)
            : base(engine)
        {
        }

        public override System.Type GetSupportingModelType()
        {
            return typeof (PassphraseContentModel);
        }

        public override string GetDefaultStyle()
        {
            return "PasswordField";
        }

    }
}
