using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.Utility
{
    class ScheduleRunOnceUtility:AbstractExecuteUtilityTask
    {
        private const string KeyPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce";
        
        public ScheduleRunOnceUtility(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string EntryName
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }
        
        public override IResult Execute(IResult previousResults)
        {
            try
            {
                InitializeArguments();

                if (string.IsNullOrWhiteSpace(EntryName))
                    throw new Exception("Entry name must be set");

                using (var key = RegistryUtilities.RegistryRootValues.LocalMachine.OpenRegistryKey(KeyPath,true))
                {
                    if (key == null)
                        throw  new Exception("Could not open run once registry key");

                    var value = string.Format("{0} {1}", TargetPath, Arguments);
                    key.SetValue(EntryName, value);
                }

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return string.Format("Add run-once utility key ({0})", EntryName);
        }
    }
}
