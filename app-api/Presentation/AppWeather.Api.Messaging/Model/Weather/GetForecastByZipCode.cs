using AppWeather.Core.Messaging.Queries;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AppWeather.Api.Messaging.Model.Weather
{
    public sealed class GetForecastByZipCodeReqsuest : IQuery<GetForecastByZipCodeResponse>
    {
        [JsonPropertyName("UserId")]
        public string UserId { get; set; }

        [JsonPropertyName("ZipCode")]
        public string ZipCode { get; set; }
    }
    public sealed class GetForecastByZipCodeResponse
    {
        [JsonPropertyName("Forecasts")]
        public List<DayForecast> Forecasts { get; set; } = new List<DayForecast>();

        [JsonPropertyName("Locality")]
        public Locality Locality { get; set; }
    }
}