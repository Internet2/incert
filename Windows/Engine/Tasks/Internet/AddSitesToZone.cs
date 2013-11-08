using System;

using System.ComponentModel;
using System.Runtime.InteropServices;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.NativeCode.InternetSecurityManager;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Internet
{
    class AddUrlToZone : AbstractTask
    {
        private const int AlreadyExists = -2147467259;

        private static readonly ILog Log = Logger.Create();

        public AddUrlToZone(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Url
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public TargetZone Zone { get; set; }

        public override IResult Execute(IResult previousResults)
        {
            IInternetSecurityManager manager = null;
            try
            {
                var guid = new Guid("7b8a2d94-0ac9-11d1-896c-00c04fb6bfc4");
                manager = Activator.CreateInstance(Type.GetTypeFromCLSID(guid)) as IInternetSecurityManager;
                if (manager == null)
                    throw new Exception("Could not instantiate security manager interface");

                var result = new Win32Exception(manager.SetZoneMapping(Zone, Url, OperationFlags.Create));
                if (result.ErrorCode == AlreadyExists)
                {
                    Log.InfoFormat("Url ({0}) already exists in the {1} zone", Url, Zone);
                    return new NextResult();
                }
                
                if (result.ErrorCode != 0)
                {
                    Log.WarnFormat("Windows returned error {0} ({1}) when trying to add the url {2} to the internet zone {3}", result.ErrorCode, result.Message, Url, Zone);
                    throw result;
                }
                    
                Log.InfoFormat("Url ({0}) successfully added to the the {1} zone", Url,Zone);
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
            finally
            {
                if (manager != null)
                    Marshal.ReleaseComObject(manager);
            }
        }

        public override string GetFriendlyName()
        {
            return "Add site to security zone";
        }
    }
}
