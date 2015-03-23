using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Org.InCommon.InCert.Engine.Dynamics;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;

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

    [DataContract]
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

        [DataMember(Name = "show", Order = 1)]
        public bool Show { get; private set; }

        [DataMember(Name = "group", Order = 2)]
        public string Group
        {
            get;
            set;
        }

        [DataMember(Name = "buttonText", Order = 3)]
        public string ButtonText
        {
            get;
            set;
        }

        [DataMember(Name = "branch", Order = 4)]
        public string Branch
        {
            get;
            set;
        }

        [DataMember(Name = "title", Order = 5)]
        public string Title
        {
            get;
            set;
        }

        [DataMember(Name = "description", Order = 6)]
        public string Description
        {
            get;
            set;
        }

        [DataMember(Name = "workingTitle", Order = 7)]
        public string WorkingTitle
        {
            get;
            set;
        }

        [DataMember(Name = "workingDescription", Order = 8)]
        public string WorkingDescription
        {
            get;
            set;
        }

        public string ToJson()
        {
            var serializer = new DataContractJsonSerializer(GetType());
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, this);
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static string ToJson(AdvancedMenuExportable[] values)
        {
            var serializer = new DataContractJsonSerializer(typeof(AdvancedMenuExportable[]));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, values);
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
