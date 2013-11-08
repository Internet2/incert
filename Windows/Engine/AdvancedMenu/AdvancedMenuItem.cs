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
}
