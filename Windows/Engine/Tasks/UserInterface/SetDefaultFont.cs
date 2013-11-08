using System;
using System.Windows.Media;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class SetDefaultFont : AbstractTask
    {

        private static readonly ILog Log = Logger.Create();

        [PropertyAllowedFromXml]
        public string FontFamily
        {

            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public SetDefaultFont(IEngine engine)
            : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var font = new FontFamilyConverter().ConvertFromInvariantString(FontFamily) as FontFamily;
                if (font == null)
                {
                    Log.WarnFormat("Cannot convert {0} to a valid font family", FontFamily);
                    return new NextResult();
                }

                AppearanceManager.DefaultFontFamily = font;

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return string.Format("Set style fonts");
        }
    }
}
