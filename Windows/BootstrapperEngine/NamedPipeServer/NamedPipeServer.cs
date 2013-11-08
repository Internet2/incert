using System;
using System.IO.Pipes;
using System.Text;

namespace Org.InCommon.InCert.BootstrapperEngine.NamedPipeServer
{
    public class PipeServer : IDisposable
    {
        private readonly NamedPipeServerStream _pipe;

        public delegate void MessageReceivedHandler(object sender, MessageReceivedEventArgs e);

        public event MessageReceivedHandler MessageReceived;

        public string ServerName { get; private set; }
        public bool IsOk { get; private set; }
        public Exception Issue { get; private set; }

        protected virtual void OnMessageReceived(string message)
        {
            var handler = MessageReceived;
            if (handler != null) handler(this, new MessageReceivedEventArgs(message));
        }

        public PipeServer(string serverName)
        {
            try
            {
                ServerName = serverName;
                _pipe = new NamedPipeServerStream(ServerName, PipeDirection.In, 5, PipeTransmissionMode.Byte,
                                                  PipeOptions.Asynchronous);
                _pipe.BeginWaitForConnection(ConnectionHandler, _pipe);
                IsOk = true;
            }
            catch (Exception e)
            {
                IsOk = false;
                Issue = e;
            }
        }

        private void ConnectionHandler(IAsyncResult result)
        {
            try
            {
                var server = result.AsyncState as NamedPipeServerStream;
                if (server == null)
                {
                    IsOk = false;
                    Issue = new Exception("Invalid state object encountered");
                    return;
                }

                server.EndWaitForConnection(result);
                var buffer = new byte[255];

                // Read the incoming message
                server.Read(buffer, 0, 255);

                var stringData = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                if (!string.IsNullOrWhiteSpace(stringData))
                    stringData = stringData.TrimEnd('\0');

                OnMessageReceived(stringData);
                server.Close();

                server = new NamedPipeServerStream(ServerName, PipeDirection.In, 5, PipeTransmissionMode.Byte,
                                                   PipeOptions.Asynchronous);
                server.BeginWaitForConnection(ConnectionHandler, server);
            }
            catch (Exception e)
            {
                IsOk = false;
                Issue = e;
            }

        }

        public class MessageReceivedEventArgs : EventArgs
        {
            public MessageReceivedEventArgs(string message)
            {
                Message = message;
            }

            public string Message { get; private set; }
        }

        public void Dispose()
        {
            try
            {
                if (_pipe == null)
                    return;

                _pipe.Close();
                _pipe.Dispose();
            }
            catch (Exception e)
            {
                IsOk = false;
                Issue = e;
            }

        }
    }
}
