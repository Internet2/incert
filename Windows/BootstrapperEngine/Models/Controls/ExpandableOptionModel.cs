using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using Org.InCommon.InCert.BootstrapperEngine.Commands;
using Org.InCommon.InCert.BootstrapperEngine.Extensions;
using Org.InCommon.InCert.BootstrapperEngine.Logging;
using Org.InCommon.InCert.BootstrapperEngine.PropertyNotifiers;
using Org.InCommon.InCert.BootstrapperEngine.Views.Controls;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Controls
{
    public class ExpandableOptionModel:PropertyNotifyBase
    {
        private readonly OptionGroupModel _model;

        private string _title;
        private string _description;
        private bool _isChecked;
        private string _detailsLinkText;
        private string _detailsUrl;
        private ICommand _detailsCommand;

        public string Key { get; set; }

        public ExpandableOption Instance { get; private set; }

        public ExpandableOptionModel(XElement node, OptionGroupModel model)
        {
            Instance = new ExpandableOption {DataContext = this};
            _model = model;
            Title = node.ChildNodeValue("Title");
            Description = node.ChildNodeValue("Description");
            IsChecked = node.AttributeValue("checked", false);
            DetailsLinkText = node.ChildNodeValue("DetailsText", "more details");
            Branch = node.ChildNodeValue("SettingKey");
            _detailsUrl = node.ChildNodeValue("DetailsUrl");
        }

        public string DetailsLinkText
        {
            get { return _detailsLinkText; }
            set { _detailsLinkText = value; OnPropertyChanged("DetailsLinkText"); }
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; OnPropertyChanged("IsChecked"); OnPropertyChanged("Foreground"); OnPropertyChanged("BorderBrush"); }
        }
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged("Description"); }
        }

        public string GroupName
        {
            get { return _model.GroupName; }
        }
     
        public Brush Foreground
        {
            get
            {
                return _model.Foreground;
            }
        }

        public Brush LinkBrush
        {
            get { return _model.TitleBackground; }
        }
        
        public Brush Background
        {
            get { return _model.Background; }
        }

        public string Branch { get; private set; }

        public ICommand DetailsCommand
        {
            get {
                return _detailsCommand ??
                       (_detailsCommand =
                        new RelayCommand(a => OpenDetailsPage(), ok => !string.IsNullOrWhiteSpace(_detailsUrl)));
            }
        }

        private void OpenDetailsPage()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_detailsUrl))
                    return;

                var info = new ProcessStartInfo(_detailsUrl) { UseShellExecute = true };
                Process.Start(info);
            }
            catch (Exception e)
            {
                Logger.Error("An issue occurred while attempting to launch the help topic", e);
            }

        }
    }
}
