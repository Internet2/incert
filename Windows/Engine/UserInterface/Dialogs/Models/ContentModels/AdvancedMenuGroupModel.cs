using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Org.InCommon.InCert.Engine.AdvancedMenu;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances.CustomControls;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Properties;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels
{
    class AdvancedMenuGroupModel : PropertyNotifyBase
    {
        private readonly IHasEngineFields _engine;
        private readonly AdvancedMenuModel _model;
        private readonly List<AdvancedMenuItemModel> _childModels = new List<AdvancedMenuItemModel>();
        private string _groupName;

        public AdvancedMenuGroupModel(IHasEngineFields engine, AdvancedMenuModel model, string groupName)
        {
            _engine = engine;
            _model = model;
            Title = groupName;
            _model.PropertyChanged += PropertyChangedHandler;
        }

        public void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (!e.PropertyName.Equals("IsEnabled", StringComparison.InvariantCulture))
                return;

            FlagPropertyAsChanged("TextBrush");
        }

        public string Title
        {
            get { return _groupName; }
            set { _groupName = value; OnPropertyChanged(); }
        }

        public AdvancedMenuItemContainer Instance { get; set; }

        public Brush BackGround
        {
            get
            {
                return _model.ContainerBackground;
            }
        }

        public Brush TextBrush
        {
            get
            {
                return IsEnabled ?
                      _model.ContainerForeground :
                      _model.ContainerForeground.MakeTransparent(45);
            }
        }

        public FontFamily FontFamily { get { return _engine.AppearanceManager.DefaultFontFamily; } }

        public Visibility ContainerVisibility { get { return Visibility.Visible; } }
        public ICommand Command { get { return null; } }


        public bool IsEnabled
        {
            get { return _model.IsEnabled; }
        }

        public List<AdvancedMenuEntry> Children { get { return (from model in _childModels where model.Instance != null select model.Instance).ToList(); } }

        public void AddItem(IAdvancedMenuItem item)
        {
            if (item == null)
                return;

            var itemModel = new AdvancedMenuItemModel(item, _model, _engine);
            itemModel.Instance = new AdvancedMenuEntry { DataContext = itemModel };

            _childModels.Add(itemModel);
        }

        public void ClearHighlight(AdvancedMenuItemModel activeModel)
        {
            foreach (var entry in _childModels.Where(entry => !entry.Equals(activeModel)))
                entry.ClearHighlight();

        }
    }
}
