using System;
using System.Threading.Tasks;
using AppWeather.Api.Framework.Controllers;
using AppWeather.Api.Messaging.Model.Weather;
using AppWeather.Core.Messaging;
using AppWeather.Core.Messaging.Queries;
using Microsoft.AspNetCore.Mvc;

namespace AppWeather.Api.Controllers
{
    /// <summary>
    ///     Wather forcast api
    /// </summary>
    public class WeatherController : BaseApiController
    {
        #region [ Ctor ]
        public WeatherController(IBus bus)
            : base(bus)
        {
        }

        #endregion

        #region [ Actions ]

        [HttpGet]
        public async Task<IActionResult> Forecast()
        {
            
            if (Request.Query.ContainsKey("cityName"))
            {
                return ToResult(await Bus.QueryAsync(new GetForecastByCityNameRequest { CityName = Request.Query["cityName"], UserId = UserId }));
 
            }
            else if (Request.Query.ContainsKey("zipCode"))
            {
                return ToResult(await Bus.QueryAsync(new GetForecastByZipCodeReqsuest { ZipCode = Request.Query["zipCode"], UserId = UserId }));
            }
            else if (Request.Query.ContainsKey("coord"))
            {
                return ToResult(await Bus.QueryAsync(new GetForecastByCoordinateRequest { Coordinate = Request.Query["coord"], UserId = UserId }));
            }
            else
            {
                 return NotFound(new { error = "There is no implemented API for the given request parameters!" });
            }
        }

        #endregion
    }
}