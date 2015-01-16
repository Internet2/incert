using System;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances.CustomControls;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels
{
    public class BrowserContentModel:AbstractContentModel
    {
        public BrowserContentModel(AbstractModel parentModel) : base(parentModel)
        {
            
        }

        public override T LoadContent<T>(AbstractContentWrapper wrapper)
        {
            
            
            var content = new BrowserControl {DataContext = this};
            InitializeBindings(content);
            InitializeValues(wrapper);

            var browserWrapper = wrapper as BrowserContentWrapper;
            if (browserWrapper == null)
            {
                throw new InvalidCastException("Could not cast wrapper to valid type");
            }

            var uri = new Uri(browserWrapper.Url);
            content.Browser.ObjectForScripting = new ScriptingModel(wrapper.Engine, RootDialogModel);
            content.Browser.Navigate(uri, null, null, "Incert: true\r\n");
            
            Content = content;

            return content as T;

        }



        
    }
}
