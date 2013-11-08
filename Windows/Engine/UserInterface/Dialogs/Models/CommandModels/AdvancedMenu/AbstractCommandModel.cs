using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Properties;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.CommandModels.AdvancedMenu
{
    abstract class AbstractCommandModel : PropertyNotifyBase, ICommandModel
    {
        protected readonly IAppearanceManager AppearanceManager;
        protected readonly AdvancedMenuModel Model;
        private string _text;
        private ICommand _command;
        private bool _defaultButton;
        private bool _cancelButton;
        private Visibility _visibility;

        protected AbstractCommandModel(IAppearanceManager appearanceManager, AdvancedMenuModel model)
        {
            AppearanceManager = appearanceManager;
            Model = model;
            Model.PropertyChanged += PropertyChangedHandler;
            PropertyChanged += PropertyChangedHandler;
        }

        public void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (!e.PropertyName.Equals("Command", StringComparison.InvariantCulture) 
                && !e.PropertyName.Equals("IsEnabled", StringComparison.InvariantCulture))
                return;

            FlagPropertyAsChanged("Enabled");
            FlagPropertyAsChanged("TextBrush");

        }

        public string Text
        {
            get { return _text; }
            protected set { _text = value; OnPropertyChanged(); }
        }

        public Brush TextBrush
        {
            get
            {
                return Enabled ?
                        AppearanceManager.NavigationTextBrush :
                        AppearanceManager.MakeBrushTransparent(AppearanceManager.NavigationTextBrush as SolidColorBrush, 45);
            }
        }

        public bool Enabled
        {
            get
            {
                if (!Model.IsEnabled)
                    return false;

                return (Command !=null);
            }
        }

        public ICommand Command
        {
            get { return _command; }
            set
            {
                _command = value;
                OnPropertyChanged();
            }
        }

        public bool IsDefaultButton
        {
            get { return _defaultButton; }
            protected set { _defaultButton = value; OnPropertyChanged(); }
        }
        public bool IsCancelButton
        {
            get { return _cancelButton; }
            protected set { _cancelButton = value; OnPropertyChanged(); }
        }

        public Visibility Visibility
        {
            get { return _visibility; }
            protected set { _visibility = value; OnPropertyChanged(); }
        }
    }
}
