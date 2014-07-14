using System.Linq;
using System.Windows.Forms;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;
using Org.InCommon.InCert.Engine.Utilities;
using Button = System.Windows.Controls.Button;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels
{
    class FolderBrowseButtonModel:FramedButtonModel
    {
        public FolderBrowseButtonModel(IEngine engine, AbstractModel parentModel) : base(engine, parentModel)
        {
        }

        private string _description;
        private string _settingKey;

        public override T LoadContent<T>(AbstractContentWrapper wrapper)
        {
            _settingKey = wrapper.SettingKey;
            var content = new Button
            {
                Foreground = TextBrush,
                Command = new RelayCommand(param => GetPathValue())
            };

            _description = GetDescriptionFromWrapper(wrapper as FolderBrowseButton);
            SetDefaultValues(wrapper as FramedButton);
            InitializeValues(wrapper);
            Content = content;
            return content as T;
        }

        private static string GetDescriptionFromWrapper(FolderBrowseButton wrapper)
        {
            return wrapper == null ? "" : wrapper.Description;
        }

        public virtual void GetPathValue()
        {
            var dialog = new FolderBrowserDialog {Description = _description};
            var currentPathValue = SettingsManager.GetTemporarySettingString(_settingKey);
            if (string.IsNullOrWhiteSpace(currentPathValue))
                currentPathValue = PathUtilities.DesktopFolder;

            dialog.SelectedPath = currentPathValue;
            var result = dialog.ShowDialog(new UserInterfaceUtilities.WindowsHandleWrapper(RootDialogModel.DialogInstance));
            if (result != DialogResult.OK)
                return;

            foreach (var model in RootDialogModel.GetModelsBySettingKey(_settingKey).OfType<InputContentModel>().Select(instance => instance))
            {
                model.SetText(dialog.SelectedPath);
            }

            //var wrapper = new StringSettingWrapper(_settingKey, dialog.SelectedPath, this);
            //SettingsManager.BindingProxy.SettingProperty = wrapper;
        }
    }
}
