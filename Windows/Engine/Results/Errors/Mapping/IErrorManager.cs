using System.Collections.Generic;
using System.Xml.Linq;

namespace Org.InCommon.InCert.Engine.Results.Errors.Mapping
{
    public interface IErrorManager
    {
        bool ImportEntriesFromXml(XElement node);
        bool ImportErrorEntries(List<AbstractErrorEntry> entries);
        AbstractErrorEntry GetErrorEntry(ErrorResult issue);
        void Initialize();
    }
}