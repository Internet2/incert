using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.WebServices
{
    class GetBaseUrlFromCertificate:AbstractTask
    {
        private const string SubjectAlternativeNameOid = "2.5.29.17";
        private static readonly ILog Log = Logger.Create();

        public GetBaseUrlFromCertificate(IEngine engine):base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var target = Assembly.GetExecutingAssembly().Location;
                var import = X509Certificate.CreateFromSignedFile(target);
                var certificate = new X509Certificate2(import);

                var rawValue = GetSubjectAlternativeNameOid(certificate.Extensions);
                if (rawValue == null)
                {
                    Log.Warn("Could not extract SAN from certificate");
                    return new NextResult();
                }

                var value = rawValue.Format(false);
                if (string.IsNullOrWhiteSpace(value))
                {
                    Log.Warn("Could not extract SAN from certificate");
                    return new NextResult();
                }

                var values = value.Split(',');
                var entryDictionary = new Dictionary<string, string>();
                foreach (var entry in values)
                {
                    if (string.IsNullOrWhiteSpace(entry))
                        continue;

                    var entryParts = entry.Split('=');
                    if (entryParts.Length !=2)
                        continue;

                    if (string.IsNullOrWhiteSpace(entryParts[0]))
                        continue;
                    
                    if (string.IsNullOrWhiteSpace(entryParts[1]))
                        continue;

                    entryDictionary[entryParts[0].Trim()] = entryParts[1].Trim();
                }

                if (!entryDictionary.ContainsKey("URL"))
                    return new NextResult();

                SettingsManager.BaseWebServiceUrl = entryDictionary["URL"];
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }

        }

        private static AsnEncodedData GetSubjectAlternativeNameOid(ICollection extensions)
        {
            if (extensions == null)
                return null;

            if (extensions.Count == 0)
                return null;
            
            return (from X509Extension extension in extensions where extension.Oid.Value.Equals(SubjectAlternativeNameOid, StringComparison.InvariantCulture) select new AsnEncodedData(extension.Oid, extension.RawData)).FirstOrDefault();
        }

        public override string GetFriendlyName()
        {
            return "Get base url from certificate";
        }
    }
}
