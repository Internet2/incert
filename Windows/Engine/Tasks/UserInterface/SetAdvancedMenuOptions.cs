using System;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class SetAdvancedMenuOptions : AbstractTask
    {
        public SetAdvancedMenuOptions(IEngine engine)
            : base(engine)
        {

        }

        private double? _leftOffset;
        private double? _topOffset;

        [PropertyAllowedFromXml]
        public string DefaultTitle
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string WindowTitle
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string DefaultDescription
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public double InitialLeftOffset { set { _leftOffset = value; } }

        [PropertyAllowedFromXml]
        public double InitialTopOffset { set { _topOffset = value; } }

        [PropertyAllowedFromXml]
        public string HelpTopic
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string DialogBackground
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ContainerBackground
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string TopBannerBackground
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string DialogForeground
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ContainerForeground
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string TopBannerForeground
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string HelpButtonImageKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string HelpButtonMouseOverImageKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string RunButtonImageKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string RunButtonMouseOverImageKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string CloseButtonImageKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string CloseButtonMouseOverImageKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string HelpButtonText
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string RunButtonText
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string CloseButtonText
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (IsPropertySpecified("WindowTitle"))
                    AdvancedMenuManager.WindowTitle = WindowTitle;

                if (IsPropertySpecified("DefaultTitle"))
                    AdvancedMenuManager.DefaultTitle = DefaultTitle;

                if (IsPropertySpecified("DefaultDescription"))
                    AdvancedMenuManager.DefaultDescription = DefaultDescription;

                if (_leftOffset.HasValue)
                    AdvancedMenuManager.InitialLeftOffset = _leftOffset.Value;

                if (_topOffset.HasValue)
                    AdvancedMenuManager.InitialTopOffset = _topOffset.Value;

                if (IsPropertySpecified("HelpTopic"))
                    AdvancedMenuManager.HelpTopic = HelpTopic;

                if (IsPropertySpecified("DialogBackground"))
                    AdvancedMenuManager.DialogBackground = AppearanceManager.GetBrushForColor(DialogBackground,
                        AppearanceManager.BackgroundBrush);

                if (IsPropertySpecified("ContainerBackground"))
                    AdvancedMenuManager.ContainerBackground = AppearanceManager.GetBrushForColor(ContainerBackground,
                        AppearanceManager.BodyTextBrush);

                if (IsPropertySpecified("TopBannerBackground"))
                    AdvancedMenuManager.TopBannerBackground = AppearanceManager.GetBrushForColor(TopBannerBackground,
                        AppearanceManager.BackgroundBrush);

                if (IsPropertySpecified("DialogForeground"))
                    AdvancedMenuManager.DialogForeground = AppearanceManager.GetBrushForColor(DialogForeground,
                        AppearanceManager.BodyTextBrush);

                if (IsPropertySpecified("ContainerForeground"))
                    AdvancedMenuManager.ContainerForeground = AppearanceManager.GetBrushForColor(ContainerForeground,
                        AppearanceManager.BodyTextBrush);

                if (IsPropertySpecified("TopBannerForeground"))
                    AdvancedMenuManager.TopBannerForeground = AppearanceManager.GetBrushForColor(TopBannerForeground,
                        AppearanceManager.BodyTextBrush);

                if (IsPropertySpecified("HelpButtonText"))
                    AdvancedMenuManager.HelpButtonText = HelpButtonText;

                if (IsPropertySpecified("RunButtonText"))
                    AdvancedMenuManager.RunButtonText = RunButtonText;

                if (IsPropertySpecified("CloseButtonText"))
                    AdvancedMenuManager.CloseButtonText = CloseButtonText;

                if (IsPropertySpecified("HelpButtonImageKey"))
                    AdvancedMenuManager.HelpButtonImageKey = HelpButtonImageKey;

                if (IsPropertySpecified("HelpButtonMouseOverImageKey"))
                    AdvancedMenuManager.HelpButtonMouseOverImageKey = HelpButtonMouseOverImageKey;

                if (IsPropertySpecified("RunButtonImageKey"))
                    AdvancedMenuManager.RunButtonImageKey = RunButtonImageKey;

                if (IsPropertySpecified("RunButtonMouseOverImageKey"))
                    AdvancedMenuManager.RunButtonMouseOverImageKey = RunButtonMouseOverImageKey;

                if (IsPropertySpecified("CloseButtonImageKey"))
                    AdvancedMenuManager.CloseButtonImageKey = CloseButtonImageKey;
                
                if (IsPropertySpecified("CloseButtonMouseOverImageKey"))
                AdvancedMenuManager.CloseButtonMouseOverImageKey = CloseButtonMouseOverImageKey;
                
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }


        public override string GetFriendlyName()
        {
            return "Set advanced menu options";
        }
    }
}
