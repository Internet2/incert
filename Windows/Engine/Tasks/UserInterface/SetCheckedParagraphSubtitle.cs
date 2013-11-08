using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Ninject;
using Ninject.Parameters;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class SetCheckedParagraphSubtitle : AbstractTask
    {
        [PropertyAllowedFromXml]
        public string Dialog
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ControlKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        private static readonly ILog Log = Logger.Create();

        private AbstractContentWrapper _content;

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);

            var content = node.Element("Content");
            if (content == null)
                return;

            var paragraph = GetInstanceFromNode<AbstractContentWrapper>(content.Elements().FirstOrDefault());
            if (paragraph == null || !paragraph.Initialized())
                return;

            _content = paragraph;
        }

        public SetCheckedParagraphSubtitle(IEngine engine) :
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

                var modelType = _content.GetSupportingModelType();
                if (modelType == null)
                {
                    Log.WarnFormat("Could not get supporting content type from wrapper {0}", _content.GetType());
                    return new NextResult();
                }

                var contentModel = Application.Current.CurrentKernel().Get(
                    modelType, new ConstructorArgument("parentModel", model)) as AbstractModel;
                if (contentModel == null)
                {
                    Log.WarnFormat("Could not generate content model for content {0}", _content.GetType());
                    return new NextResult();
                }

                contentModel.LoadContent<FrameworkElement>(_content);
                BindingOperations.ClearBinding(contentModel.Content, TextBlock.ForegroundProperty);

                model.LowerContent = contentModel as AbstractContentModel;
                return new NextResult();

            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }
        
        public override string GetFriendlyName()
        {
            return "Set checked paragraph subtitle";
        }
    }
}
