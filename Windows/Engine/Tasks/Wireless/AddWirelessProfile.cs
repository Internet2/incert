using System;
using System.IO;
using System.Linq;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Wireless;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.Wireless
{
    class AddWirelessProfile:AbstractTask
    {
        public AddWirelessProfile(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string ProfilePath
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }
        
        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ProfilePath))
                    return new CouldNotAddWirelessProfile {Issue = "No profile specified"};

                if (!File.Exists(ProfilePath))
                    return new CouldNotAddWirelessProfile{Issue = "Profile path is null or invalid"};

                var profileText = File.ReadAllText(ProfilePath);
                if (string.IsNullOrWhiteSpace(profileText))
                    return new CouldNotAddWirelessProfile{Issue ="Could not generate profile text"};

                foreach (var result in NetworkUtilities.GetWirelessAdapters()
                    .Select(adapter => NetworkUtilities.AddWirelessProfile(adapter, profileText))
                    .Where(result => !result.Result))
                {
                    return new CouldNotAddWirelessProfile{Issue = result.Reason};
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
            return "Add wireless profile";
        }
    }
}
