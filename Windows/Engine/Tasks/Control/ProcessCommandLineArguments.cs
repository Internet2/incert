using System;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Control
{
    class ProcessCommandLineArguments:AbstractTask
    {
        
        public ProcessCommandLineArguments(IEngine engine):base(engine) 
        {
        }
        
        public override IResult Execute(IResult previousResults)
        {
            try
            {
                CommandLineManager.ProcessCommandLines();
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }
        
        public override string GetFriendlyName()
        {
            return "Process command-line arguments";
        }
    }
}
