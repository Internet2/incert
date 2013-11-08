using System;
using System.Collections.Generic;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.UserInterface;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class DisableAllBannerDialogControls : AbstractTask
    {
        private readonly List<string> _exclude = new List<string>();

        [PropertyAllowedFromXml]
        public string Dialog
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ExcludeControlKey
        {
            set
            {
                _exclude.Add(value);
            }
        }

        public DisableAllBannerDialogControls(IEngine engine) :
            base(engine)
        {
        }


        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var dialog = DialogsManager.GetExistingDialog(Dialog);
                if (dialog == null)
                    return new DialogInstanceNotFound { Dialog = Dialog };

                dialog.EnableDisableAllControls(_exclude, false);

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Disable banner dialog";
        }
    }
}
