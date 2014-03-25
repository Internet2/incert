using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.CommandModels
{
    interface ICommandModel
    {
        string Text { get;  }
        Brush TextBrush { get; }
        bool Enabled { get; }
        ICommand Command { get; }
        bool IsDefaultButton { get; }
        bool IsCancelButton { get; }
        Visibility Visibility { get; }
    }
}
