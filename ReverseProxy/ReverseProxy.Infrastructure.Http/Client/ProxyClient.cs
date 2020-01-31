using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using QmcReverseProxy.Infrastructure.Http.Configuration;
using QmcReverseProxy.Infrastructure.Http.Utility;

namespace QmcReverseProxy.Infrastructure.Http.Client
{
    public class ProxyClient : HttpClient
    {
        private readonly HttpContext _context;
        private readonly IApiConfiguration _configuration;

        public ProxyClient(HttpClientHandler handler, HttpContext context, IApiConfiguration configuration)
            : base(handler)
        {
            ServicePointManager.ServerCertificateValidationCallback =
                (callbackSender, certificate, chain, sslPolicyErrors) => true;

            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Handles the request for the given url with the given context and handler.
        /// </summary>
        /// <param name="path">The path to send the request to.</param>
        /// <param name="httpMethod">The HTTP method to use.</param>
        public HttpResponseMessage HandleRequest(string path, string httpMethod)
        {
            return HandleRequest(path, HttpMethodUtilities.Transform(httpMethod));
        }

        /// <summary>
        /// Handles the request for the given url with the given context and handler.
        /// </summary>
        /// <param name="path">The path to send the request to.</param>
        /// <param name="httpMethod">The HTTP method to use.</param>
        public HttpResponseMessage HandleRequest(string path, HttpMethod httpMethod)
        {
            var backendPath = path;
            if (Uri.IsWellFormedUriString(path, UriKind.Absolute))
            {
                // it is absolute?
                try
                {
                    backendPath = new Uri(backendPath, UriKind.Absolute).PathAndQuery;
                }
                catch (Exception)
                {
                    // probably not an absolute url, lets leave it at that
                }
            }

            var backendUri = new Uri(new Uri(_configuration.BaseUrl), backendPath);

            return HandleRequest(backendUri, httpMethod);
        }

        /// <summary>
        /// Handles the request for the given url with the given context and handler.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="httpMethod">The HTTP method to use.</param>
        private HttpResponseMessage HandleRequest(Uri url, HttpMethod httpMethod)
        {
            switch (httpMethod)
            {
                case HttpMethod.POST:
                    using (var receiveStream = _context.Request.InputStream)
                    {
                        using (var readStream = new StreamReader(receiveStream, _context.Request.ContentEncoding))
                        {
                            var requestContent = readStream.ReadToEnd();

                            return base.PostAsync(url, new StringContent(requestContent, _context.Request.ContentEncoding, "application/json")).Result;
                        }
                    }

                case HttpMethod.PUT:
                    using (var receiveStream = _context.Request.InputStream)
                    {
                        using (var readStream = new StreamReader(receiveStream, _context.Request.ContentEncoding))
                        {
                            var requestContent = readStream.ReadToEnd();

                            return base.PutAsync(url, new StringContent(requestContent, _context.Request.ContentEncoding, "application/json")).Result;
                        }
                    }

                case HttpMethod.DELETE:
                    return base.DeleteAsync(url).Result;

                default: // Default is GET

                    return base.GetAsync(url).Result;
            }
        }

        #region overrides

        private readonly string _errorMessage = $"this method is not implemented user '{nameof(HandleRequest)}' instead.";

        public new Task<HttpResponseMessage> DeleteAsync(Uri requestUri)
        {
            throw new NotImplementedException(_errorMessage);
        }

        public new Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            throw new NotImplementedException(_errorMessage);
        }

        public new Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content)
        {
            throw new NotImplementedException(_errorMessage);
        }

        public new Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content)
        {
            throw new NotImplementedException(_errorMessage);
        }

        #endregion
    }
}
