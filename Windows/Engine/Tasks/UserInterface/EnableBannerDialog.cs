using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class EnableBannerDialog:AbstractTask
    {
        private static readonly ILog Log = Logger.Create();
        
        [PropertyAllowedFromXml]
        public string Dialog
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public EnableBannerDialog(IEngine engine)
            : base(engine)
        {
            
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var dialog = DialogsManager.GetExistingDialog(Dialog);
                if (dialog == null)
                {
                    Log.WarnFormat("Cannot retrieve dialog for key {0}", Dialog);
                    return new NextResult();
                }

                dialog.IsEnabled=true;
                return new NextResult();
            }  
            catch(Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Enable banner dialog";
        }
    }
}
