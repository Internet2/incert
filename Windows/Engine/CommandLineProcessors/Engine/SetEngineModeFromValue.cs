using System;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Logging;
using log4net;

namespace Org.InCommon.InCert.Engine.CommandLineProcessors.Engine
{
    class SetEngineModeFromValue:AbstractCommandLineProcessor
    {
        private static readonly ILog Log = Logger.Create();
        
        public SetEngineModeFromValue(IEngine engine):base(engine)
        {
            
        }

        public override void ProcessCommandLine(string value)
        {
            if (Engine == null)
            {
                Log.WarnFormat("Could not set engine mode value; engine is null");
                return;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                Log.WarnFormat("Could not set engine mode value; value is not specified");
                return;
            }

            EngineModes mode;
            if (!Enum.TryParse(value, true, out mode))
            {
                Log.WarnFormat("Could not set engine mode value; value is not valid");
                return;
            }
            
            Engine.Mode = mode;
        }
    }
}
