using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models;

namespace Org.InCommon.InCert.Engine.Settings
{
    public class BindingSettingsProxy: INotifyPropertyChanged
    {
        private readonly ISettingsManager _manager;
        public event PropertyChangedEventHandler PropertyChanged;

        public BindingSettingsProxy(ISettingsManager manager)
        {
            _manager = manager;
        }

        public StringSettingWrapper SettingProperty
        {
            get
            {
                return null;
            }
            set
            {
                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Normal,
                    new Action(() => SetValue(value)));
            }
        }

        private void SetValue(StringSettingWrapper wrapper)
        {
            if (wrapper == null)
                return;

            _manager.SetTemporarySettingString(wrapper.GetKey(), wrapper.GetValue());
            InvokeActions(wrapper.Model);
            OnPropertyChanged("SettingProperty");
        }

        private static void InvokeActions(AbstractModel model)
        {
            if (model == null)
                return;

            if (model.RootDialogModel == null)
                return;

            model.RootDialogModel.DoActions(false);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
