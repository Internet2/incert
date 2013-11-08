using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Control
{
    class MoveToKeyedTask:AbstractTask
    {
        public MoveToKeyedTask(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string NavPoint
        {
            get { return GetDynamicValue(); } 
            set {SetDynamicValue(value);}
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NavPoint))
                    throw new Exception("Task key cannot be empty");

                return new JumpResult {Key = NavPoint};
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return string.Format("Move to keyed task ({0})", NavPoint);
        }
    }
}
