using AppWeather.Core.Integration;
using AppWeather.ExternalServices.OpenWeatherMap.Model;
using System.Linq;
using System.Threading.Tasks;

namespace AppWeather.ExternalServices.OpenWeatherMap
{
    public sealed class OpenWeatherMapApiService : RestClient, IOpenWeatherMapApiService
    {
        #region [ Ctor ]
        public OpenWeatherMapApiService(IOpenWeatherMapConfiguration bindingConfiguration) 
            : base(bindingConfiguration)
        {
        }

        #endregion

        #region [ Methods ]

        public async Task<ApiResponse<Forecast, Error>> GetForecastByCityName(string cityName)
        {
            var resourceConfiguration = _bindingConfiguration.Resources.FirstOrDefault(r => r.Name.Equals("GetForecastByCityName"));

            return
                await ConfigurateClient(resourceConfiguration)
                           .GetAsync<Forecast, Error>(string.Format($"{this._bindingConfiguration.Endpoint}{resourceConfiguration.Location}&{_bindingConfiguration.ApiKeyName}={_bindingConfiguration.ApiKeyValue}", cityName));
        }



        public async Task<ApiResponse<Forecast, Error>> GetForecastByZipCode(string zipCode)
        {
            var resourceConfiguration = _bindingConfiguration.Resources.FirstOrDefault(r => r.Name.Equals("GetForecastByZipCode"));

            return
                await ConfigurateClient(resourceConfiguration)
                           .GetAsync<Forecast, Error>(string.Format($"{this._bindingConfiguration.Endpoint}{resourceConfiguration.Location}&{_bindingConfiguration.ApiKeyName}={_bindingConfiguration.ApiKeyValue}", zipCode));
        }

        public async Task<ApiResponse<Forecast, Error>> GetForecastByCoordinate(float lat, float lon)
        {
            var resourceConfiguration = _bindingConfiguration.Resources.FirstOrDefault(r => r.Name.Equals("GetForecastByCoordinate"));

            return
                await ConfigurateClient(resourceConfiguration)
                           .GetAsync<Forecast, Error>(string.Format($"{this._bindingConfiguration.Endpoint}{resourceConfiguration.Location}&{_bindingConfiguration.ApiKeyName}={_bindingConfiguration.ApiKeyValue}", lat, lon));
        }

        #endregion
    }
}