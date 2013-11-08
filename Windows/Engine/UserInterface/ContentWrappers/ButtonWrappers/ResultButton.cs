using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ButtonWrappers
{
    class ResultButton : AbstractButton
    {
        public ResultButton(IEngine engine):base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Result
        {
            set
            {
                TaskResult = ReflectionUtilities.LoadFromAssembly<AbstractTaskResult>(value);
            }
        }

        public AbstractTaskResult TaskResult { get; set; }

        public override bool Initialized()
        {
            if (!base.Initialized())
                return false;

            return TaskResult != null && TaskResult.Initialized();
        }
    }
}
