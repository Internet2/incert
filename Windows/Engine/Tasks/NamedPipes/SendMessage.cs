using System;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.NamedPipes
{
    class SendMessage : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();
        
        [PropertyAllowedFromXml]
        public string PipeName
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string Message
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public int Timeout { get; set; }

        [PropertyAllowedFromXml]
        public bool Async { get; set; }

        public SendMessage(IEngine engine)
            : base(engine)
        {
            // default 1 second timeout
            Timeout = 1;
            Async = true;
        }


        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var task = new Task(() => Send(Message));
                
                if (!Async)
                    task.RunSynchronously();
                else
                    task.Start();
                
                return new NextResult();
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while trying to send '{0}' to pipe '{1}': {2}", Message, PipeName, e.Message);
                return new NextResult();
            }
        }

        
        private void Send(string message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(message))
                    return;
                
                using (var pipeStream = new NamedPipeClientStream
                    (".", PipeName, PipeDirection.Out, PipeOptions.Asynchronous))
                {
                    pipeStream.Connect(Timeout*1000);

                    var buffer = Encoding.UTF8.GetBytes(message);
                    pipeStream.Write(buffer, 0, buffer.Length); 
                    pipeStream.Flush();
                    pipeStream.WaitForPipeDrain();
                }
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while trying to send '{0}' to pipe '{1}': {2}", Message, PipeName, e.Message);
            }
        }

      

        public override string GetFriendlyName()
        {
            return string.Format("Send message ({0}) to named pipe ({1})", Message, PipeName);
        }
    }
}
