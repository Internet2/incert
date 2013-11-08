using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Importables.PropertySetters;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Help
{
    class SetHelpSettings : AbstractTask
    {
        private double? _leftOffset;
        private double? _topOffset;

        public SetHelpSettings(IEngine engine)
            : base (engine)
        {
          
        }

        [PropertyAllowedFromXml]
        public KeyedDynamicStringPropertyEntry BannerEntry { get; set; }

        [PropertyAllowedFromXml]
        public string PreserveContentText
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string DialogTitle
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string AppendWhenShowingExternal
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string BaseHelpUrl
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ReportingEntry
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        [PropertyAllowedFromXml]
        public double InitialLeftOffset { set { _leftOffset = value; } }

        [PropertyAllowedFromXml]
        public double InitialTopOffset { set { _topOffset = value; } }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (BannerEntry != null)
                {
                    HelpManager.HomeUrl = BannerEntry.Key;
                    HelpManager.TopBannerText = BannerEntry.Value;
                }

                if (!string.IsNullOrWhiteSpace(PreserveContentText))
                    HelpManager.PreserveContentText = PreserveContentText;


                if (!string.IsNullOrWhiteSpace(DialogTitle))
                    HelpManager.DialogTitle = DialogTitle;

                if (!string.IsNullOrWhiteSpace(AppendWhenShowingExternal))
                    HelpManager.AppendToExternalUris = AppendWhenShowingExternal;

                if (_leftOffset.HasValue)
                    HelpManager.InitialLeftOffset = _leftOffset.Value;

                if (_topOffset.HasValue)
                    HelpManager.InitialTopOffset = _topOffset.Value;

                if (!string.IsNullOrWhiteSpace(BaseHelpUrl))
                    HelpManager.BaseHelpUrl = BaseHelpUrl;

                if (!string.IsNullOrWhiteSpace(ReportingEntry))
                    HelpManager.ReportingEntry = ReportingEntry;

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Set default help url";
        }
    }
}
