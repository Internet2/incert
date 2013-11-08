using System;
using System.Windows;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.WebServices.Contracts;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using log4net;
using log4net.Appender;
using log4net.Core;

namespace Org.InCommon.InCert.Engine.Logging
{
    class WebServiceAppender:AppenderSkeleton
    {
        private readonly IEndpointManager _endpointManager;
        public bool UploadEvents { get; set; }

        public long MachineId { get; private set; }
        public long SessionId { get; private set; }

        public WebServiceAppender(IEndpointManager endpointManager)
        {
            _endpointManager = endpointManager;
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                if (!UploadEvents)
                    return;

                EndPointFunctions function;
                if (loggingEvent is BlockingLoggingEvent)
                    function = EndPointFunctions.LogWait;
                else
                    function = EndPointFunctions.LogAsync;

                var request = _endpointManager.GetContract<AbstractLoggingContract>(function);
                request.Message = RenderLoggingEvent(loggingEvent);
                request.Event = loggingEvent.Level.ToString();
                request.User = ThreadContext.Properties["UserId"].ToStringOrDefault("[Unknown]");
                request.Session = Application.Current.GetSessionId();
                request.Machine = Application.Current.GetIdentifier();

                var result = request.MakeRequest<LogInfoWrapper>();

                if (result == null)
                    throw request.GetError();

                SetInfoFields(result);
            }
            catch (Exception e)
            {
                ErrorHandler.Error("An error occurred while attempting to upload a logevent", e);
            }
        }

        private void SetInfoFields(LogInfoWrapper infoWrapper)
        {
            if (infoWrapper == null)
                return;

            if (infoWrapper.ComputerId >0)
                MachineId = infoWrapper.ComputerId;

            if (infoWrapper.SessionId >0)
                SessionId = infoWrapper.SessionId;
        }

    }
}
