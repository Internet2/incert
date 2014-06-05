using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Org.InCommon.InCert.Engine.AdvancedMenu;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances.CustomControls;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.CommandModels.AdvancedMenu;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Properties;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels
{
    class AdvancedMenuModel : PropertyNotifyBase
    {
        private static readonly ILog Log = Logger.Create();

        private readonly DispatcherTimer _timer;
        private readonly IHasEngineFields _engine;
        private readonly AbstractDialogModel _model;
        private readonly AdvancedMenuWindow _dialogInstance;
        
        private string _title;
        private string _description;
        private bool _isEnabled;
        private Cursor _cursor;

        private string _baseDescription;
        private int _dotCount;

        private CloseCommandModel _closeModel;
        private RunModel _runModel;
        private HelpModel _helpModel;

        private ICommand _clearFocusCommand;

        private readonly Dictionary<string, AdvancedMenuGroupModel> _groupModels = new Dictionary<string, AdvancedMenuGroupModel>();

        private double _left;
        private double _top;
        private double _width;
        private double _height;

        public AdvancedMenuModel(IHasEngineFields engine,
            AbstractDialogModel model)
        {
            IsEnabled = true;
            _engine = engine;
            _model = model;
            _dialogInstance = new AdvancedMenuWindow { DataContext = this };
            BuildGroupsList();
            _width = 500;
            _height = 550;
            Cursor = Cursors.Arrow;
            Title = _engine.AdvancedMenuManager.DefaultTitle;
            Description = engine.AdvancedMenuManager.DefaultDescription;
            _timer = new DispatcherTimer
                {
                    Interval = new TimeSpan(0, 0, 0, 0, 500),
                    IsEnabled = false,
                };

            _timer.Tick += TimerTickHandler;
        }

        public IResult Result { get; set; }

        public Cursor Cursor
        {
            get { return _cursor; }
            set
            {
                _cursor = value;
                OnPropertyChanged();
            }
        }

        public WindowStyle WindowStyle { get { return WindowStyle.ToolWindow; } }

        public AdvancedMenuModel ContentModel
        {
            get { return this; }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public AdvancedMenuWindow DialogInstance
        {
            get { return _dialogInstance; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }

        public Brush TextBrush
        {
            get { return _engine.AppearanceManager.BodyTextBrush; }
        }

        public Brush ContainerBackGround
        {
            get { return new SolidColorBrush(Colors.White); }
        }

        public AbstractDialogModel ParentModel
        {
            get { return _model; }
        }

        public void ActivateWorkingDisplay(IAdvancedMenuItem item)
        {
            IsEnabled = false;
            UserInterfaceUtilities.DisableCloseButton(_dialogInstance);
            _baseDescription = item.WorkingDescription;
            Description = item.WorkingDescription;
            Title = item.WorkingTitle;
            Cursor = Cursors.Wait;
            _timer.Start();
            _timer.IsEnabled = true;
        }

        public void DeactiveWorkingDisplay(IAdvancedMenuItem item)
        {
            IsEnabled = true;
            Cursor = Cursors.Arrow;
            UserInterfaceUtilities.EnableCloseButton(_dialogInstance);
            _timer.IsEnabled = false;
            _timer.Stop();
            ClearHighlight(null);
        }

        public ICommand ClearFocusCommand
        {
            get
            {
                return _clearFocusCommand ??
                    (_clearFocusCommand = new RelayCommand(param => ClearHighlight(null)));
            }
        }

        private void BuildGroupsList()
        {
            foreach (var item in _engine.AdvancedMenuManager.Items.Values)
            {
                if (!item.Show)
                    continue;

                var group = GetGroupModel(item.Group);
                if (group == null)
                    continue;

                group.AddItem(item);
            }
        }

        public void ClearHighlight(AdvancedMenuItemModel activeModel)
        {
            foreach (var group in _groupModels.Values)
            {
                group.ClearHighlight(activeModel);
            }

            if (activeModel == null)
            {
                Title = _engine.AdvancedMenuManager.DefaultTitle;
                Description = _engine.AdvancedMenuManager.DefaultDescription;
                RunModel.Command = null;
            }
        }

        private AdvancedMenuGroupModel GetGroupModel(string groupKey)
        {
            if (string.IsNullOrWhiteSpace(groupKey))
                return null;

            if (!_groupModels.ContainsKey(groupKey))
            {
                var groupModel = new AdvancedMenuGroupModel(_engine.AppearanceManager, _engine.BranchManager, this, groupKey);
                groupModel.Instance = new AdvancedMenuItemContainer { DataContext = groupModel };
                _groupModels[groupKey] = groupModel;
            }

            return _groupModels[groupKey];
        }

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

        public double Width
        {
            get { return _width; }
            set { _width = value; OnPropertyChanged(); }
        }

        public double Height
        {
            get { return _height; }
            set { _height = value; OnPropertyChanged(); }
        }

        public string WindowTitle
        {
            get { return _engine.AppearanceManager.ApplicationTitle; }
        }

        public FontFamily FontFamily
        {
            get { return _engine.AppearanceManager.DefaultFontFamily; }
        }

        public Brush Background
        {
            get { return _engine.AppearanceManager.BackgroundBrush; }
        }

        public List<AdvancedMenuItemContainer> Groups
        {
            get
            {
                return (from model in _groupModels.Values
                        where model.Instance != null
                        select model.Instance).ToList();
            }
        }



        public void ShowDialog(double left, double top, string group)
        {
            try
            {
                _dialogInstance.Left = GetLeftPosition(left);
                _dialogInstance.Top = GetTopPosition(top);
                _dialogInstance.Owner = ParentModel.DialogInstance;
                _dialogInstance.Show();
                ScrollToGroup(group);
                do
                {
                    Application.Current.DoEvents();
                    Thread.Sleep(5);
                } while (_dialogInstance.IsLoaded);



            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while showing the advanced menu: {0}", e.Message);
            }

        }

        private void ScrollToGroup(string group)
        {
            if (string.IsNullOrWhiteSpace(group))
                return;

            if (!_groupModels.ContainsKey(group))
                return;

            var groupModel = _groupModels[group];

            if (groupModel.Instance == null)
                return;
          
            LogicalTreeHelper.BringIntoView(groupModel.Instance);
        }

        private double GetLeftPosition(double value)
        {
            var result = value + _engine.AdvancedMenuManager.InitialLeftOffset;
            if (result < 25) result = 25;
            return result;
        }

        private double GetTopPosition(double value)
        {
            var result = value + _engine.AdvancedMenuManager.InitialTopOffset;
            if (result < 25) result = 25;
            return result;
        }

        public void CloseDialog()
        {
            _dialogInstance.Close();
        }

        public CloseCommandModel CloseModel
        {
            get { return _closeModel ?? (_closeModel = new CloseCommandModel(_engine.AppearanceManager, this)); }
        }

        public RunModel RunModel
        {
            get { return _runModel ?? (_runModel = new RunModel(_engine.AppearanceManager, this)); }
        }

        public HelpModel HelpModel
        {
            get { return _helpModel ?? (_helpModel = new HelpModel(_engine.AppearanceManager, _engine.HelpManager, this)); }
        }

        public string HelpTopic
        {
            get { return _engine.AdvancedMenuManager.HelpTopic; }
        }

        private delegate void TimerTickDelegate(object sender, EventArgs e);

        private void TimerTickHandler(object sender, EventArgs e)
        {
            if (!_timer.Dispatcher.CheckAccess())
            {
                _timer.Dispatcher.Invoke(new TimerTickDelegate(TimerTickHandler), new[] { sender, e });
            }

            Description = _baseDescription + new string('.', _dotCount);
            _dotCount++;
            if (_dotCount > 4)
                _dotCount = 0;
        }
    }

}
