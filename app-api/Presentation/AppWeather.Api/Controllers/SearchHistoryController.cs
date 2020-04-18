using AppWeather.Api.Framework.Controllers;
using AppWeather.Api.Messaging.Model.SearchHistory;
using AppWeather.Core.Messaging;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AppWeather.Api.Controllers
{
    /// <summary>
    ///     Routs the request to 'User Search' 
    /// </summary>
    public class SearchHistoryController : BaseApiController
    {
        #region [ Ctor ]
        public SearchHistoryController(IBus bus) 
            : base(bus)
        {
        }
        #endregion
        #region [ Actions ]

        /// <summary>
        ///    Get last five searches by the user
        /// </summary>
        /// <returns>Return a list of suggestions</returns>
        [HttpGet]
        public async Task<IActionResult> GetLast(int count = 5)
        {
            return ToResult(await Bus.QueryAsync(new GetLastRequest() { Count = count, UserId = UserId }));
        }

        /// <summary>
        ///    Get last five searches by the user
        /// </summary>
        /// <returns>Return a list of suggestions</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return ToResult(await Bus.QueryAsync(new GetAllRequest() { UserId = UserId }));
        }
        #endregion
    }
}
