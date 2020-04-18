using AppWeather.Api.Messaging.Model.Weather;
using AppWeather.Core.Domain.UserSearchModel;
using AppWeather.Core.Messaging;
using AppWeather.Core.Messaging.Queries;
using AppWeather.ExternalServices.OpenWeatherMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppWeather.Api.Messaging.Handlers.Weather
{
    public sealed class GetForecastByCityNameHandler : IQueryHandler<GetForecastByCityNameRequest, GetForecastByCityNameResponse>
    {

        #region [ Fields ]

        private IOpenWeatherMapApiService _openWeatherMapApiService;
        private IUserSearchRepository _userSearchRepository;

        #endregion

        #region [ Ctor ]

        public GetForecastByCityNameHandler(IOpenWeatherMapApiService openWeatherMapApiService, IUserSearchRepository userSearchRepository)
        {
            _openWeatherMapApiService = openWeatherMapApiService;
            _userSearchRepository = userSearchRepository;
        }

        #endregion

        #region [ IQueryHandler : Implementation ]

        public List<string> Errors { get; set; } = new List<string>();

        public async Task<QueryResponse<GetForecastByCityNameResponse>> HandleAsync(GetForecastByCityNameRequest query)
        {
            try
            {
                var apiResult = await _openWeatherMapApiService.GetForecastByCityName(query.CityName);

                if (apiResult.IsSucessful)
                {
                    var current = apiResult.Response.list.ToList().OrderBy(item => item.dt_txt).First();
                    _userSearchRepository.Add(new UserSearch(query.UserId, apiResult.Response.city.name, current.main.temp, current.main.humidity, DateTime.Now));

                    return new QueryResponse<GetForecastByCityNameResponse>(
                        new GetForecastByCityNameResponse
                        {
                            Locality = new Locality
                            {
                                CityName = apiResult.Response.city.name,
                                Lat = apiResult.Response.city.coord.lat,
                                Lon = apiResult.Response.city.coord.lon
                            },
                            Forecasts = apiResult.Response.list.ToList()
                                    .GroupBy(item => item.dt_txt.Substring(8, 2))
                                    .Select(group => new DayForecast()
                                    {
                                        Date = group.Key,
                                        Day = DateTime.Parse(group.First().dt_txt).DayOfWeek.ToString(),
                                        MinTemp = (int)group.Min(item => item.main.temp_min),
                                        MaxTemp = (int)group.Min(item => item.main.temp_max),
                                        AvgHumidity = (int)group.Average(item => item.main.humidity),
                                        AvgWindSpeed = (int)group.Average(item => item.wind.deg),
                                    }).ToList()
                        });
                }
                else
                {
                    var msgType = apiResult.Error.cod.Equals("404") ? MessageType.NotFound : MessageType.Error;
                    return new QueryResponse<GetForecastByCityNameResponse>(msgType, new QueryResponseError(apiResult.Error.cod, apiResult.Error.message));
                }
            }
            catch(Exception ex)
            {
                return new QueryResponse<GetForecastByCityNameResponse>(MessageType.Error, ex);
            }
        }

        public bool Validate(GetForecastByCityNameRequest query)
        {
            if (query == null)
                this.Errors.Add("Error: Query parametter is null!");
            else
            {
                if (string.IsNullOrEmpty(query.CityName))
                    this.Errors.Add("Error: CityName is empty!");

                if (query.CityName.Length < 3)
                    this.Errors.Add("Error: CityName must be at least 3 alphabetic letters!");

                if (Regex.IsMatch(query.CityName, "[^a-zA-ZöäüÖÄÜß :]"))
                    this.Errors.Add("The cityname can contain only german alphabetic letters and space!");
            }

            return Errors.Count == 0;
        }

        #endregion
    }
}