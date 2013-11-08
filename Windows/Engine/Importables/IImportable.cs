using System.Xml.Linq;

namespace Org.InCommon.InCert.Engine.Importables
{
    public interface IImportable
    {
        void ConfigureFromNode(XElement node);
        /*void ResolveDynamicProperties();*/
        bool Initialized();
       
    }
}
