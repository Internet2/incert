using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.LinkWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.HyperlinkModels
{
    class TopicHyperlinkModel:AbstractHyperlinkModel
    {
        public string Target { get; private set; }
        
        public TopicHyperlinkModel(AbstractModel parentModel) : base(parentModel)
        {
        }

        public override void LoadContent(AbstractLink wrapper)
        {
            base.LoadContent(wrapper);
            Target = wrapper.Target;
            var commandGroup = new CommandGroup();
            commandGroup.Commands.Add(
                new RelayCommand(param => UserInterfaceUtilities.OpenBrowser(Target)));
            commandGroup.Commands.Add(
                new RelayCommand(param => RootDialogModel.ResetFocus()));
            Command = commandGroup;
        }
    }
}
