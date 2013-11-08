using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Conditions.Encryption
{
    abstract class AbstractEncryptionCondition : AbstractCondition
    {
        private static readonly ILog Log = Logger.Create();

        protected AbstractEncryptionCondition(IEngine engine)
            : base (engine)
        {
        }

        protected static bool IsDriveFixed(string value)
        {
            var name = NormalizeDriveName(value);
            
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("volume name not specified");

            return (from drive in DriveInfo.GetDrives()
                    where drive.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)
                    select drive.DriveType == DriveType.Fixed).FirstOrDefault();
        }

        protected static string NormalizeDriveName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return name;

                var result = name.FirstOrDefault().ToString(CultureInfo.InvariantCulture);
                return string.IsNullOrWhiteSpace(result) ? name : string.Format(@"{0}:\", result);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to normalize the drive name {0}: {1}", name, e.Message);
                return name;
            }

        }
    }
}
