using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using QmcReverseProxy.Infrastructure.Http.Client;
using QmcReverseProxy.Infrastructure.Http.Configuration;

namespace QmcReverseProxy.Infrastructure.Http.Utility
{
    public static class ProxyUtilities
    {
        /// <summary>
        /// Adds all headers with exception to Content headers from a given response to a proxy response.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="proxy"></param>
        /// <param name="excludedHeaders">Names of headers which should not be mapped.</param>
        public static void MapHeaders(HttpRequest request, ProxyClient proxy, List<string> excludedHeaders = null)
        {
            foreach (string headerName in request.Headers)
            {
                if (headerName.StartsWith("Content-", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (excludedHeaders != null && excludedHeaders.Contains(headerName))
                {
                    continue;
                }

                var headerValue = request.Headers[headerName];

                proxy.DefaultRequestHeaders.Add(headerName, headerValue);
            }
        }

        /// <summary>
        /// Adds all headers with exception to Content headers from a given response to a proxy response.
        /// </summary>
        /// <param name="response">The source of the headers.</param>
        /// <param name="proxyResponse">The destination of the headers.</param>
        /// <param name="excludedHeaders">Names of headers which should not be mapped.</param>
        public static void MapHeaders(HttpResponseMessage response, HttpResponse proxyResponse, List<string> excludedHeaders = null)
        {
            foreach (var header in response.Headers)
            {
                if (header.Key.StartsWith("Content-", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (excludedHeaders != null && excludedHeaders.Contains(header.Key))
                {
                    continue;
                }

                proxyResponse.Headers.Add(header.Key, string.Join(";", header.Value));
            }
        }

        /// <summary>
        /// Adds or updates cookies on the response of the given context.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cookies">A cookie collection to add to the response.</param>
        public static void MapCookies(IEnumerable<Cookie> cookies, HttpRequest request)
        {
            foreach (var cookie in cookies.OrderBy(c => c.TimeStamp))
            {
                var existingCookie = request.Cookies.Get(cookie.Name);
                if (existingCookie == null)
                {
                    // create
                    request.Cookies.Add(new HttpCookie(cookie.Name, cookie.Value));
                }
                else
                {
                    // update
                    existingCookie.Value = cookie.Value;

                    request.Cookies.Set(existingCookie);
                }
            }
        }

        /// <summary>
        /// Adds or updates cookies on the response of the given context.
        /// </summary>
        /// <param name="cookies">A cookie collection to add to the response.</param>
        /// <param name="response">The response.</param>
        public static void MapCookies(IEnumerable<Cookie> cookies, HttpResponse response)
        {
            foreach (var cookie in cookies.OrderBy(c => c.TimeStamp))
            {
                var existingCookie = response.Cookies.Get(cookie.Name);
                if (existingCookie == null)
                {
                    // create
                    response.Cookies.Add(new HttpCookie(cookie.Name, cookie.Value));
                }
                else
                {
                    // update
                    existingCookie.Value = cookie.Value;

                    response.Cookies.Set(existingCookie);
                }
            }
        }

        /// <summary>
        /// Adds or updates cookies on the given handler.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="handler">A handler handling the cookies.</param>
        /// <param name="configuration">The used proxies configuration..</param>
        public static void MapCookies(HttpRequest request, HttpClientHandler handler, IApiConfiguration configuration)
        {
            var defaultDomain = new Uri(configuration.ProxyUrl).Host;

            foreach (var cookieName in request.Cookies.AllKeys)
            {
                var requestCookie = request.Cookies.Get(cookieName);
                if (requestCookie == null)
                {
                    continue;
                }

                var domain = requestCookie.Domain ?? defaultDomain;
                var cookie = new Cookie(requestCookie.Name, requestCookie.Value)
                {
                    Expires = requestCookie.Expires,
                    HttpOnly = requestCookie.HttpOnly,
                    Path = requestCookie.Path,
                    Secure = requestCookie.Secure,
                    Domain = domain
                };

                handler.CookieContainer.Add(cookie);
            }
        }

        /// <summary>
        ///Gets all cookies in a CookieContainer. Only for testing.
        /// </summary>
        /// <param name="cookieJar">A cookie container</param>
        public static IEnumerable<Cookie> GetAllCookies(CookieContainer cookieJar)
        {
            var table = (Hashtable)cookieJar.GetType().InvokeMember("m_domainTable", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, cookieJar, new object[] { });
            var cookies = new List<Cookie>();
            foreach (var key in table.Keys)
            {
                try
                {
                    // Look for http cookies.
                    if (cookieJar.GetCookies(new Uri($"http://{key}/")).Count > 0)
                    {
                        foreach (Cookie cookie in cookieJar.GetCookies(new Uri($"http://{key}/")))
                        {
                            cookies.Add(cookie);
                        }
                    }

                    // Look for https cookies
                    if (cookieJar.GetCookies(new Uri($"https://{key}/")).Count > 0)
                    {
                        foreach (Cookie cookie in cookieJar.GetCookies(new Uri($"https://{key}/")))
                        {
                            cookies.Add(cookie);
                        }
                    }

                }
                catch (Exception)
                {
                    // ignore exception, not relevant
                }
            }

            return cookies;
        }
    }
}
