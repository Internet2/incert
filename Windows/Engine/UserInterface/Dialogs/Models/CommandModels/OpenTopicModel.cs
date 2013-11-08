using System.Windows;
using System.Windows.Input;
using Org.InCommon.InCert.Engine.Help;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.CommandModels
{
    class OpenTopicModel:AbstractCommandModel
    {
        private readonly string _topic;
        private readonly IHelpManager _helpManager;
        private ICommand _command;

        public OpenTopicModel(AbstractDialogModel model, string topic, IHelpManager helpManager)
            : base(model)
        {
            _topic = topic;
            _helpManager = helpManager;
        }

        public override Visibility Visibility
        {
            get
            {
                return !_helpManager.TopicExists(_topic) ? Visibility.Collapsed : base.Visibility;
            }
        }

        public override ICommand Command
        {
            get
            {
                return _command ?? (
                    _command = new ClearFocusCommand(
                       RootDialogModel.DialogInstance,
                       param => _helpManager.ShowHelpTopic(_topic, RootDialogModel)));
            }
        }
    }
}
