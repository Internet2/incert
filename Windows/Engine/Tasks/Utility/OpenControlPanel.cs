using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.Utility
{
    class OpenControlPanel : AbstractTask
    {
        public OpenControlPanel(IEngine engine)
            : base(engine)
        {
        }


        [PropertyAllowedFromXml]
        public SystemUtilities.ControlPanelNames ControlPanel { get; set; }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                SystemUtilities.OpenControlPanel(ControlPanel);
                
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return string.Format("Open control panel ({0})", ControlPanel.ToString());
        }
    }
}
