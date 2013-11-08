using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Org.InCommon.InCert.Engine.AdvancedMenu;
using Org.InCommon.InCert.Engine.TaskBranches;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances.CustomControls;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Properties;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels
{
    class AdvancedMenuGroupModel : PropertyNotifyBase
    {
        private readonly IAppearanceManager _appearanceManager;
        private readonly IBranchManager _branchManager;
        private readonly AdvancedMenuModel _model;
        private readonly List<AdvancedMenuItemModel> _childModels = new List<AdvancedMenuItemModel>();
        private string _groupName;

        public AdvancedMenuGroupModel(IAppearanceManager appearanceManager, IBranchManager branchManager, AdvancedMenuModel model, string groupName)
        {
            _appearanceManager = appearanceManager;
            _branchManager = branchManager;
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

        public Brush BackGround { get { return new SolidColorBrush(Colors.White); } }

        public Brush TextBrush
        {
            get
            {
                return IsEnabled ?
                      _appearanceManager.BackgroundBrush :
                      _appearanceManager.MakeBrushTransparent(_appearanceManager.BackgroundBrush as SolidColorBrush, 45);
            }
        }

        public FontFamily FontFamily { get { return _appearanceManager.DefaultFontFamily; } }

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

            var itemModel = new AdvancedMenuItemModel(item, _model, _appearanceManager, _branchManager);
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
