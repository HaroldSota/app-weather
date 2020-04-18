using Microsoft.Extensions.Configuration;

namespace AppWeather.Core.Configuration
{
    public interface IAppWeatherConfig
    {
        bool IsTesting { get; }
        string UserSearchCookie { get; }
        string DataConnectionString { get; }
        public IConfigurationSection Bindings { get; }
    }
}
