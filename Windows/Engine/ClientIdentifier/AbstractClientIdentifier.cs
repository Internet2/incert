using System;

namespace Org.InCommon.InCert.Engine.ClientIdentifier
{
    public abstract class AbstractClientIdentifier:IClientIdentifier
    {
        protected IClientIdentifier BackupIdentifier { get; private set; }

        protected AbstractClientIdentifier()
        {
            BackupIdentifier = null;
        }

        protected AbstractClientIdentifier(IClientIdentifier backupIdentifier)
        {
            BackupIdentifier = backupIdentifier;
        }
        
        public abstract string GetIdentifier();

        protected string GetBackupIdentifier()
        {
            return BackupIdentifier == null 
                ? Guid.NewGuid().ToString() 
                : BackupIdentifier.GetIdentifier();
        }
    }
}
