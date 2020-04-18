using AppWeather.Core.Messaging.Queries;
using System.Threading.Tasks;

namespace AppWeather.Core.Messaging
{
    public interface IBus
    {
        //QueryResponse<TQueryResult> Query<TQueryResult>(IQuery<TQueryResult> query);

        Task<QueryResponse<TQueryResult>> QueryAsync<TQueryResult>(IQuery<TQueryResult> query);
    }
}