using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Win32;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;
using log4net;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances.CustomControls;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels
{
    class FileBrowseButtonModel:FramedButtonModel
    {
        private static readonly ILog Log = Logger.Create();

        public FileBrowseButtonModel(IEngine engine, AbstractModel parentModel) : base(engine, parentModel)
        {
        }

        private string _title;
        private string _filter;
        private string _settingKey;

        public override T LoadContent<T>(AbstractContentWrapper wrapper)
        {
            _settingKey = wrapper.SettingKey;
            var content = new FramedButtonControl
            {
                DataContext = this,
                Command = new RelayCommand(param => GetPathValue())
            };

            _filter = GetFilterFromWrapper(wrapper as FileBrowseButton);
            _title = GetTitleFromWrapper(wrapper as FileBrowseButton);
            SetDefaultValues(wrapper as FramedButton);
            InitializeValues(wrapper);
            Content = content;
            return content as T;
        }

        private static string GetFilterFromWrapper(FileBrowseButton wrapper)
        {
            return wrapper == null ? "" : wrapper.Filter;
        }

        private static string GetTitleFromWrapper(FileBrowseButton wrapper)
        {
            return wrapper == null ? "" : wrapper.Title; 
        }

        public void GetPathValue()
        {
            try
            {
                var dialog = new OpenFileDialog
                {
                    Multiselect = false, 
                    ValidateNames = true, 
                    DereferenceLinks = true, 
                    CheckFileExists = true, 
                    CheckPathExists = true
                };

                AssignFilterToDialog(dialog, _filter);

                if (!string.IsNullOrWhiteSpace(_title))
                    dialog.Title = _title;

                var info = GetFileInfoForKey(_settingKey);
                if (info != null)
                {
                    dialog.FileName = info.Name;
                    dialog.InitialDirectory = info.DirectoryName;
                }
           
                var result = dialog.ShowDialog(RootDialogModel.DialogInstance);
                if (result.GetValueOrDefault(false) == false)
                    return;

                foreach (var model in RootDialogModel.GetModelsBySettingKey(_settingKey).OfType<InputContentModel>().Select(instance => instance))
                {
                    model.SetText(dialog.FileName);
                }
            }
            finally
            {
                Content.ClearFocus();
            }
        }

        private static void AssignFilterToDialog(FileDialog dialog, string filter)
        {
            try
            {
                if (dialog == null)
                    return;

                if (string.IsNullOrWhiteSpace(filter))
                    return;

                dialog.Filter = filter;
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to assign the filter {0} to the open file dialog: {1}", filter, e.Message);
            }
        }

        private FileInfo GetFileInfoForKey(string key)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                    return null;

                if (!SettingsManager.IsTemporarySettingStringPresent(key))
                    return null;

                var path = SettingsManager.GetTemporarySettingString(key);
                return new FileInfo(path);

            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
