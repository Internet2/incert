using System;
using Org.InCommon.InCert.Engine.Results;

namespace Org.InCommon.InCert.Engine.Engines
{
    public interface IEngine:IHasEngineFields
    {
        IResult Execute();
        EngineModes Mode { get; set; }
        Guid Identifier { get; }
        
    }
}
