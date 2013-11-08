using System;
using System.ComponentModel;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.NativeCode.Wmi;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.WebServices.Contracts;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.ActiveDirectory
{
    class SetComputerName : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        [PropertyAllowedFromXml]
        public string PreferredName
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public SetComputerName(IEngine engine)
            : base (engine)
        {
           
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(PreferredName))
                    throw new Exception("Preferred name cannot be empty");
                
                var request =
                    EndpointManager.GetContract<AbstractComputerNameContract>(EndPointFunctions.GetComputername);
                request.PreferredName = PreferredName.ToUpperInvariant();

                var result = request.MakeRequest<ComputerNameResult>();
                if (result == null)
                    return request.GetErrorResult();

                if (Environment.MachineName.Equals(result.NameToUse, StringComparison.OrdinalIgnoreCase))
                {
                    Log.InfoFormat("Computer name is already {0}", result.NameToUse);
                    return new NextResult();
                }

                using (var computer = ComputerSystem.GetComputerSystem())
                {
                    if (computer == null)
                        throw  new Exception("Cannot instantiate wmi computer system object");

                    var renameResult = Convert.ToInt32(computer.Rename(result.NameToUse, null, null));
                    if (renameResult !=0)
                        throw new Win32Exception(renameResult);
                }

                Log.InfoFormat("Computer name changed to {0}", result.NameToUse);
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Rename computer";
        }
    }
}
