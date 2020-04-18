using AppWeather.Core.Configuration.Bindings;

namespace AppWeather.Core.Integration
{
    public interface IRestClientConfig
    {
        public IRestClient ConfigurateClient(IBindingResourceConfiguration resourceConfiguration);
    }
}
