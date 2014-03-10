using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.InCommon.InCert.Engine.Properties;

namespace Org.InCommon.InCert.Engine.ClientIdentifier
{
    public interface IClientIdentifier
    {
        string GetIdentifier();
    }
}
