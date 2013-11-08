using System;
using System.Collections.Generic;
using System.Linq;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Identity
{
    class GroupPresent : AbstractCondition
    {
        public GroupPresent(IEngine engine)
            : base (engine)
        {
        }

        public string UserGroupsKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public string GroupPath
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(UserGroupsKey))
                    return new BooleanReason(false, "No user properties key specified");

                var groupsList = SettingsManager.GetTemporaryObject(UserGroupsKey) as List<string>;
                if (groupsList == null)
                    return new BooleanReason(false, "Could not retrieve user groups list for key {0}", UserGroupsKey);

                if (!groupsList.Any())
                    return new BooleanReason(false, "Returned user group list is empty");

                if (string.IsNullOrWhiteSpace(GroupPath))
                    return new BooleanReason(false, "No group path specified");

                return groupsList.Any(@group => @group.Equals(GroupPath, StringComparison.InvariantCultureIgnoreCase)) 
                    ? new BooleanReason(true, "Group path {0} present in user's groups list", GroupPath) 
                    : new BooleanReason(false, "group path {0} not present in user's groups list", GroupPath);
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An issue occurred while evaluating the condition: {0}", e.Message);
            }
        }

        public override bool IsInitialized()
        {
            if (!IsPropertySet("UserGroupsKey"))
                return false;

            if (!IsPropertySet("GroupPath"))
                return false;

            return true;
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            GroupPath = XmlUtilities.GetTextFromAttribute(node, "groupPath");
            UserGroupsKey = XmlUtilities.GetTextFromAttribute(node, "userGroupsKey");
        }
    }
}
