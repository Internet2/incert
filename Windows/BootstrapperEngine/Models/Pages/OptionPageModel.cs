using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Xml.Linq;
using Org.InCommon.InCert.BootstrapperEngine.Extensions;
using Org.InCommon.InCert.BootstrapperEngine.Logging;
using Org.InCommon.InCert.BootstrapperEngine.Models.Controls;
using Org.InCommon.InCert.BootstrapperEngine.Views.Controls;
using Org.InCommon.InCert.BootstrapperEngine.Views.Pages;
using System.Linq;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Pages
{
    public class OptionPageModel : AbstractPageModel
    {
        private string _title;
        private string _description;
        private Brush _linkBrush;

        public OptionPageModel(PagedViewModel model, XElement element)
            : base(new OptionPage(), model)
        {
            ImportFromNode(element);
        }

        private void ImportFromNode(XElement node)
        {
            try
            {
                if (node == null)
                    return;

                Title = node.ChildNodeValue("Title");
                Description = node.ChildNodeValue("Description");
                Foreground = node.ChildNodeBrushValue("ForegroundColor", Model.Foreground);
                Background = node.ChildNodeBrushValue("BackgroundColor", Model.Background);
                LinkBrush = node.ChildNodeBrushValue("LinkColor", Model.Foreground);
                ChildModels = new List<OptionGroupModel>();

                foreach (var groupElement in node.Elements("Group"))
                    ChildModels.Add(new OptionGroupModel(groupElement, this));
            }
            catch (Exception e)
            {
                Logger.Error("An exception occurred while trying to initialize option page model from xml: {0}", e.Message);
            }
        }

        public bool IsValid
        {
            get
            {
                return Children != null && Children.Any();
            }
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

        public Brush LinkBrush
        {
            get { return _linkBrush; }
            set { _linkBrush = value; OnPropertyChanged("LinkBrush"); }
        }

        public Brush FrameBackground
        {
            get { return Model.Background; }
        }

        public List<OptionGroup> Children
        {
            get
            {
                return ChildModels == null ? null : ChildModels.Select(e => e.Instance).ToList();
            }
        }
        public List<OptionGroupModel> ChildModels { get; private set; }

    }
}
