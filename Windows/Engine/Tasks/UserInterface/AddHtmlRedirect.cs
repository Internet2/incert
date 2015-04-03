using System;
using System.Collections.Generic;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Importables.PropertySetters;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    public class AddHtmlRedirect:AbstractTask
    {
        private readonly List<KeyedDynamicStringPropertyEntry> _entries = new List<KeyedDynamicStringPropertyEntry>();

        [PropertyAllowedFromXml]
        public KeyedDynamicStringPropertyEntry Entry
        {
            set
            {
                if (value == null)
                    return;

                _entries.Add(value);
            }
        }
        
        public AddHtmlRedirect(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                foreach (var entry in _entries)
                {
                    BannerManager.AddHtmlRedirect(entry.Key, entry.Value);
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
            return "Add Html Redirects";
        }
    }
}
