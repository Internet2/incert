using System;
using System.Windows;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers
{
    public interface IDialogsManager
    {
        void Initialize();

        string ActiveDialogKey { get; set; }
        AbstractDialogModel GetExistingDialog(string key);
        T GetDialog<T>(string key) where T : AbstractDialogModel;
        T GetDialog<T>(Window instance) where T : AbstractDialogModel;
        void SetDialog(AbstractDialogModel manager, string key);
        void RemoveDialog(string key);
       

        void CloseAllDialogs();

        void WaitForDurationOrCancel(DateTime startTime, TimeSpan duration);

        IAppearanceManager AppearanceManager { get; }

        bool CancelPending { get; set; }
        bool CancelRequested { get; set; }

    }
}