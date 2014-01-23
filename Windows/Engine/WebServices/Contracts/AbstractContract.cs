using System;
using System.Activities.Statements;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Utilities;
using Org.InCommon.InCert.Engine.WebServices.Exceptions;
using Org.InCommon.InCert.Engine.WebServices.FactoryProtocols;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using RestSharp;
using log4net;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts
{


    public abstract class AbstractContract
    {
        protected readonly IEndpointManager EndpointManager;
        private static readonly ILog Log = Logger.Create();

        public bool IgnoreCertificateErrors { get; set; }
        public bool AssumeConnectionError { get; set; }
        public bool UseResetSafeCall { get; set; }
        public int Timeout { get; set; }
        public int MaxRetries { get; set; }

        private Exception _issue;

        protected abstract IRestRequest GetRequestObject();
        public abstract EndPointFunctions SupportedFunction { get; }

        protected AbstractContract(IEndpointManager endpointManager)
        {
            EndpointManager = endpointManager;
            Timeout = 30000;
            MaxRetries = 3;
        }

        public Exception GetError()
        {
            return _issue ?? new Exception("Unknown issue");
        }

        public void SetError(Exception value)
        {
            _issue = value;
        }

        public virtual string GetEndpointUrl()
        {
            return EndpointManager.GetEndpointForFunction(SupportedFunction);
        }
        
        public virtual T MakeRequest<T>() where T : class,new()
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += ServerCertificateValidationCallback;
                var count = 0;

                while (count < MaxRetries)
                {
                    var result = TryRequest<T>();
                    if (result != null)
                        return result;

                    if (IsIssueFinal())
                        return default(T);

                    InterveneIfConnectionLost();

                    UserInterfaceUtilities.WaitForSeconds(DateTime.UtcNow, 1);
                    count++;
                }

                return default(T);
            }
            catch (Exception e)
            {
                Log.Warn(e);
                SetError(e);
                return null;
            }
            finally
            {
                if (ServicePointManager.ServerCertificateValidationCallback == null)
                    ServicePointManager.ServerCertificateValidationCallback = null;
            }
        }

        protected virtual void InterveneIfConnectionLost()
        {
            if (NetworkUtilities.AnyActualAdapterWithValidLease())
                return;

            Log.Warn("No network adapters found with valid lease. Attempting to renew lease.");
            NetworkUtilities.ReleaseAndRenewInterfaces();
        }

        protected virtual T TryRequest<T>() where T : class,new()
        {
            try
            {
                var client = GetClient();

                if (string.IsNullOrWhiteSpace(client.BaseUrl))
                {
                    SetError(
                        new Exception("Endpoint manager returned empty endpoint url for " + GetType().Name));
                    return default(T);
                }

                var request = GetRequestObject();
                
                using (var queryTask = Task<IRestResponse<T>>.Factory.StartNew(() => client.Execute<T>(request)))
                {

                    queryTask.WaitUntilExited();

                    if (queryTask.IsFaulted)
                    {
                        SetError(queryTask.Exception);
                        return default(T);
                    }

                    var result = queryTask.Result;

                    if (ErrorOccurred(result))
                    {
                        Log.Warn("An error while attempting to perform the web-service request");
                        SetError(GetExceptionFromRestResponse(result));
                        return default(T);
                    }

                    return ProcessResults(result.Data);
                }
            }
            catch (AggregateException e)
            {
                Log.Warn(e);
                SetError(e.GetBaseException());
                return default(T);
            }
            catch (Exception e)
            {
                Log.Warn(e);
                SetError(e);
                return default(T);
            }
        }

        private bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            return IgnoreCertificateErrors;
        }

        protected virtual bool IsIssueFinal()
        {
            var issue = GetError();
            if (issue == null)
                return false;

            return issue as WebServiceException != null;
        }

        protected virtual T ProcessResults<T>(T result) where T : class, new()
        {
            return result;
        }

        protected virtual bool ErrorOccurred(IRestResponse result)
        {
            if (result.ResponseStatus != ResponseStatus.Completed)
            {
                Log.WarnFormat("issue occurred while attempting to complete web-service call: response status ({0}) is not 'completed'", result.ResponseStatus);
                return true;
            }

            if (result.StatusCode != HttpStatusCode.OK)
            {
                Log.WarnFormat("issue occurred while attempting to complete web-service call: {0}", result.StatusCode);
                return true;
            }

            return false;
        }

        protected virtual bool IsHttpIssue(HttpStatusCode status)
        {
            if (status == HttpStatusCode.OK) return false;
            
            if (status == HttpStatusCode.NoContent) return false;

            return true;
        }

        protected virtual Exception GetExceptionFromRestResponse(IRestResponse response)
        {
            if (response == null)
            {
                Log.Warn("web-service response body is empty - returning 'unknown issue'");
                return new Exception("unknown issue");
            }

            if (IsHttpIssue(response.StatusCode))
            {
                Log.WarnFormat("received custom issue feedback from server: status code = {0} ({1}); desc = {2}", response.StatusCode, (int)response.StatusCode, response.StatusDescription);
                return new WebServiceException(string.Format("{0}", response.StatusDescription));    
            }
            
            if (response.ErrorException != null)
            {
                Log.WarnFormat("web-service response has exception set: {0}", response.ErrorException.Message);
                return response.ErrorException;
            }

            if (!string.IsNullOrWhiteSpace(response.ErrorMessage))
            {
                Log.WarnFormat("web-service response has error message set: {0}", response.ErrorMessage);
                return new Exception(response.ErrorMessage);
            }

            return new Exception("An unknown issue has occurred");
        }

        public abstract IResult GetErrorResult();
        
        protected virtual RestClient GetClient()
        {
            var url = GetEndpointUrl();
            var client = new RestClient(url)
            {
                Timeout = Timeout, 
                HttpFactory = GetHttpFactory(url),
            };
            return client;
        }

        protected virtual IHttpFactory GetHttpFactory(string requestUrl)
        {
            try
            {
                var uri = new Uri(requestUrl, UriKind.RelativeOrAbsolute);
                if (uri.IsFile)
                    return new SimpleFactory<FileProtocol>();

                return new SimpleFactory<Http>();
            }
            catch (Exception e)
            {
                Log.WarnFormat(
                    "An issue occurred while determining the httpfactory for the url {0}: {1}. Using default factory.",
                    requestUrl, e.Message);
                return new SimpleFactory<Http>();
            }
        }

        



    }
}
