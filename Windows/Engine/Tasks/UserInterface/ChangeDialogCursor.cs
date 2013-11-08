using System;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class ChangeDialogCursor : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        [PropertyAllowedFromXml]
        public string Dialog
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public String Cursor
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public ChangeDialogCursor(IEngine engine)
            : base (engine)
        {
            
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var dialog = DialogsManager.GetExistingDialog(Dialog);
                if (dialog == null)
                {
                    Log.DebugFormat("Cannot set a cursor for the dialog {0}: dialog does not exist.", Dialog);
                    return new NextResult();
                }

                var cursor = Cursor.ConvertToCursor();
                if (cursor == null)
                {
                    Log.WarnFormat("Cannot set a cursor for the dialog {0}: could not generate cursor {1}.", Dialog, Cursor);
                    return new NextResult();
                }
                    
                dialog.Cursor = cursor;
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Change dialog cursor";
        }
    }
}
