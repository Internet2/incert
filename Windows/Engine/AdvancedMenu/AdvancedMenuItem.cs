using Newtonsoft.Json;
using Org.InCommon.InCert.Engine.Dynamics;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.AdvancedMenu
{
    public class AdvancedMenuItem : AbstractDynamicPropertyContainer, IAdvancedMenuItem
    {
        public AdvancedMenuItem(IEngine engine)
            : base(engine)
        {
        }

        public bool Show { get { return true; } }

        [PropertyAllowedFromXml]
        public string Group
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ButtonText
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string Branch
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string Title
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string Description
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }


        [PropertyAllowedFromXml]
        public string WorkingTitle
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string WorkingDescription
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override bool Initialized()
        {
            if (!IsPropertySet("Group"))
                return false;

            if (!IsPropertySet("ButtonText"))
                return false;

            if (!IsPropertySet("Branch"))
                return false;

            if (!IsPropertySet("Title"))
                return false;

            if (!IsPropertySet("WorkingTitle"))
                return false;

            if (!IsPropertySet("WorkingDescription"))
                return false;

            return true;
        }
    }

    public class AdvancedMenuExportable : IAdvancedMenuItem
    {
        public AdvancedMenuExportable(IAdvancedMenuItem menuItem)
        {
            Show = menuItem.Show;
            Group = menuItem.Group;
            ButtonText = menuItem.ButtonText;
            Branch = menuItem.Branch;
            Title = menuItem.Title;
            Description = menuItem.Description;
            WorkingTitle = menuItem.WorkingTitle;
            WorkingDescription = menuItem.WorkingDescription;
        }

        [JsonProperty("show")]
        public bool Show { get; private set; }

        [JsonProperty("group")]
        public string Group
        {
            get;
            set;
        }

        [JsonProperty("buttonText")]
        public string ButtonText
        {
            get;
            set;
        }

        [JsonProperty("branch")]
        public string Branch
        {
            get;
            set;
        }

        [JsonProperty("title")]
        public string Title
        {
            get;
            set;
        }

        [JsonProperty("description")]
        public string Description
        {
            get;
            set;
        }

        [JsonProperty("workingTitle")]
        public string WorkingTitle
        {
            get;
            set;
        }

        [JsonProperty("workingDescription")]
        public string WorkingDescription
        {
            get;
            set;
        }
    }
}
