﻿using AppWeather.Api.Messaging.Model.Weather;
using AppWeather.Core.Domain.UserSearchModel;
using AppWeather.Core.Integration;
using AppWeather.Core.Messaging;
using AppWeather.Core.Messaging.Queries;
using AppWeather.ExternalServices.OpenWeatherMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppWeather.Api.Messaging.Handlers.Weather
{
    public sealed class GetForecastByZipCodeHandler : IQueryHandler<GetForecastByZipCodeReqsuest, GetForecastByZipCodeResponse>
    {
        #region [ Fields ]

        private IOpenWeatherMapApiService _openWeatherMapApiService;
        private IUserSearchRepository _userSearchRepository;

        #endregion

        #region [ Ctor ]

        public GetForecastByZipCodeHandler(IOpenWeatherMapApiService openWeatherMapApiService, IUserSearchRepository userSearchRepository)
        {
            this._openWeatherMapApiService = openWeatherMapApiService;
            this._userSearchRepository = userSearchRepository;
        }

        #endregion

        #region [ IQueryHandler : Implementation ]

        public List<string> Errors { get; set; } = new List<string>();

        public async Task<QueryResponse<GetForecastByZipCodeResponse>> HandleAsync(GetForecastByZipCodeReqsuest query)
        {
            try
            {
                var apiResult = await _openWeatherMapApiService.GetForecastByZipCode(query.ZipCode);

                if (apiResult.IsSucessful)
                {
                    var current = apiResult.Response.list.ToList().OrderBy(item => item.dt_txt).First();
                    _userSearchRepository.Add(new UserSearch(query.UserId, apiResult.Response.city.name, current.main.temp, current.main.humidity, DateTime.Now));

                    return new QueryResponse<GetForecastByZipCodeResponse>(
                        new GetForecastByZipCodeResponse
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
                    return new QueryResponse<GetForecastByZipCodeResponse>(msgType, new QueryResponseError(apiResult.Error.cod, apiResult.Error.message));
                }
            }
            catch (Exception ex)
            {
                return new QueryResponse<GetForecastByZipCodeResponse>(MessageType.Error, ex);
            }
        }

        public bool Validate(GetForecastByZipCodeReqsuest query)
        {
            if (query == null)
                this.Errors.Add("Error: Query parametter is null!");
            if (string.IsNullOrEmpty(query.ZipCode))
                this.Errors.Add("Error:Zip Code is empty!");
            else if(!Regex.IsMatch(query.ZipCode, "^(?!01000|99999)(0[1-9]\\d{3}|[1-9]\\d{4})$"))
                this.Errors.Add($"{query.ZipCode} is not a valid German zip code! Germany has 5 digits always from 01001 up to 99998.");

            return Errors.Count == 0;
        }

        #endregion
    }
}
