using System;
using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models;
using log4net;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.ControlActions
{
    class SelectAllContentAction:AbstractControlAction
    {
        private static readonly ILog Log = Logger.Create();

        public SelectAllContentAction(IEngine engine) : base(engine)
        {
        }

        public override void DoAction(AbstractModel model, bool includeOneTime)
        {
            try
            {
                if (!IsOneTimeOk(includeOneTime))
                    return;
                
                if (model == null)
                    return;

                if (model.Content == null)
                    return;
                
                var result = EvaluateConditions();
                if (!result.Result)
                    return;

                if ((model.Content as TextBox) != null)
                {
                    var target = model.Content as TextBox;
                    target.SelectAll();
                    target.Focus();
                }
                    
                if ((model.Content as PasswordBox) != null)
                {
                    var target = model.Content as PasswordBox;
                    target.SelectAll();
                    target.Focus();
                }
                
                Application.Current.DoEvents();
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }
        }
    }
}
