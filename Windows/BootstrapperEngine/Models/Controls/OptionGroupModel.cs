using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Xml.Linq;
using Org.InCommon.InCert.BootstrapperEngine.Extensions;
using Org.InCommon.InCert.BootstrapperEngine.Models.Pages;
using Org.InCommon.InCert.BootstrapperEngine.PropertyNotifiers;
using Org.InCommon.InCert.BootstrapperEngine.Views.Controls;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Controls
{
    public class OptionGroupModel : PropertyNotifyBase
    {
        public List<ExpandableOptionModel> ChildModels { get; private set; }
        public List<ExpandableOption> Children
        {
            get
            {
                if (ChildModels == null)
                    return null;

                return ChildModels.Select(e => e.Instance).ToList();
            }
        }

        public OptionGroup Instance { get; private set; }

        private readonly OptionPageModel _model;

        public OptionGroupModel(XElement node, OptionPageModel model)
        {
            _model = model;
            Instance = new OptionGroup { DataContext = this };

            GroupName = Guid.NewGuid().ToString();
            Title = node.ChildNodeValue("Title");

            ChildModels = new List<ExpandableOptionModel>();
            foreach (var entryNode in node.Elements("Entry"))
                ChildModels.Add(new ExpandableOptionModel(entryNode, this));
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Value"); }
        }

        public string GroupName { get; set; }

        public Brush Foreground
        {
            get { return _model.Foreground; }
        }

        public Brush Background
        {
            get { return _model.Background; }
        }

        public Brush LinkBrush
        {
            get { return _model.LinkBrush; }
        }

        public Brush TitleBackground
        {
            get { return _model.FrameBackground; }
        }

        public string SelectedValue
        {
            get
            {
                if (ChildModels == null)
                    return "";

                var selectedModel = ChildModels.FirstOrDefault(e => e.IsChecked);
                if (selectedModel == null)
                    return "";

                return selectedModel.Branch;
            }
        }

    }
}
