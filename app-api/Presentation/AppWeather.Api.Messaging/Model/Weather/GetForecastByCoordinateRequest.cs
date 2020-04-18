using AppWeather.Core.Messaging.Queries;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AppWeather.Api.Messaging.Model.Weather
{
    public sealed class GetForecastByCoordinateRequest : IQuery<GetForecastByCioordinateResponse>
    {
        [JsonPropertyName("UserId")]
        public string UserId { get; set; }

        [JsonPropertyName("Coordinate")]
        public string Coordinate { get; set; }
    }

    public sealed class GetForecastByCioordinateResponse
    {
        [JsonPropertyName("Forecasts")]
        public List<DayForecast> Forecasts { get; set; } = new List<DayForecast>();

        [JsonPropertyName("Locality")]
        public Locality Locality { get; set; }
    }
}
