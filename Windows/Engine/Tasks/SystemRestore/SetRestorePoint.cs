using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.SystemRestore;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.SystemRestore
{
    class SetRestorePoint : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        private const uint BeginSystemChange = 100;
        private const uint EndSystemChange = 101;
        private const uint ApplicationInstall = 0;
        private const uint CancelledOperation = 13;

        public SetRestorePoint(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Name
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Name))
                    return new CouldNotCreateRestorePoint {Issue = "No name for restore point specified"};

                int result;
                using (var task = Task<int>.Factory.StartNew(() => CreateRestorePoint(Name)))
                {
                    task.WaitUntilExited();
                 
                    if (task.IsFaulted)
                        result = -1;
                    else
                        result = task.Result;
                }

                if (result != 0)
                    return FormatIssue(result);

                return new NextResult();

            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }


        public override string GetFriendlyName()
        {
            return "Set restore point";
        }

        private static int CreateRestorePoint(string name)
        {
            var result = OpenRestorePoint(name);
            if (result != 0)
            {
                Log.Warn("An issue (" + result.ToString(CultureInfo.InvariantCulture) +
                                    ") occurred while attempting to open a system restore point");
                return result;
            }


            result = CloseRestorePoint(name);
            if (result != 0)
            {
                Log.Warn("An issue (" + result.ToString(CultureInfo.InvariantCulture) +
                                    ") occurred while attempting to close a system restore point");
                CancelRestorePoint(name);
            }

            return result;
        }

        private static int OpenRestorePoint(string name)
        {
            try
            {
                return (int)NativeCode.Wmi.SystemRestore.CreateRestorePoint(name, BeginSystemChange, ApplicationInstall);
            }
            catch (COMException e)
            {
                Log.Warn(e);
                return e.ErrorCode;
            }
        }

        private static int CloseRestorePoint(string name)
        {
            try
            {
                return (int)NativeCode.Wmi.SystemRestore.CreateRestorePoint(name, EndSystemChange, ApplicationInstall);
            }
            catch (COMException e)
            {
                Log.Warn(e);
                return e.ErrorCode;
            }
        }

        private static void CancelRestorePoint(string name)
        {
            try
            {
                Log.Warn("Attempting to cancel system restore point");
                var result = (int)NativeCode.Wmi.SystemRestore.CreateRestorePoint(name, EndSystemChange, CancelledOperation);

                if (result != 0)
                    Log.Warn("An issue (" + result.ToString(CultureInfo.InvariantCulture) +
                                   ") occurred while attempting to close a system restore point");

                Log.Warn("Restore point cancelled successfully");
            }
            catch (COMException e)
            {
                Log.Warn(e);

            }
        }

        private static CouldNotCreateRestorePoint FormatIssue(int result)
        {
            var issue = "[unknown issue]";
            try
            {
                if (result != -1)
                {
                    var exception = new Win32Exception(result);
                    issue = exception.Message;
                }
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }

            return new CouldNotCreateRestorePoint { Issue = issue };
        }

    }


}
