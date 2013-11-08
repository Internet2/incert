using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class CompleteCheckedParagraph : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        [PropertyAllowedFromXml]
        public string ControlKey
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

        public CompleteCheckedParagraph(IEngine engine) :
            base(engine)
        {

        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var dialog = DialogsManager.GetExistingDialog(Dialog);
                if (dialog == null)
                {
                    Log.WarnFormat("Unable to retrieve dialog for key {0}", Dialog);
                    return new NextResult();
                }

                var model = dialog.ContentModel.FindChildModel<CheckedParagraphModel>(ControlKey);
                if (model == null)
                {
                    Log.WarnFormat("Could not find CheckedParagraphModel for key {0}", ControlKey);
                    return new NextResult();
                }

                model.State = CheckedParagraphModel.CheckedParagraphState.Complete;

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }

        }

        public override string GetFriendlyName()
        {
            return "Complete checked paragraph";
        }

    }
}
