using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Properties;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Wrappers;
using log4net;
using Application = System.Windows.Application;
using Control = System.Windows.Controls.Control;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers
{
    sealed class AppearanceManager : PropertyNotifyBase, IAppearanceManager
    {
        private const string BodyTextBrushKey = "BodyTextColor";
        private const string LinkTextBrushKey = "LinkTextColor";
        private const string TitleTextBrushKey = "TitleTextColor";
        private const string BorderBrushKey = "BorderColor";
        private const string BackgroundKey = "BackgroundColor";
        private const string InverseTitleTextBrushKey = "InverseTitleText";
        private const string InverseBodyTextBrushKey = "InverseBodyText";
        private const string LightBackgroundKey = "LightBackgroundColor";
        private const string CheckMarkBrushKey = "CheckMarkColor";
        private const string InputFieldTextBrushKey = "InputFieldTextColor";
        private const string NavigationTextBruskKey = "NavigationTextColor";

        private static readonly ILog Log = Logger.Create();

        private readonly ISettingsManager _manager;
        //private ResourceDictionary _styles = new ResourceDictionary();
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly Dictionary<string, TimedMessageWrapper> _timerMessages = new Dictionary<string, TimedMessageWrapper>();
        private readonly Dictionary<string, Brush> _brushes = new Dictionary<string, Brush>();

        // font
        private FontFamily _defaultFontFamily;

        // icon
        private BitmapFrame _applicationIcon;

        // application title and company
        private string _applicationTitle;
        private string _applicationCompany;

        public AppearanceManager(ISettingsManager manager)
        {
            _manager = manager;

            DefaultMargin = new Thickness(0, 0, 0, 2);
            DefaultPadding = new Thickness(0, 4, 0, 4);
        }

        public void Initialize()
        {
            _timer.Tick += HandleTimerTick;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            _timer.Start();
        }

      public string ApplicationTitle
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_applicationTitle))
                    _applicationTitle = Application.Current.GetProductName();

                return _applicationTitle;
            }

            set { _applicationTitle = value; OnPropertyChanged(); }
        }

        public string ApplicationCompany
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_applicationCompany))
                    _applicationCompany = Application.Current.GetCompanyName();

                return _applicationCompany;
            }

            set { _applicationCompany = value; OnPropertyChanged(); }
        }

        public BitmapFrame ApplicationIcon
        {
            get { return _applicationIcon; }
            set
            {
                _applicationIcon = value; OnPropertyChanged();
            }
        }

        public Brush DisabledTextBrush
        {
            get { return MakeBrushTransparent(BodyTextBrush as SolidColorBrush, 40); }
        }

        public Brush InverseTitleTextBrush
        {
            get { return GetBrushFromDictionary(InverseTitleTextBrushKey, new SolidColorBrush(Colors.White)); }
            set { _brushes[InverseTitleTextBrushKey] = value; OnPropertyChanged(); }
        }

        public Brush InverseBodyTextBrush
        {
            get { return GetBrushFromDictionary(InverseBodyTextBrushKey, new SolidColorBrush(Colors.White)); }
            set { _brushes[InverseBodyTextBrushKey] = value; OnPropertyChanged(); }
        }


        public Brush BorderBrush
        {
            get { return GetBrushFromDictionary(BorderBrushKey, new SolidColorBrush(Colors.Black)); }
            set { _brushes[BorderBrushKey] = value; OnPropertyChanged(); }
        }

        public Brush BackgroundBrush
        {
            get { return GetBrushFromDictionary(BackgroundKey, new SolidColorBrush(Colors.White)); }
            set { _brushes[BackgroundKey] = value; OnPropertyChanged(); }
        }

        public Brush TitleTextBrush
        {
            get { return GetBrushFromDictionary(TitleTextBrushKey, new SolidColorBrush(Colors.Black)); }
            set { _brushes[TitleTextBrushKey] = value; OnPropertyChanged(); }
        }

        public Brush BodyTextBrush
        {
            get { return GetBrushFromDictionary(BodyTextBrushKey, new SolidColorBrush(Colors.Black)); }
            set { _brushes[BodyTextBrushKey] = value; OnPropertyChanged(); }
        }

        public Brush LightBackgroundBrush
        {
            get { return GetBrushFromDictionary(LightBackgroundKey, new SolidColorBrush(Colors.Yellow)); }
            set { _brushes[LightBackgroundKey] = value; OnPropertyChanged(); }
        }

        public Brush LinkTextBrush
        {
            get { return GetBrushFromDictionary(LinkTextBrushKey, new SolidColorBrush(Colors.Blue)); }
            set { _brushes[LinkTextBrushKey] = value; OnPropertyChanged(); }
        }

        public Brush CheckMarkBrush
        {
            get { return GetBrushFromDictionary(CheckMarkBrushKey, new SolidColorBrush(Colors.Green)); }
            set { _brushes[CheckMarkBrushKey] = value; OnPropertyChanged(); }
        }

        public Brush InputFieldTextBrush { 
            get { return GetBrushFromDictionary(InputFieldTextBrushKey, new SolidColorBrush(Colors.Black)); }
            set { _brushes[InputFieldTextBrushKey] = value; OnPropertyChanged(); }
        }

        public Brush NavigationTextBrush
        {
            get { return GetBrushFromDictionary(NavigationTextBruskKey, new SolidColorBrush(Colors.Black)); }
            set { _brushes[NavigationTextBruskKey] = value; OnPropertyChanged(); }
        }
        
        public Color FadeColor(Color value)
        {
            var color = System.Drawing.Color.FromArgb(value.A, value.R, value.G, value.B);
            color = ControlPaint.LightLight(color);
            return new Color
            {
                A = color.A,
                R = color.R,
                G = color.G,
                B = color.B
            };
        }

        private Brush GetBrushFromDictionary(string key, Brush defaultValue)
        {
            if (string.IsNullOrWhiteSpace(key))
                return defaultValue;

            if (!_brushes.ContainsKey(key))
                _brushes[key] = defaultValue;

            return _brushes[key];
        }
        
        public FontFamily DefaultFontFamily
        {
            get { return _defaultFontFamily ?? (_defaultFontFamily = new FontFamily("Verdana")); }
            set { _defaultFontFamily = value; OnPropertyChanged(); }
        }


        public Thickness DefaultMargin { get; set; }
        public Thickness DefaultPadding { get; set; }

        public void SetBrush(string key, Brush value)
        {
            _brushes[key] = value;
        }

        public bool IsTimedMessagePresent(string key)
        {
            return _timerMessages.ContainsKey(key);
        }

        public void AddTimedMessage(AbstractModel model, string key, string text)
        {
            var wrapper = !_timerMessages.ContainsKey(key) 
                ? new TimedMessageWrapper { Active = false, DotCount = 0,} 
                : _timerMessages[key];
            
            wrapper.Model = model;
            wrapper.BaseText = text;
            
            _timerMessages[key] = wrapper;
        }

        public void ChangeTimedMessage(string key, string value)
        {
           ChangeTimedMessage(key, value,true);
        }

        public void ChangeTimedMessage(string key, string value, bool resetDotCount)
        {
            if (String.IsNullOrWhiteSpace(key))
                return;

            if (String.IsNullOrWhiteSpace(value))
                return;

            if (!_timerMessages.ContainsKey(key))
                return;

            var wrapper = _timerMessages[key];
            wrapper.BaseText = value;
            
            if (resetDotCount)
                wrapper.DotCount = 0;
        }

        public void ActivateTimedMessage(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                Log.Warn("Cannot activate timed message: no key specified");
                return;
            }

            if (!_timerMessages.ContainsKey(key))
            {
                Log.WarnFormat("No timed message exists for the key {0}", key);
                return;
            }

            _timerMessages[key].Active = true;
        }

        public void DeactiveTimedMessage(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                Log.Warn("Cannot deactivate timed message: no key specified");
                return;
            }

            if (!_timerMessages.ContainsKey(key))
            {
                Log.WarnFormat("No timed message exists for the key {0}", key);
                return;
            }

            _timerMessages[key].Active = false;
        }

        public void RemoveTimedMessage(string key)
        {
            if (!_timerMessages.ContainsKey(key))
                return;

            _timerMessages.Remove(key);
        }

        public void ClearTimedMessages()
        {
            _timer.Stop();
            try
            {
                _timerMessages.Clear();
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }
            finally
            {
                _timer.Start();
            }


        }

        public void ClearTimedMessages(object context)
        {
            _timer.Stop();
            try
            {
                var removeList = _timerMessages.Keys.Where(key => _timerMessages[key].Model == context).ToList();

                foreach (var key in removeList)
                    _timerMessages.Remove(key);
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }
            finally
            {
                _timer.Start();
            }


        }

        private void HandleTimerTick(object sender, EventArgs e)
        {
            if (!_timerMessages.Keys.Any())
                return;

            foreach (var key in _timerMessages.Keys)
            {
                var wrapper = _timerMessages[key];
                if (!wrapper.Active)
                    continue;

                if (string.IsNullOrWhiteSpace(wrapper.BaseText))
                    continue;
               
                wrapper.DotCount++;
                if (wrapper.DotCount > 4)
                    wrapper.DotCount = 0;

                _manager.BindingProxy.SettingProperty =
                    new StringSettingWrapper(
                        key,
                        wrapper.BaseText + new string('.', wrapper.DotCount),
                        wrapper.Model);
            }
        }

        public static Color? GetForegroundColorFromStyle(Style style)
        {
            if (style == null)
                return null;

            foreach (Setter setter in style.Setters)
            {
                if (setter.Property != Control.ForegroundProperty || setter.Property != TextBlock.ForegroundProperty)
                    continue;

                if (!(setter.Value is Color))
                    continue;

                return (Color)setter.Value;
            }

            return null;
        }

        private static Color? GetSystemColor(string colorName)
        {
            try
            {
                var type = typeof(SystemColors);
                var property = type.GetProperty(colorName,
                                                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static);

                return property == null ? null : property.GetValue(null, null) as Color?;
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return null;
            }
        }

        public void SetBrush(string key, string value)
        {
            try
            {
                var color = GetSystemColor(value);
                if (!color.HasValue)
                    color = (ColorConverter.ConvertFromString(value) as Color?);

                if (!color.HasValue)
                    return;

                SetBrush(key, new SolidColorBrush(color.Value));
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }
        }

        public Brush FadeBrush(SolidColorBrush value)
        {
            if (value == null)
            {
                Log.Warn("Cannot fade brush; original brush is null or invalid.");
                return new SolidColorBrush(Colors.White);
            }

            return new SolidColorBrush(FadeColor(value.Color));
        }

        public Brush MakeBrushTransparent(SolidColorBrush value, byte newAlphaValue)
        {
            if (value == null)
            {
                Log.Warn("Cannot make brush transparent; original brush is null or invalid.");
                return new SolidColorBrush(Colors.White);
            }

            return new SolidColorBrush(
                Color.FromArgb(
                    newAlphaValue, 
                    value.Color.R, 
                    value.Color.G, 
                    value.Color.B));
        }


        /*  public Brush GetBrushForColor(string colorName)
        {
            return GetBrushForColor(colorName, new SolidColorBrush(Colors.White));
        }*/

        public Brush GetBrushForColor(string colorName, Brush defaultValue)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(colorName))
                    return defaultValue;

                if (_brushes.ContainsKey(colorName))
                    return _brushes[colorName];

                var systemBrush = GetBrushForSystemColor(colorName);
                if (systemBrush != null)
                    return systemBrush;

                var color = ColorConverter.ConvertFromString(colorName);
                if (color == null)
                    return defaultValue;

                return new SolidColorBrush((Color)color);
            }
            catch (Exception)
            {
                return defaultValue;
            }

        }

        private static SolidColorBrush GetBrushForSystemColor(string colorName)
        {
            try
            {
                var type = typeof(SystemColors);
                var property = type.GetProperty(colorName,
                                                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static);

                return property == null ? null : (SolidColorBrush)property.GetValue(null, null);
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return null;
            }
        }
    }
}
