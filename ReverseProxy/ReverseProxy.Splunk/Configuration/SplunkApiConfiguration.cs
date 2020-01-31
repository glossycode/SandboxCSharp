using System.Configuration;
using QmcReverseProxy.Infrastructure.Http.Configuration;

namespace QmcReverseProxy.Splunk.Configuration
{
    public class SplunkApiConfiguration : IApiConfiguration
    {
        #region public members

        public string UserName => ConfigurationManager.AppSettings["SplunkUserName"] ?? "qlpartnersplunk";

        public string Password => ConfigurationManager.AppSettings["SplunkPassword"] ?? "L3tM3!n";

        public string ProxyUrl => ConfigurationManager.AppSettings["SplunkProxyBaseUrl"] ?? "http://splunk-proxy.local.quickline.ch";

        public string BaseUrl => ConfigurationManager.AppSettings["SplunkBaseUrl"] ?? "https://splunk.qldc.ch";

        public string LoginPath => ConfigurationManager.AppSettings["SplunkLoginPath"] ?? "/en-GB/account/insecurelogin";

        public string InitialPath => ConfigurationManager.AppSettings["SplunkInitialPath"] ?? "/en-GB/app/UI-QL-vision/ott_partnerkpi";

        #endregion
    }
}