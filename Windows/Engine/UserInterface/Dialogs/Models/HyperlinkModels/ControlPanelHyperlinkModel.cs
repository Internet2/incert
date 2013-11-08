using System;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.LinkWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.HyperlinkModels
{
    class ControlPanelHyperlinkModel:AbstractHyperlinkModel
    {
        public ControlPanelHyperlinkModel(AbstractModel parentModel) : base(parentModel)
        {
        }

        public override void LoadContent(AbstractLink wrapper)
        {
            base.LoadContent(wrapper);

            SystemUtilities.ControlPanelNames target;
            Enum.TryParse(wrapper.Target, out target);
            var commandGroup = new CommandGroup();
            commandGroup.Commands.Add(
                new RelayCommand(param => SystemUtilities.OpenControlPanel(target)));
            commandGroup.Commands.Add(
                new RelayCommand(param => RootDialogModel.ResetFocus()));
            Command = commandGroup;
        }
    }
}
