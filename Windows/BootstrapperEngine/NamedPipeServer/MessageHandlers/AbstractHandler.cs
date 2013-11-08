namespace Org.InCommon.InCert.BootstrapperEngine.NamedPipeServer.MessageHandlers
{
    public abstract class AbstractHandler
    {
        protected AbstractHandler(PipeServer pipe)
        {
            pipe.MessageReceived += MessageReceivedHandler;
        }

        protected abstract void MessageReceivedHandler(object sender, PipeServer.MessageReceivedEventArgs e);
    }
}
