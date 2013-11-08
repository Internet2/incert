using System.Collections.Generic;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Utility
{
    abstract class AbstractExecuteUtilityTask:AbstractTask
    {
        private readonly List<string> _arguments;

        protected AbstractExecuteUtilityTask(IEngine engine) : base(engine)
        {
            _arguments = new List<string>();
        }

        [PropertyAllowedFromXml]
        public string Verb
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string TargetPath
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string Argument
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;

                _arguments.Add(value);
            }
        }

        public string Arguments
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string WorkingDirectory
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public bool HideWindow { get; set; }

        [PropertyAllowedFromXml]
        public bool UseShellExecute { get; set; }

        // convert the list of arguments into a string and assign it to the arguments property
        internal void InitializeArguments()
        {
            Arguments = string.Join(" ", _arguments); 
        }
    }
}
