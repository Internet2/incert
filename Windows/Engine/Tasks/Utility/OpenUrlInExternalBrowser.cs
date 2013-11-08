using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Utility
{
    class OpenUrlInExternalBrowser : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        public OpenUrlInExternalBrowser(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Url { get { return GetDynamicValue(); } set { SetDynamicValue(value); } }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (!UserInterfaceUtilities.OpenBrowser(Url))
                    Log.WarnFormat("Could not open url: {0}", Url);
                
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return string.Format("Open url ({0}) in external browser", Url);
        }
    }
}
