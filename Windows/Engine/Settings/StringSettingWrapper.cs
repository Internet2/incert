using System;
using System.Security;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models;

namespace Org.InCommon.InCert.Engine.Settings
{
    public class StringSettingWrapper:IDisposable
    {
        private readonly SecureString _secureContent;
        private readonly string _key;
        public  AbstractModel Model { get; private set; }

        public StringSettingWrapper(string key, string value, AbstractModel model)
        {
            _key = key;
            _secureContent = new SecureString();
            Model = model;
            foreach (var entry in value.ToCharArray())
            {
                _secureContent.AppendChar(entry);
            }   
        }

        public StringSettingWrapper(string key, SecureString value, AbstractModel model)
        {
            _key = key;
            _secureContent = value;
            Model = model;
        }

        public string GetKey()
        {
            return _key;
        }

        public SecureString GetValue()
        {
            return _secureContent;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                
            }

            if (_secureContent!=null)
                _secureContent.Dispose();

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
