using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.UserInterface;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class ChangeBanner : AbstractTask
    {

        [PropertyAllowedFromXml]
        public string Banner
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string Dialog
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public ChangeBanner(IEngine engine) :
            base(engine)
        {

        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var dialog = DialogsManager.GetExistingDialog(Dialog);
                return dialog == null ? new DialogInstanceNotFound { Dialog = Dialog } :
                    dialog.ShowBanner(Banner);

            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return string.Format("Change banner ({0})", Banner);
        }
    }
}
