using AppWeather.Api.Messaging.Model.Places;
using AppWeather.Core.Messaging;
using AppWeather.Core.Messaging.Queries;
using AppWeather.ExternalServices.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppWeather.Api.Messaging.Handlers.Places
{
    public class GetSuggestionHandler : IQueryHandler<GetSuggestionReqsuest, GetSuggestionResponse>
    {
        #region [ Fields ]

        private IGooglePlacesApiService googlePlacesApiService;

        #endregion

        #region [ Ctor ]

        public GetSuggestionHandler(IGooglePlacesApiService googlePlacesApiService)
        {
            this.googlePlacesApiService = googlePlacesApiService;
        }

        #endregion

        #region [ IQueryHandler: Implementation ]

        public List<string> Errors { get; set; } = new List<string>();

        public async Task<QueryResponse<GetSuggestionResponse>> HandleAsync(GetSuggestionReqsuest query)
        {
            try
            {
                if (!string.IsNullOrEmpty(query.CityName))
                {
                    var apiResult = await googlePlacesApiService.GetByPlacesSuggestionAsync(query.CityName);

                    if (apiResult.IsSucessful)
                    {
                        return new QueryResponse<GetSuggestionResponse>(new GetSuggestionResponse()
                        {
                            SuggestionList = apiResult.Response.predictions
                                                        .Select(p => p.structured_formatting.main_text)
                                                        .Distinct()
                                                        .ToList()

                        });
                    }
                }
                // Default behavoire no sugestion
                return new QueryResponse<GetSuggestionResponse>(new GetSuggestionResponse());
            }
            catch
            {
                // Default behavoire no sugestion
                return new QueryResponse<GetSuggestionResponse>(new GetSuggestionResponse());
            }
        }

        public bool Validate(GetSuggestionReqsuest query)
        {
            if (query == null)
                Errors.Add("Error: Query parametter is null!");

            return Errors.Count == 0;
        }

        #endregion
    }
}