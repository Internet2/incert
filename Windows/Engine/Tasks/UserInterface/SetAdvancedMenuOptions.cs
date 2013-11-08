using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;

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

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(WindowTitle))
                    AdvancedMenuManager.WindowTitle = WindowTitle;

                if (!string.IsNullOrWhiteSpace(DefaultTitle))
                    AdvancedMenuManager.DefaultTitle = DefaultTitle;

                if (!string.IsNullOrWhiteSpace(DefaultDescription))
                    AdvancedMenuManager.DefaultDescription = DefaultDescription;

                if (_leftOffset.HasValue)
                    AdvancedMenuManager.InitialLeftOffset = _leftOffset.Value;

                if (_topOffset.HasValue)
                    AdvancedMenuManager.InitialTopOffset = _topOffset.Value;

                if (!string.IsNullOrWhiteSpace(HelpTopic))
                    AdvancedMenuManager.HelpTopic = HelpTopic;

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
