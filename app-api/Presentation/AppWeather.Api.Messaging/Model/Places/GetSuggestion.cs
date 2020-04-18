using AppWeather.Core.Messaging.Queries;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AppWeather.Api.Messaging.Model.Places
{
    public sealed class GetSuggestionReqsuest: IQuery<GetSuggestionResponse>
    {
        [JsonPropertyName("CityName")]
        public string CityName { get; set; }
    }

    public sealed class GetSuggestionResponse
    {
        [JsonPropertyName("Suggestions")]
        public List<string> SuggestionList { get; set; } = new List<string>();
    }
}