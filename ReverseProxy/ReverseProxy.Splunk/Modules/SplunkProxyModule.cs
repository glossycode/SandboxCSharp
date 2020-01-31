using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using log4net;
using QmcReverseProxy.Infrastructure.Http.Client;
using QmcReverseProxy.Infrastructure.Http.Utility;
using QmcReverseProxy.Splunk.Configuration;

namespace QmcReverseProxy.Splunk.Modules
{
    public class SplunkProxyModule : IHttpModule
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<string> _excludedHeaders = new List<string> { "Transfer-Encoding", "X-Frame-Options" };

        private readonly Regex _loginUrlRegex = new Regex(@".*/account/login.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly Regex _headerRegex = new Regex(@"<header .*</header>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex _dashboardRegex = new Regex(@".*/dashboard.js.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly Regex _userNameUrlRegex = new Regex(@"(/undefined)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex _applicationRegex = new Regex(@"(/-/-/)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex _applicationRegex2 = new Regex(@"(/-/)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Regex to identify a request on a Power BI Report.
        /// Note, that the query string parameter 'rs:embed' with the value 'true' is mandatory.
        /// Note, that the query string parameter 'sessionId' with a guid value must be available
        /// The two query string parameters can be in any order.
        /// </summary>
        private readonly Regex _splunkReportRequestRegex = new Regex(@"^(?!.*return_to).*(?=.*\b(&|\?){1}reportInfoId=(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}\b)($|&.*|\?.*){1}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex _splunkUhdRequestRegex = new Regex(@"^(?!.*return_to).*(?=.*\b(&|\?){1}deviceId=[0-9a-fA-F]{12}\b)($|&.*|\?.*){1}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex _splunkFileRequest = new Regex(@"^.*(/static/|/config|/__raw/|/i18ncatalog).*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private const string ReportInfoCookieName = "SplunkReportInfo";
        private const string UhdInfoCookieName = "SplunkUhdInfo";

        public void Init(HttpApplication context)
        {
            context.BeginRequest += HandleUhdRequest;
        }

        public void Dispose()
        { }


        private void HandleUhdRequest(object sender, EventArgs e)
        {
            var configuration = new SplunkApiConfiguration();
            var application = (HttpApplication)sender;
            var context = application.Context;
            var rawUrl = _userNameUrlRegex.Replace(_applicationRegex2.Replace(_applicationRegex.Replace(context.Request.RawUrl, $"/{configuration.UserName}/launcher/"), "/launcher/"), $"/{configuration.UserName}/");

            var deviceAddress = _splunkUhdRequestRegex.IsMatch(rawUrl)
                ? context.Request.QueryString["deviceId"]
                : context.Request.Cookies[UhdInfoCookieName]?.Value;

            var un = !_splunkFileRequest.IsMatch(rawUrl) && string.IsNullOrEmpty(deviceAddress);
            var deux = _splunkUhdRequestRegex.IsMatch(rawUrl) && !Regex.IsMatch(rawUrl, $".*{Regex.Escape(deviceAddress)}.*", RegexOptions.IgnoreCase);

            var cookie1 = context.Request.Cookies[ReportInfoCookieName]?.Value;
            var cookie2 = context.Request.Cookies[UhdInfoCookieName]?.Value;

            var cond1 = !_splunkFileRequest.IsMatch(rawUrl) && string.IsNullOrEmpty(deviceAddress) ;
            var cond2 = (_splunkUhdRequestRegex.IsMatch(rawUrl) && !Regex.IsMatch(rawUrl, $".*{Regex.Escape(deviceAddress)}.*", RegexOptions.IgnoreCase));

            if (!_splunkFileRequest.IsMatch(rawUrl) && string.IsNullOrEmpty(deviceAddress) || (_splunkUhdRequestRegex.IsMatch(rawUrl) && !Regex.IsMatch(rawUrl, $".*{Regex.Escape(deviceAddress)}.*", RegexOptions.IgnoreCase)))
            {
                return;
            }

            HandleRequest(configuration, context, rawUrl);
        }
        
        private void HandleRequest(SplunkApiConfiguration configuration, HttpContext context, string rawUrl)
        {
            string redirect = null;

            using (var handler = CreateHandler())
            {
                using (var proxy = new ProxyClient(handler, context, configuration))
                {
                    try
                    {
                        ProxyUtilities.MapHeaders(context.Request, proxy, _excludedHeaders);
                        ProxyUtilities.MapCookies(context.Request, handler, configuration);

                        Log.Debug($"Partner KPI: new request for: '{rawUrl}'.");
                        var response = proxy.HandleRequest(rawUrl, context.Request.HttpMethod);
                        Log.Debug($"Partner KPI: response received: {(int)response.StatusCode} {response.StatusCode}.");

                        var cookies = ProxyUtilities.GetAllCookies(handler.CookieContainer);

                        if (((int)response.StatusCode).ToString().StartsWith("3"))
                        {
                            // HTTP Status: 300 - 399
                            redirect = response.Headers.Location?.PathAndQuery;
                        }

                        ProxyUtilities.MapHeaders(response, context.Response, _excludedHeaders);
                        ProxyUtilities.MapCookies(cookies, context.Response);

                        var contentType = response.Content.Headers?.ContentType?.MediaType ?? context.Request.ContentType;
                        var content = _dashboardRegex.IsMatch(rawUrl) || string.Equals(contentType, "text/html", StringComparison.OrdinalIgnoreCase)
                            ? Encoding.UTF8.GetBytes(_headerRegex.Replace(response.Content.ReadAsStringAsync().Result, string.Empty))
                            : response.Content.ReadAsByteArrayAsync().Result;

                        context.Response.BinaryWrite(content);
                        context.Response.ContentType = string.IsNullOrWhiteSpace(contentType) ? "text/html" : contentType;
                        context.Response.StatusCode = (int)response.StatusCode;

                        if (_loginUrlRegex.IsMatch(redirect ?? string.Empty))
                        {
                            Log.Debug("Partner KPI: auto logging in user with configured credentials.");

                            var query = HttpUtility.ParseQueryString(response.Headers.Location?.Query ?? string.Empty);
                            var returnTo = query.Get("return_to");
                            var loginUrl = $"{configuration.LoginPath}?username={configuration.UserName}&password={configuration.Password}{(string.IsNullOrWhiteSpace(returnTo) ? string.Empty : $"&return_to={returnTo}")}";

                            context.Response.RedirectLocation = loginUrl;
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(redirect))
                            {
                                Log.Debug($"Partner KPI: sending redirect response to: '{redirect}'.");
                            }

                            context.Response.RedirectLocation = redirect;
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Error($"Partner KPI: Error sending the request: '{exception.Message}'", exception);
                    }
                }
            }

            context.Response.End();
        }

        private HttpClientHandler CreateHandler()
        {
            return new HttpClientHandler
            {
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                UseCookies = true,
                CookieContainer = new CookieContainer()
            };
        }
    }
}
