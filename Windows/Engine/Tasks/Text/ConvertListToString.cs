using System;
using System.Collections.Generic;
using System.Linq;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Text
{
    public class ConvertListToString:AbstractTask
    {
        public ConvertListToString(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string ListKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string SettingKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ListKey))
                    throw new Exception("List key cannot be empty");

                if (string.IsNullOrWhiteSpace(SettingKey))
                    throw  new Exception("Setting key cannot be empty");

                var list = SettingsManager.GetTemporaryObject(ListKey) as List<string>;
                if (list == null)
                    throw  new Exception("List not present in temporary store");

                if (!list.Any())
                    throw new Exception("List has no members");

                if (list.Count == 1)
                {
                    SettingsManager.SetTemporarySettingString(SettingKey, list[0]);
                    return new NextResult();
                }

                if (list.Count == 2)
                {
                    SettingsManager.SetTemporarySettingString(SettingKey,
                        string.Format("{0} and {1}", list.First(), list.Last()));
                    return new NextResult();
                }

                var result = string.Join(", ", list);
                result = result.Replace(", " + list.Last(), ", and " + list.Last());
                SettingsManager.SetTemporarySettingString(SettingKey, result);

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Convert list to string";
        }
    }
}
