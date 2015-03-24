using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Ninject;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;
using log4net;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers
{
    public class DialogsManager : IDialogsManager
    {
        private static readonly ILog Log = Logger.Create();

        private readonly IAppearanceManager _appearanceManager;

        private readonly Dictionary<string, AbstractDialogModel> _dialogs = new Dictionary<string, AbstractDialogModel>();


        public DialogsManager(IAppearanceManager appearanceManager)
        {
            _appearanceManager = appearanceManager;
        }

        public void Initialize()
        {

        }

        public string ActiveDialogKey { get; set; }

        public void CloseAllDialogs()
        {
            foreach (var dialog in _dialogs.Values)
            {
                dialog.HideDialog();
            }
        }

        public void WaitForDurationOrCancel(DateTime start, TimeSpan interval)
        {
             
            if (interval.Duration().Ticks == 0)
                return;

            TimeSpan elapsed;
            do
            {
                Application.Current.DoEvents(250);
                elapsed = DateTime.UtcNow.Subtract(start);

                if (CancelRequested)
                    break;

            } while (elapsed.Duration().TotalSeconds <= interval.Duration().TotalSeconds);
            Application.Current.DoEvents(interval.TotalMilliseconds);
        }
        
        public IAppearanceManager AppearanceManager
        {
            get { return _appearanceManager; }
        }

        public bool CancelPending { get; set; }
        public bool CancelRequested { get; set; }

        public T GetDialog<T>(string key) where T : AbstractDialogModel
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                Log.Warn("Cannot retrieve dialog: no valid key specified");
                return default(T);
            }

            if (_dialogs.ContainsKey(key))
            {
                var result = _dialogs[key] as T;
                if (result == null)
                    Log.WarnFormat("The dialog associated with the key {0} is not of type {1}", key, typeof(T).Name);

                return result;
            }

            var dialog = Application.Current.CurrentKernel().Get<T>();
            dialog.DialogKey = key;
            _dialogs[key] = dialog;
            
            return dialog;
        }

        public void RemoveDialog(string key)
        {
            if (!_dialogs.ContainsKey(key))
            {
                return;
            }

            _dialogs.Remove(key);
        }

        public AbstractDialogModel GetExistingDialog(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                key = ActiveDialogKey;
            
            if (string.IsNullOrWhiteSpace(key))
            {
                Log.Warn("Cannot retrieve dialog: dialog key is empty or not valid");
                return null;
            }

            return !_dialogs.ContainsKey(key) ? null : _dialogs[key];
        }

        public void SetDialog(AbstractDialogModel manager, string key)
        {
            _dialogs[key] = manager;
        }




    }
}
