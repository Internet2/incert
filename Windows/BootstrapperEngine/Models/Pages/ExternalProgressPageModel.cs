using System;
using Org.InCommon.InCert.BootstrapperEngine.Commands;
using Org.InCommon.InCert.BootstrapperEngine.Logging;
using Org.InCommon.InCert.BootstrapperEngine.NamedPipeServer;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Pages
{
    class ExternalProgressPageModel:ProgressPageModel
    {
        private readonly PipeServer _pipe;
        private const string ErrorPrefix = "error:";
        public ExternalProgressPageModel(PagedViewModel model, string pipeName) : base(model)
        {
            _pipe = new PipeServer(pipeName);
            _pipe.MessageReceived += PipedMessageReceivedHandler;
        }

        void PipedMessageReceivedHandler(object sender, PipeServer.MessageReceivedEventArgs e)
        {
            try
            {
                var message = e.Message;
                if (string.IsNullOrWhiteSpace(message))
                    return;

                if (message.Equals(LaunchEngineCommand.WaitMessage, StringComparison.InvariantCulture))
                    message = "Initializing";

                if (message.StartsWith(ErrorPrefix, StringComparison.InvariantCulture))
                {
                    SetError(message);
                    return;
                }
            
                Model.Dispatcher.Invoke(new Action(()=>Subtitle=message));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void SetError(string message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(message))
                    return;

                Model.ExternalEngine.IssueText = message.TrimStart(ErrorPrefix.ToCharArray());
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            
        }

        public override void Dispose()
        {
            base.Dispose();
            _pipe.Dispose();
        }
    }
}
