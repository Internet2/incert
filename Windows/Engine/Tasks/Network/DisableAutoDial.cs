using System;
using Microsoft.Win32;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Network
{
    class DisableAutoDial:AbstractTask
    {
        private static readonly ILog Log = Logger.Create();
        
        private const string KeyPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings";
        private const string ValueName = "EnableAutoDial";

        public DisableAutoDial(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(KeyPath, true))
                {
                    if (key == null)
                        return new NextResult();

                    var value = key.GetValue(ValueName).ToIntOrDefault(0);
                    if (value == 0)
                        return new NextResult();

                    key.SetValue(ValueName,0);
                    Log.InfoFormat("Auto-dial disabled");
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
            return "Disable auto-dial";
        }
    }
}
