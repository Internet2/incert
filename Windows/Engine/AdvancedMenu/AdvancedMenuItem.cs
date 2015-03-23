using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Org.InCommon.InCert.Engine.Dynamics;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;

namespace Org.InCommon.InCert.Engine.AdvancedMenu
{
    [DataContract]
    public class AdvancedMenuItem : AbstractDynamicPropertyContainer, IAdvancedMenuItem
    {
        public AdvancedMenuItem(IEngine engine)
            : base(engine)
        {
        }

        public bool Show { get { return true; } }

        [DataMember(Name = "group", Order = 1)]
        [PropertyAllowedFromXml]
        public string Group
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [DataMember(Name = "buttonText", Order = 2)]
        [PropertyAllowedFromXml]
        public string ButtonText
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [DataMember(Name = "branch", Order = 3)]
        [PropertyAllowedFromXml]
        public string Branch
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [DataMember(Name = "title", Order = 4)]
        [PropertyAllowedFromXml]
        public string Title
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [DataMember(Name = "description", Order = 5)]
        [PropertyAllowedFromXml]
        public string Description
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }
        
        [DataMember(Name = "workingTitle", Order = 6)]
        [PropertyAllowedFromXml]
        public string WorkingTitle
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [DataMember(Name = "workingDescription", Order = 7)]
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
