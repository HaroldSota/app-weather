using AppWeather.Api.Framework.Controllers;
using AppWeather.Api.Messaging.Model.Places;
using AppWeather.Core.Messaging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AppWeather.Api.Controllers
{
    /// <summary>
    ///     Routs the request to 'Google Place Autocomplete' 
    /// </summary>
    public class PlacesController : BaseApiController
    {
        #region [ Ctor ]

        public PlacesController(IBus bus) 
            : base(bus)
        {
        }

        #endregion

        #region [ Actions ]

        /// <summary>
        ///    Get Suggestion  
        /// </summary>
        /// <param name="cityName">Provide the seatch text</param>
        /// <returns>Return a list of suggestions</returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]string cityName)
        {
            return ToResult(await Bus.QueryAsync(new GetSuggestionReqsuest { CityName = cityName }));
        }

        #endregion

    }
}
