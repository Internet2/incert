using System;
using System.Collections.Generic;
using System.Linq;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Settings
{
    class ClearPersistedObject : AbstractTask
    {
        private readonly List<string> _keys = new List<string>();

        public ClearPersistedObject(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string ObjectKey
        {
            set
            {
                _keys.Add(value);
            }
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (!_keys.Any())
                    return new NextResult();

                foreach (var key in _keys)
                {
                    SettingsManager.RemoveTemporaryObject(key);
                }

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Clear persisted object";
        }
    }
}
