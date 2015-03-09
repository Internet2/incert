using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using Org.InCommon.InCert.Engine.Help;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Properties;
using log4net;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels
{
    class HelpDialogModel : PropertyNotifyBase
    {
        private readonly IHelpManager _helpManager;
        private readonly IAppearanceManager _appearanceManager;
        private ICommand _backCommand;
        private ICommand _forwardCommand;
        private ICommand _reloadCommand;
        private ICommand _homeCommand;
        private ICommand _preserveCheckedCommand;
        private double _left;
        private double _top;

        private static readonly ILog Log = Logger.Create();

        private readonly HelpWindow _dialogInstance;

        private bool _positioned;

        public double Left
        {
            get { return _left; }
            set { _left = value; OnPropertyChanged(); }
        }

        public double Top
        {
            get { return _top; }
            set { _top = value; OnPropertyChanged(); }
        }

        public ICommand BackCommand
        {
            get
            {
                return _backCommand ?? (
                    _backCommand = new RelayCommand(param => _dialogInstance.Browser.GoBack()));
            }
        }

        public ICommand ForwardCommand
        {
            get
            {
                return _forwardCommand ?? (
                    _forwardCommand = new RelayCommand(param => _dialogInstance.Browser.GoForward()));
            }
        }

        public ICommand HomeCommand
        {
            get
            {
                return _homeCommand ?? (
                        _homeCommand = new RelayCommand(
                            param => OpenHomePage(),
                            param => !string.IsNullOrWhiteSpace(_helpManager.HomeUrl)));
            }
        }

        public ICommand ReloadCommand
        {
            get
            {
                return _reloadCommand ?? (
                    _reloadCommand = new RelayCommand(
                  param => _dialogInstance.Browser.Refresh(),
                  param => _dialogInstance.Browser.Source != null));
            }
        }

        public ICommand PreserveCommand
        {
            get
            {
                return _preserveCheckedCommand ?? (
                    _preserveCheckedCommand = new RelayCommand(
                        param => PreserveChecked = !PreserveChecked));

            }
        }


        public FontFamily FontFamily { get { return _appearanceManager.DefaultFontFamily; } }

        public string Title { get { return _helpManager.DialogTitle; } }

        public HelpDialogModel(IHelpManager helpManager, IAppearanceManager appearanceManager)
        {
            _helpManager = helpManager;
            _appearanceManager = appearanceManager;
            _dialogInstance = new HelpWindow { DataContext = this };
            _dialogInstance.Browser.Navigated += LoadingCompleteHandler;
            _dialogInstance.Browser.Navigating += NavigatingHandler;

        }

        private void NavigatingHandler(object sender, NavigatingCancelEventArgs e)
        {
            if (!_dialogInstance.Browser.Dispatcher.CheckAccess())
            {
                _dialogInstance.Browser.Dispatcher.Invoke(()=>NavigatingHandler( sender, e ));
                return;
            }

            if (e.Uri == null)
                return;

            var scheme = e.Uri.Scheme.ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(scheme))
                return;

            if (!_helpManager.SchemeHandlers.ContainsKey(scheme))
                return;

            e.Cancel = true;

            var newUri = _helpManager.SchemeHandlers[scheme].LoadDocument(e.Uri);
            if (newUri == null)
                return;

            _dialogInstance.Browser.Source = newUri;
        }

        public bool PreserveChecked
        {
            get { return _helpManager.PreserveContentOnExit; }
            set { _helpManager.PreserveContentOnExit = value; OnPropertyChanged(); }
        }

        public bool ShowingContent
        {
            get
            {
                if (_dialogInstance == null)
                    return false;

                return _dialogInstance.Visibility == Visibility.Visible;
            }
        }

        public Uri CurrentContentUri
        {
            get
            {
                return _dialogInstance == null ? null : _dialogInstance.Browser.Source;
            }
        }


        public bool CanGoBack
        {
            get { return _dialogInstance.Browser.CanGoBack; }
        }

        public bool CanGoForward
        {
            get { return _dialogInstance.Browser.CanGoForward; }
        }

        private void LoadingCompleteHandler(object sender, NavigationEventArgs e)
        {
            FlagPropertyAsChanged("CanGoBack");
            FlagPropertyAsChanged("CanGoForward");
            _dialogInstance.Browser.Focus();
        }

        public Brush Background
        {
            get { return _appearanceManager.BackgroundBrush; }
        }

        public Brush TextBrush
        {
            get { return _appearanceManager.BodyTextBrush; }
        }

        public Brush HighlightBrush
        {
            get { return _appearanceManager.LinkTextBrush; }
        }

        public void ShowUri(Uri uri, double left, double top)
        {
           if (_dialogInstance.Visibility != Visibility.Visible && !_positioned)
            {
                Left = GetLeftPosition(left);
                Top = GetTopPosition(top);
                _positioned = true;
            }

            ShowUri(uri);
        }

        public void ShowUri(Uri uri)
        {
            if (!_dialogInstance.Browser.Dispatcher.CheckAccess())
            {
                _dialogInstance.Browser.Dispatcher.Invoke(() => ShowUri(uri));
                return;
            }

            _dialogInstance.Browser.Navigate(uri);
            _dialogInstance.Show();
        }

        private double GetLeftPosition(double value)
        {
            var result = value + _helpManager.InitialLeftOffset;
            if (result < 25) result = 25;
            return result;
        }

        private double GetTopPosition(double value)
        {
            var result = value +_helpManager.InitialTopOffset;
            if (result < 25) result = 25;
            return result;
        }
        
        public string TopBannerText { get { return _helpManager.TopBannerText; } }

        public string PreserveText
        {
            get { return _helpManager.PreserveContentText; }
        }

        private void OpenHomePage()
        {
            try
            {
                _dialogInstance.Browser.Source = new Uri(_helpManager.HomeUrl);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to open the help dialog's home url: {0}", e.Message);
            }

        }
    }
}
