using System;
using System.Collections.Generic;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Text
{
    public class AddStringToList:AbstractTask
    {
        public AddStringToList(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Value
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        [PropertyAllowedFromXml]
        public string ListKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Value))
                    return new NextResult();

                if (string.IsNullOrWhiteSpace(ListKey))
                    throw  new Exception("List key must be specified");

                var list = SettingsManager.GetTemporaryObject(ListKey) as List<string> ?? new List<string>();

                list.Add(Value);

                SettingsManager.SetTemporaryObject(ListKey, list);
                
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Add value to list";
        }
    }
}
