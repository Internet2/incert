using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Assistants;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Converters;
using log4net;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels
{
    class PassphraseContentModel : AbstractContentModel
    {
        private readonly ISettingsManager _manager;
        private static readonly ILog Log = Logger.Create();
        private string _watermark;

        public PassphraseContentModel(AbstractModel parentModel, ISettingsManager manager)
            : base(parentModel)
        {
            _manager = manager;
        }

        public string Watermark
        {
            get { return _watermark; }
            set { _watermark = value; OnPropertyChanged(); }
        }

        public override T LoadContent<T>(AbstractContentWrapper wrapper)
        {
            try
            {
                var result = new PasswordBox();
                InitializeBindings(result);
                InitializeSettingBinding(result, wrapper);
                InitializeValues(wrapper as PasswordInputField, result);
                Content = result;
                return result as T;
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return default(T);
            }
        }


        protected void InitializeValues(PasswordInputField wrapper, PasswordBox target)
        {
            base.InitializeValues(wrapper);
            TextBrush = AppearanceManager.GetBrushForColor(wrapper.Color, AppearanceManager.InputFieldTextBrush);
            Padding = wrapper.Padding.GetValueOrDefault(new Thickness(4));
            target.Password = _manager.GetTemporarySettingString(wrapper.SettingKey);
            Watermark = wrapper.Watermark;
        }

        private void InitializeSettingBinding(FrameworkElement instance, AbstractContentWrapper wrapper)
        {
            if (instance == null)
                return;

            instance.Style = GetNamedStyle("PhraseFieldStyle");

            instance.SetBinding(PasswordHelper.WatermarkProperty, GetOneWayBinding(this, "Watermark"));
            instance.SetBinding(PasswordHelper.PasswordProperty,
                new Binding
                    {
                        Converter = new SettingsConverter(this, _manager),
                        ConverterParameter = wrapper.SettingKey,
                        Mode = BindingMode.OneWayToSource,
                        Path = new PropertyPath("SettingProperty"),
                        Source = _manager.BindingProxy,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    });
        }
    }
}
