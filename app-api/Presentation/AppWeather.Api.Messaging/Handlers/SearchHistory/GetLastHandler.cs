using AppWeather.Api.Messaging.Model.SearchHistory;
using AppWeather.Core.Messaging;
using AppWeather.Core.Messaging.Queries;
using AppWeather.Persistence;
using AppWeather.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppWeather.Api.Messaging.Handlers.SearchHistory
{
    public sealed class GetLastHandler : IQueryHandler<GetLastRequest, GetLastResponse[]>
    {
        #region [ Fields ]

        private readonly IDbContext _dbContext;

        #endregion

        #region [ Ctor ]

        public GetLastHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region [ IQueryHandler : Implementation ]

        public List<string> Errors { get; set; } = new List<string>();

        public async Task<QueryResponse<GetLastResponse[]>> HandleAsync(GetLastRequest query)
        {
            try
            {
                return new QueryResponse<GetLastResponse[]>(_dbContext.Set<UserSearchData>()
                        .OrderByDescending(item => item.SearchTime)
                        .Take(query.Count)
                        .Select(item => new GetLastResponse()
                        {
                            CityName = item.CityName,
                            Temperature = item.Temperature,
                            Humidity = item.Humidity
                        }).ToArray());
            }
            catch (Exception ex)
            {

                return new QueryResponse<GetLastResponse[]>(MessageType.Error, ex);
            }
        }

        public bool Validate(GetLastRequest query)
        {
            if (string.IsNullOrEmpty(query.UserId))
                Errors.Add("User Id can not be empty");
            if(query.Count <= 0)
                Errors.Add("The number of returned item can not be less than 1");
            return Errors.Count == 0;
        }
        #endregion
    }
}