using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers
{
    public interface IAppearanceManager
    {
        void Initialize();
       
        string ApplicationTitle { get; set; }
        string ApplicationCompany { get; set; }

        FontFamily DefaultFontFamily { get; set; }
        BitmapFrame ApplicationIcon { get; set; }
        Brush DisabledTextBrush { get; }
        Brush InverseTitleTextBrush { get; set; }
        Brush InverseBodyTextBrush { get; set; }
        Brush BorderBrush { get; set; }
        Brush LightBackgroundBrush { get; set; }
        Brush BackgroundBrush { get; set; }
        Brush TitleTextBrush { get; set; }
        Brush BodyTextBrush { get; set; }
        Brush LinkTextBrush { get; set; }
        Brush CheckMarkBrush { get; set; }
        Brush InputFieldTextBrush { get; set; }
        Brush NavigationTextBrush { get; set; }
        
        Brush FadeBrush(SolidColorBrush value);
        Brush MakeBrushTransparent(SolidColorBrush value, byte newAlphaAmount);
        Brush GetBrushForColor(string value, Brush defaultValue);
        
        Color FadeColor(Color value);
        
        void SetBrush(string key, string value);
        void SetBrush(string key, Brush value);
       
        bool IsTimedMessagePresent(string key);
        void AddTimedMessage(AbstractModel model, string key, string text);
        void ChangeTimedMessage(string key, string value);
        void ChangeTimedMessage(string key, string value, bool resetDotCount);
        void ActivateTimedMessage(string key);
        void DeactiveTimedMessage(string key);
        void RemoveTimedMessage(string key);
        void ClearTimedMessages();
        void ClearTimedMessages(object context);
        
        event PropertyChangedEventHandler PropertyChanged;
        Thickness DefaultMargin { get; set; }
        Thickness DefaultPadding { get; set; }
    }
}