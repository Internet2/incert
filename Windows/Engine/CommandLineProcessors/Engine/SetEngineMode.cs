using System;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using log4net;

namespace Org.InCommon.InCert.Engine.CommandLineProcessors.Engine
{
    public class SetEngineMode : AbstractCommandLineProcessor
    {
        private static readonly ILog Log = Logger.Create();
        
        [PropertyAllowedFromXml]
        public EngineModes Mode { get; set; }

        public SetEngineMode(IEngine engine)
            : base(engine)
        {

        }

        public override void ProcessCommandLine(string value)
        {
            if (Engine == null)
            {
                Log.WarnFormat("Could not set engine mode value; engine is null");
                return;
            }

            Engine.Mode = Mode;
        }
    }
}
