using AppWeather.Core.Integration;
using AppWeather.ExternalServices.Google.Model;
using System.Threading.Tasks;

namespace AppWeather.ExternalServices.Google
{
    public interface IGooglePlacesApiService
    {
        Task<ApiResponse<Places, Error>> GetByPlacesSuggestionAsync(string place);
    }
}
