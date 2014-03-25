using System;
using System.IO;
using System.Windows.Media.Imaging;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class SetApplicationIcon : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        [PropertyAllowedFromXml]
        public string Icon
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public SetApplicationIcon(IEngine engine)
            : base(engine)
        {
          
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Icon))
                {
                    Log.Warn("Cannot set default icon; no icon specified");
                    return new NextResult();
                }

                var path = Path.Combine(PathUtilities.IconFolder, Icon);
                if (!File.Exists(path))
                {
                    Log.WarnFormat("Cannot set default icon; {0} not found in icon folder.", Icon);
                    return new NextResult();
                }

                AppearanceManager.ApplicationIcon = BitmapFrame.Create(new Uri(path, UriKind.Absolute));

                return new NextResult();
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return new NextResult();
            }

        }

        public override string GetFriendlyName()
        {
            return "Set default icon";
        }
    }
}
