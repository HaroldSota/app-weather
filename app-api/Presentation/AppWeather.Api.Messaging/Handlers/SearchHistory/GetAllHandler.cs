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
    public sealed class GetAllHandler : IQueryHandler<GetAllRequest, GetAllResponse[]>
    {
        #region [ Fields ]

        private readonly IDbContext _dbContext;

        #endregion

        #region [ Ctor ]

        public GetAllHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region [ IQueryHandler : Implementation ]

        public List<string> Errors { get; set; } = new List<string>();

        public async Task<QueryResponse<GetAllResponse[]>> HandleAsync(GetAllRequest queryObject)
        {
            try
            {
                return new QueryResponse<GetAllResponse[]>(_dbContext.Set<UserSearchData>()
                 .OrderByDescending(item => item.SearchTime)
                 .Select(item => new GetAllResponse()
                 {
                     CityName = item.CityName,
                     Temperature = item.Temperature,
                     Humidity = item.Humidity
                 }).ToArray());
            }
            catch (Exception ex)
            {
                return new QueryResponse<GetAllResponse[]>(MessageType.Error, ex);
            }
        }

        public bool Validate(GetAllRequest query)
        {
            if (string.IsNullOrEmpty(query.UserId))
                Errors.Add("User Id can not be empty");

            return Errors.Count == 0;
        }

        #endregion

    }
}