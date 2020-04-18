using AppWeather.Core.Messaging.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppWeather.Core.Messaging.Queries
{
    public interface IQueryHandler<in TQuery, TQueryResult>
     where TQuery : IQuery<TQueryResult>
    {
        List<string> Errors { get; set; }

        //QueryResponse<TQueryResult> Handle(TQuery queryObject);

        Task<QueryResponse<TQueryResult>> HandleAsync(TQuery queryObject);

        bool Validate(TQuery query);
    }
}
