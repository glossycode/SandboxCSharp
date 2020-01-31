namespace QmcReverseProxy.Infrastructure.Http.Configuration
{
    public interface IApiConfiguration
    {
        string ProxyUrl { get; }

        string BaseUrl { get; }
    }
}