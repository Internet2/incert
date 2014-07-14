using System.Collections.Generic;
using System.Xml.Linq;

namespace Org.InCommon.InCert.Engine.Importables
{
    public interface IImportable
    {
        void ConfigureFromNode(XElement node);
        bool Initialized();
        bool IsPropertySpecified(string name);

    }
}
