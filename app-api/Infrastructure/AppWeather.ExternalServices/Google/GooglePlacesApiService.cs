using AppWeather.Core.Integration;
using AppWeather.ExternalServices.Google.Model;
using System.Linq;
using System.Threading.Tasks;

namespace AppWeather.ExternalServices.Google
{
    public sealed class GooglePlacesApiService : RestClient, IGooglePlacesApiService
    {
        #region [ Ctor ]
        public GooglePlacesApiService(IGooglePlacesApiConfiguration bindingConfiguration)
            : base(bindingConfiguration)
        {
        }

        #endregion

        #region [ Methods ]
        public async Task<ApiResponse<Places, Error>> GetByPlacesSuggestionAsync(string place)
        {
            var resourceConfiguration = _bindingConfiguration.Resources.FirstOrDefault(r => r.Name.Equals("GetByLocation"));
            return
                await ConfigurateClient(resourceConfiguration)
                           .GetAsync<Places, Error>(string.Format($"{this._bindingConfiguration.Endpoint}{resourceConfiguration.Location}&{_bindingConfiguration.ApiKeyName}={_bindingConfiguration.ApiKeyValue}", place));
        }



        #endregion
    }
}