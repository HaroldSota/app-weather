using AppWeather.Api.Messaging.Model.Weather;
using AppWeather.Core.Domain.UserSearchModel;
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
    public sealed class GetForecastByCoordinateHandler : IQueryHandler<GetForecastByCoordinateRequest, GetForecastByCioordinateResponse>
    {
        #region [ Fields ]

        private IOpenWeatherMapApiService _openWeatherMapApiService;
        private IUserSearchRepository _userSearchRepository;

        #endregion

        #region [ Ctor ]

        public GetForecastByCoordinateHandler(IOpenWeatherMapApiService openWeatherMapApiService, IUserSearchRepository userSearchRepository)
        {
            _openWeatherMapApiService = openWeatherMapApiService;
            _userSearchRepository = userSearchRepository;
        }

        #endregion

        #region [ IQueryHandler : Implementation ]
        public List<string> Errors { get; set; } = new List<string>();

        public async Task<QueryResponse<GetForecastByCioordinateResponse>> HandleAsync(GetForecastByCoordinateRequest query)
        {
            try
            {
                var lat = float.Parse(query.Coordinate.Split(',')[0]);
                var lon = float.Parse(query.Coordinate.Split(',')[1]);
                var apiResult = await _openWeatherMapApiService.GetForecastByCoordinate(lat, lon);

                if (apiResult.IsSucessful)
                {
                    var current = apiResult.Response.list.ToList().OrderBy(item => item.dt_txt).First();
                    _userSearchRepository.Add(new UserSearch(query.UserId, apiResult.Response.city.name, current.main.temp, current.main.humidity, DateTime.Now));

                    return new QueryResponse<GetForecastByCioordinateResponse>(
                        new GetForecastByCioordinateResponse
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
                    return new QueryResponse<GetForecastByCioordinateResponse>(MessageType.Validation, new QueryResponseError(apiResult.Error.cod, apiResult.Error.message));
                }
            }
            catch (Exception ex)
            {
                return new QueryResponse<GetForecastByCioordinateResponse>(MessageType.Validation, ex);
            }
        }

        public bool Validate(GetForecastByCoordinateRequest query)
        {
            if (query == null)
                this.Errors.Add("Error: Query parametter is null!");
            else
            {
                if (string.IsNullOrEmpty(query.Coordinate))
                    this.Errors.Add("Error: GeoCode is null or empty!");

                
                if (!Regex.IsMatch(query.Coordinate, "^[-+]?[0-9]*\\.?[0-9]+,[-+]?[0-9]*\\.?[0-9]+$"))
                    this.Errors.Add("The given GeoCode is not in the correct fomat e.g. lat,lon!");
            }

            return Errors.Count == 0; 
        }

        #endregion
    }
}
