using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AppWeather.Core.Configuration.Bindings;

namespace AppWeather.Core.Integration
{
    /// <summary>
    ///     Client to create Http requests and process response result
    /// </summary>
    public abstract class RestClient : IRestClientConfig, IRestClient
    {
        #region [ Properties ]

        protected readonly IBindingConfiguration _bindingConfiguration;

        private HttpClient _client;

        protected HttpClient Client => _client ?? (_client = new HttpClient(handler));

        #endregion

        #region [ Initialization ]

        private HttpClientHandler handler = new HttpClientHandler();

        private int size = 1000;

        /// <inheritdoc />
        /// <summary>
        ///     Sets the api binging propertys for requests made by this client instance
        /// </summary>
        /// <param name="bindingConfiguration"></param>
        protected RestClient(IBindingConfiguration bindingConfiguration)
        {
            _bindingConfiguration = bindingConfiguration;
        }


        #endregion

        #region [ IRestClientConfig: Implementation]

        public IRestClient ConfigurateClient(IBindingResourceConfiguration resourceConfiguration)
        {
            if (string.IsNullOrEmpty(_bindingConfiguration.Endpoint))
            {
                throw new Exception("BaseAddress/Endpoint is required!");
            }

            Client.BaseAddress = new Uri(_bindingConfiguration.Endpoint);

            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Range = new RangeHeaderValue(0, size);
            Client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(resourceConfiguration.ContentType));
            Client.Timeout = resourceConfiguration.Timeout == 0 ? new TimeSpan(0, 0, 0, 10) : new TimeSpan(0, 0, 0, resourceConfiguration.Timeout);

            return this;
        }

        #endregion 

        #region [ IRestClient: Implementation]

        /// <inheritdoc />
        public async Task<ApiResponse<T, E>> GetAsync<T, E>(string uri)
        {
            return await InvokeAsync<T, E>
                (
                       client => client.GetAsync(uri),
                       async (response, deserializeType) =>
                       {

                           // get result type object
                           if (deserializeType.Equals(typeof(T)))
                           {
                               if(!typeof(T).Equals(typeof(string)))
                                    return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());

                               return await response.Content.ReadAsStringAsync();
                           }

                           // get error type object
                           return JsonConvert.DeserializeObject <E> (await response.Content.ReadAsStringAsync());
                       }
                );
        }

        #endregion

        private async Task<ApiResponse<T, E>> InvokeAsync<T, E>(
            Func<HttpClient, Task<HttpResponseMessage>> apiCaller,
            Func<HttpResponseMessage, Type, Task<object>> responseReader = null)
        {
            if (apiCaller == null)
                throw new ArgumentNullException(nameof(apiCaller));

            HttpResponseMessage httpResponse;
            var apiReponse = new ApiResponse<T, E>();

            try
            {
                httpResponse = await apiCaller(Client).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw new Exception($"Error communicating with {_bindingConfiguration.Name}", e);
                //throw new Exception($"Error connectiong to resource server!{Environment.NewLine}Error message: '{e.Message}'");
            }

            if (!httpResponse.IsSuccessStatusCode)
            {
                var exception = new Exception($"{_bindingConfiguration.Name} returned an error. StatusCode : {httpResponse.StatusCode}");
                exception.Data.Add("StatusCode", httpResponse.StatusCode);
                try
                {
                    return new ApiResponse<T, E> { Error = responseReader != null ? (E)(await responseReader(httpResponse, typeof(E)).ConfigureAwait(false)) : default(E) };
                }
                catch
                {
                    throw new Exception($"{_bindingConfiguration.Name} returned an error. StatusCode : {httpResponse.StatusCode}", exception);
                }
            }

            return new ApiResponse<T, E> { Response = responseReader != null ? (T)(await responseReader(httpResponse, typeof(T)).ConfigureAwait(false)) : default(T) };
        }


    }
}
