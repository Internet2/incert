using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using log4net;
using Org.InCommon.InCert.Engine.Logging;
using RestSharp;
using RestSharp.Extensions;

namespace Org.InCommon.InCert.Engine.WebServices.FactoryProtocols
{
    /// <summary>
    /// An extremely limited implementation of the RestSharp IHttp interface, meant to allow the client to retrieve content from file:// -based uris.  
    /// This should not be used for production purposes.
    /// </summary>
    class FileProtocol : IHttp
    {
        private static readonly ILog Log = Logger.Create();
        private static bool _warningLogged;

        public FileProtocol()
        {
            Headers = new List<HttpHeader>();
            Files = new List<HttpFile>();
            Parameters = new List<HttpParameter>();
            Cookies = new List<HttpCookie>();
        }

        public HttpWebRequest DeleteAsync(Action<HttpResponse> action)
        {
            throw new NotImplementedException();
        }

        public HttpWebRequest GetAsync(Action<HttpResponse> action)
        {
            throw new NotImplementedException();
        }

        public HttpWebRequest HeadAsync(Action<HttpResponse> action)
        {
            throw new NotImplementedException();
        }

        public HttpWebRequest OptionsAsync(Action<HttpResponse> action)
        {
            throw new NotImplementedException();
        }

        public HttpWebRequest PostAsync(Action<HttpResponse> action)
        {
            throw new NotImplementedException();
        }

        public HttpWebRequest PutAsync(Action<HttpResponse> action)
        {
            throw new NotImplementedException();
        }

        public HttpWebRequest PatchAsync(Action<HttpResponse> action)
        {
            throw new NotImplementedException();
        }

        public HttpWebRequest MergeAsync(Action<HttpResponse> action)
        {
            throw new NotImplementedException();
        }

        public HttpWebRequest AsPostAsync(Action<HttpResponse> action, string httpMethod)
        {
            throw new NotImplementedException();
        }

        public HttpWebRequest AsGetAsync(Action<HttpResponse> action, string httpMethod)
        {
            throw new NotImplementedException();
        }

        public HttpResponse Delete()
        {
            throw new NotImplementedException();
        }

        public HttpResponse Get()
        {
            throw new NotImplementedException();
        }

        public HttpResponse Head()
        {
            throw new NotImplementedException();
        }

        public HttpResponse Options()
        {
            throw new NotImplementedException();
        }

        public HttpResponse Post()
        {
            throw new NotImplementedException();
        }

        public HttpResponse Put()
        {
            throw new NotImplementedException();
        }

        public HttpResponse Patch()
        {
            throw new NotImplementedException();
        }

        public HttpResponse Merge()
        {
            throw new NotImplementedException();
        }

        public HttpResponse AsPost(string httpMethod)
        {
            throw new NotImplementedException();
        }

        public HttpResponse AsGet(string httpMethod)
        {
            if (!_warningLogged)
            {
                Log.Warn("The file protocol is only meant for testing and tutorial purposes. It should not be used in production.");
                _warningLogged = true;
            }
            
            return GetMethodInternal();
        }

        // stripped down version of rest-sharp implementation
        private HttpResponse GetMethodInternal()
        {
            var webRequest = ConfigureWebRequest(Url);
            return GetResponse(webRequest);
        }

        // stripped down version of rest-sharp implementation
        private static FileWebRequest ConfigureWebRequest(Uri url)
        {
            return (FileWebRequest)WebRequest.Create(url);
        }

        // stripped down version of rest-sharp implementation
        private HttpResponse GetResponse(WebRequest request)
        {
            var response = new HttpResponse { ResponseStatus = ResponseStatus.None };

            try
            {
                var webResponse = GetRawResponse(request);
                ExtractResponseData(response, webResponse);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.ErrorException = ex;
                response.ResponseStatus = ResponseStatus.Error;
            }

            return response;
        }

        // stripped down version of rest-sharp implementation
        private void ExtractResponseData(IHttpResponse response, WebResponse fileResponse)
        {
            using (fileResponse)
            {
                // here, we want to always use "text/xml"
                response.ContentType = "text/xml";
                response.ContentLength = fileResponse.ContentLength;
                var webResponseStream = fileResponse.GetResponseStream();
                ProcessResponseStream(webResponseStream, response);

                // response should always be ok
                response.StatusCode = HttpStatusCode.OK;
                response.ResponseUri = fileResponse.ResponseUri;
                response.ResponseStatus = ResponseStatus.Completed;

                foreach (var headerName in fileResponse.Headers.AllKeys)
                {
                    var headerValue = fileResponse.Headers[headerName];
                    response.Headers.Add(new HttpHeader { Name = headerName, Value = headerValue });
                }

                fileResponse.Close();
            }
        }

        // stripped down version of rest-sharp implementation
        private void ProcessResponseStream(Stream webResponseStream, IHttpResponse response)
        {
            if (ResponseWriter == null)
            {
                response.RawBytes = webResponseStream.ReadAsBytes();
            }
            else
            {
                ResponseWriter(webResponseStream);
            }
        }

        // stripped down version of rest-sharp implementation
        private static FileWebResponse GetRawResponse(WebRequest request)
        {
            try
            {
                return (FileWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                if (ex.Response is FileWebResponse)
                {
                    return ex.Response as FileWebResponse;
                }
                throw;
            }
        }

        public Action<Stream> ResponseWriter { get; set; }
        public CookieContainer CookieContainer { get; set; }
        public ICredentials Credentials { get; set; }
        public bool AlwaysMultipartFormData { get; set; }
        public string UserAgent { get; set; }
        public int Timeout { get; set; }

        public int ReadWriteTimeout
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool FollowRedirects { get; set; }
        public X509CertificateCollection ClientCertificates { get; set; }
        public int? MaxRedirects { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public IList<HttpHeader> Headers { get; private set; }
        public IList<HttpParameter> Parameters { get; private set; }
        public IList<HttpFile> Files { get; private set; }
        public IList<HttpCookie> Cookies { get; private set; }
        public string RequestBody { get; set; }
        public string RequestContentType { get; set; }

        public bool PreAuthenticate
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public byte[] RequestBodyBytes { get; set; }
        public Uri Url { get; set; }
        public IWebProxy Proxy { get; set; }

    }
}
