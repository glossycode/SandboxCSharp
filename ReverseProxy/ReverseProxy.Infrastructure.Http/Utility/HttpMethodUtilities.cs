using System;
using QmcReverseProxy.Infrastructure.Http.Client;

namespace QmcReverseProxy.Infrastructure.Http.Utility
{
    public static class HttpMethodUtilities
    {
        public static HttpMethod Transform(string httpMethod)
        {
            HttpMethod result;
            Enum.TryParse(httpMethod, true, out result);

            return result;
        }
    }
}