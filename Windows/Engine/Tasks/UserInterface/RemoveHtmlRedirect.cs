using System;
using System.Collections.Generic;
using log4net;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    public class RemoveHtmlRedirect : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();
        private readonly List<string> _keys = new List<string>();

        public RemoveHtmlRedirect(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Entry
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Log.Warn("Cannot add key to values collection; key cannot be null or whitespace.");
                    return;
                }

                _keys.Add(value);
            }
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                foreach (var key in _keys)
                {
                    BannerManager.RemoveHtmlRedirect(key);
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
            return "Remove Html Redirects";
        }
    }
}