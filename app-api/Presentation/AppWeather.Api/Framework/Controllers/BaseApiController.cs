using AppWeather.Core;
using AppWeather.Core.Configuration;
using AppWeather.Core.Messaging;
using AppWeather.Core.Messaging.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;

namespace AppWeather.Api.Framework.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BaseApiController : ControllerBase
    {
        #region [ Fields ]
        protected readonly IBus Bus;
        private static string _userSearchCookieKeyName;
        private string UserSearchCookieKeyName => _userSearchCookieKeyName;

        #endregion

        #region [ Ctor ]

        /// <summary>
        ///     BaseApiController ctor.
        /// </summary>
        /// <param name="bus">Cmd. transportation buss</param>
        public BaseApiController(IBus bus)
        {
            Bus = bus;

            if (string.IsNullOrEmpty(UserSearchCookieKeyName))
            {
                _userSearchCookieKeyName = EngineContext.Current.Resolve<IAppWeatherConfig>().UserSearchCookie;
            }
        }

        #endregion

        #region [ Properties ]

        protected string UserId
        {
            get
            {
                if (Request.Cookies.ContainsKey(_userSearchCookieKeyName))
                    return Request.Cookies[_userSearchCookieKeyName];

               var config =  EngineContext.Current.Resolve<IAppWeatherConfig>();

                if (!config.IsTesting)
                {
                    var userId = Guid.NewGuid().ToString();

                    Response.Cookies.Append(_userSearchCookieKeyName, userId);

                    return userId;
                }

                return "TestUserId";
            }

        }

        #endregion


        #region [ Methods ]

        /// <summary>
        ///     Formats the response status to be sent to th client
        /// </summary>
        /// <typeparam name="TQueryResult">The result object type</typeparam>
        /// <param name="response">the wrapper of the result object</param>
        /// <returns></returns>
        protected IActionResult ToResult<TQueryResult>(QueryResponse<TQueryResult> response)
        {
            switch (response.MessageType)
            {
                case MessageType.OK: return Ok(response.Result);
                case MessageType.NotFound: return NotFound(response.Error);
                case MessageType.Validation: return BadRequest(response.Error);
                case MessageType.Error:
                default:
                    return Problem(response.Error.Message);
            }
        }
        #endregion

    }
}