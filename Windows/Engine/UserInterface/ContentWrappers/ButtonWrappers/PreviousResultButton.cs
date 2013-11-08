using System;
using System.Reflection;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ButtonWrappers
{
    class PreviousResultButton : AbstractButton
    {
        private static readonly ILog Log = Logger.Create();

        private AbstractTaskResult _taskResult;

        [PropertyAllowedFromXml]
        public string Result
        {
            set
            {
                _taskResult = ReflectionUtilities.LoadFromAssembly<AbstractTaskResult>(value);
            }
        }

        public PreviousResultButton(IEngine engine):base(engine)
        {
        }

        public AbstractTaskResult TaskResult
        {
            get
            {
                var result = SettingsManager.GetTemporaryObject("Previous Task Result") as AbstractTaskResult;
                return result == null ? _taskResult : TransformResult(result);
            }
            set
            {
                _taskResult = value;
            }
        }

        public override bool Initialized()
        {
            if (Target == ButtonTargets.None)
                return false;
            
            return _taskResult != null && _taskResult.Initialized();
        }

        private AbstractTaskResult TransformResult(AbstractTaskResult source)
        {
            try
            {
                var destination = TaskResult;
                return (AbstractTaskResult)ReflectionUtilities.CopyObjectProperties(
                    source,
                    destination,
                    BindingFlags.Public | BindingFlags.Instance);
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return source;
            }

        }
    }
}
