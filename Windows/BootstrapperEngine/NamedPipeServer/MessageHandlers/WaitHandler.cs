using System;

namespace Org.InCommon.InCert.BootstrapperEngine.NamedPipeServer.MessageHandlers
{
    public class WaitHandler:AbstractHandler
    {
        private readonly string _desiredMessage;
        public bool Received { get; private set; }

        public WaitHandler(PipeServer pipe, string desiredMessage) : base(pipe)
        {
            _desiredMessage = desiredMessage;
           
        }

        protected override void MessageReceivedHandler(object sender, PipeServer.MessageReceivedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Message))
                return;

            Received = e.Message.Equals(_desiredMessage, StringComparison.InvariantCulture);
        }
    }   
}
