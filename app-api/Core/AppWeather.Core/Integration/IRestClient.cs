using AppWeather.Core.Configuration.Bindings;
using System.Threading.Tasks;

namespace AppWeather.Core.Integration
{
    /// <summary>
    ///     IRestClient
    /// </summary>
    public interface IRestClient
    {
        /// <summary>
        /// Executes a GET-style request and callback asynchronously
        /// </summary>
        /// <typeparam name="T">Target deserialization type</typeparam>
        Task<ApiResponse<T, E>> GetAsync<T,E>(string uri);
    }
}