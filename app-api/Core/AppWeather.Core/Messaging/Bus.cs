using AppWeather.Core.Messaging.Queries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppWeather.Core.Messaging
{
    public class Bus : IBus
    {
        #region [ Fields ]

        private readonly IHandlerResolver _queryHandlerResolver = new QueryHandlerResolver();

        #endregion

        #region [ Ctor ]

        public Bus()
        {
            
        }

        #endregion

        #region [ Methods ]

        public async Task<QueryResponse<TQueryResult>> QueryAsync<TQueryResult>(IQuery<TQueryResult> query)
        {
            try
            {
                var handlerType = _queryHandlerResolver.Get(query.GetType());

                dynamic handler = EngineContext.Current.Resolve(handlerType.GetInterfaces()[0]);

                handler.Errors = new List<string>();

                if (handler.Validate((dynamic)query))
                {
                    return await handler.HandleAsync((dynamic)query);
                }

                string error = handler.Errors.Count > 0 ? handler.Errors[0] : string.Empty;
                return new QueryResponse<TQueryResult>(MessageType.Validation ,new QueryResponseError("Validate", error));
                   
            }
            catch (Exception ex)
            {
                return new QueryResponse<TQueryResult>(MessageType.Error, ex);
            }
        }

        #endregion
    }
}