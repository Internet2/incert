using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Org.InCommon.InCert.Engine.AdvancedMenu;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.TaskBranches;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances.CustomControls;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Properties;
using log4net;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels
{
    class AdvancedMenuItemModel : PropertyNotifyBase
    {
        private static readonly ILog Log = Logger.Create();

        private readonly IAdvancedMenuItem _item;
        private readonly AdvancedMenuModel _model;
        private readonly IAppearanceManager _appearanceManager;
        private readonly IBranchManager _branchManager;

        private Brush _background;

        private Brush _textBrush;
        private Brush _graphicForeground;
        private Brush _graphicBackground;

        private ICommand _singleClickCommand;
        private ICommand _doubleClickCommand;

        public AdvancedMenuEntry Instance { get; set; }

        public AdvancedMenuItemModel(IAdvancedMenuItem item, AdvancedMenuModel model, IAppearanceManager appearanceManager, IBranchManager branchManager)
        {
            _item = item;
            _model = model;
            _appearanceManager = appearanceManager;
            _branchManager = branchManager;
            Background = _appearanceManager.BodyTextBrush;
            GraphicForeground = _appearanceManager.BodyTextBrush;
            GraphicBackground = _appearanceManager.BackgroundBrush;
            TextBrush = _appearanceManager.BackgroundBrush;
            _model.PropertyChanged += PropertyChangedHandler;
        }

        public void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (!e.PropertyName.Equals("IsEnabled", StringComparison.InvariantCulture))
                return;

            FlagPropertyAsChanged("TextBrush");
            FlagPropertyAsChanged("Background");
            FlagPropertyAsChanged("GraphicForeground");
            FlagPropertyAsChanged("GraphicBackground");
        }

        public string KeyText { get { return _item.ButtonText; } }
        public string Title { get { return _item.Title; } }

        public Brush Background
        {
            get
            {
                return IsEnabled
                    ? _background
                    : _appearanceManager.MakeBrushTransparent(_background as SolidColorBrush, 45);
            }
            private set
            {
                _background = value;
                OnPropertyChanged();
            }
        }

        public Brush GraphicBackground
        {
            get
            {
                return IsEnabled
                    ? _graphicBackground
                    : _appearanceManager.MakeBrushTransparent(_graphicBackground as SolidColorBrush, 45);
            }
            set
            {
                _graphicBackground = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get { return _model.IsEnabled; }
        }

        public Brush GraphicForeground
        {
            get
            {
                return IsEnabled
                    ? _graphicForeground
                    : _appearanceManager.MakeBrushTransparent(_graphicForeground as SolidColorBrush, 45);
            }
            private set
            {
                _graphicForeground = value;
                OnPropertyChanged();
            }
        }

        public Brush TextBrush
        {
            get
            {
                return IsEnabled
                    ? _textBrush
                    : _appearanceManager.MakeBrushTransparent(_textBrush as SolidColorBrush, 45);
            }
            private set
            {
                _textBrush = value;
                OnPropertyChanged();
            }
        }
        public FontFamily FontFamily { get { return _appearanceManager.DefaultFontFamily; } }

        public ICommand SingleClickCommand
        {
            get { return _singleClickCommand ?? (_singleClickCommand = new RelayCommand(param => SetInfoFields())); }
        }

        private void SetInfoFields()
        {
            GraphicForeground = _appearanceManager.BackgroundBrush;
            GraphicBackground = _appearanceManager.BodyTextBrush;
            _model.Title = Title;
            _model.Description = _item.Description;
            _model.RunModel.Command = DoubleClickCommand;
            LogicalTreeHelper.BringIntoView(Instance);
            _model.ClearHighlight(this);
        }

        private void ExecuteBranch()
        {
            try
            {
                _model.ActivateWorkingDisplay(_item);
                if (!string.IsNullOrWhiteSpace(_item.WorkingTitle))
                    _model.Title = _item.WorkingTitle;

                if (!string.IsNullOrWhiteSpace(_item.WorkingDescription))
                    _model.Description = _item.WorkingDescription;

                var branch = _branchManager.GetBranch(_item.Branch);
                if (branch == null)
                    throw new Exception(string.Format("Branch {0} not present", _item.Branch));

                var result = branch.Execute(null);

                _model.Description = _item.Description;
                _model.Title = _item.Title;
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to invoke an advanced menu command ({0}): {1}", _item.Title, e.Message);
            }
            finally
            {
                _model.DeactiveWorkingDisplay(_item);
            }


        }

        public ICommand DoubleClickCommand
        {
            get
            {
                return _doubleClickCommand ?? (
                    _doubleClickCommand = new ClearFocusCommand(_model.DialogInstance, param => ExecuteBranch()));
            }
        }

        public object Tag { get { return this; } }

        public void ClearHighlight()
        {
            Background = _appearanceManager.BodyTextBrush;
            GraphicForeground = _appearanceManager.BodyTextBrush;
            GraphicBackground = _appearanceManager.BackgroundBrush;
            TextBrush = _appearanceManager.BackgroundBrush;
        }
    }
}

