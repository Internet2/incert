using System.Linq;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.UserInterface
{
    class BannerControlExists:AbstractCondition
    {
        private string _key;
        private string _dialog;
        
        public BannerControlExists(IEngine engine):base(engine)
        {
        }
        public override BooleanReason Evaluate()
        {
            var dialog = DialogsManager.GetExistingDialog(_dialog);
            if (dialog == null)
                return new BooleanReason(false, "Cannot retrieve dialog for key {0}", _dialog);

            var instances = dialog.GetModelsByKey(_key);
            return !instances.Any() 
                ? new BooleanReason(false, "no instances of {0} found", _key) 
                : new BooleanReason(true, "at least one instance of {0} found", _key);
        }

        public override bool IsInitialized()
        {
            if (string.IsNullOrWhiteSpace(_key))
                return false;

            if (string.IsNullOrWhiteSpace(_dialog))
                return false;

            return true;
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            _key = XmlUtilities.GetTextFromAttribute(node, "key");
            _dialog = XmlUtilities.GetTextFromAttribute(node, "dialog");
        }
    }
}
