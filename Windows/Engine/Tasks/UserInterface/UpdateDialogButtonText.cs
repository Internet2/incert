using System;
using System.Collections.Generic;
using System.Reflection;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Importables.PropertySetters;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class UpdateDialogButtonText:AbstractTask
    {
        private readonly List<KeyedDynamicStringPropertyEntry> _setters = new List<KeyedDynamicStringPropertyEntry>();
        public UpdateDialogButtonText(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Dialog
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public KeyedDynamicStringPropertyEntry Setter
        {
            set
            {
                if (value == null)
                    return;

                _setters.Add(value);
            }
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var dialog = DialogsManager.GetExistingDialog(Dialog);
                if (dialog == null)
                    throw new Exception(string.Format("Could not retrieve dialog for key {0}", Dialog));
                
                foreach (var setter in _setters)
                    SetButtonText(dialog, setter.Key, setter.Value);
                
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Set dialog button text";
        }

        private void SetButtonText(AbstractDialogModel model, string key, string value)
        {
            var methodName = string.Format("set{0}text", key.ToLowerInvariant());
            var methodInfo = GetType()
                .GetMethod(methodName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic);

            if (methodInfo == null)
                throw new Exception(string.Format("Could not find setter method for {0}", key));

            if (!ReflectionUtilities.IsMethodAllowedFromXml(methodInfo))
                throw new Exception(string.Format("Method {0} not callable from xml", key));

            methodInfo.Invoke(this, new object[] {model, value });
        }

        [MethodAllowedFromXml]
        internal void SetNextButtonText(AbstractDialogModel model, string value)
        {
            model.NextModel.Text = value;
        }

        [MethodAllowedFromXml]
        internal void SetBackButtonText(AbstractDialogModel model, string value)
        {
            model.BackModel.Text = value;
        }

        [MethodAllowedFromXml]
        internal void SetHelpButtonText(AbstractDialogModel model, string value)
        {
            model.HelpModel.Text = value;
        }

        [MethodAllowedFromXml]
        internal void SetAdvancedButtonText(AbstractDialogModel model, string value)
        {
            model.AdvancedModel.Text = value;
        }


    }
}
