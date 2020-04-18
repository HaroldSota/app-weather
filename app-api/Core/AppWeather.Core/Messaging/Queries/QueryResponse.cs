using System;

namespace AppWeather.Core.Messaging.Queries
{
    public sealed class QueryResponse<TQueryResult>
    {
        #region [ Ctor ]

        public QueryResponse(TQueryResult result)
        {
            MessageType = MessageType.OK;
            Result = result;
        }

        public QueryResponse(MessageType messageType, QueryResponseError error)
        {
            MessageType = messageType;
            Error = error;
        }

        public QueryResponse(MessageType messageType, Exception exception)
        {
            MessageType = messageType;
            Error = new QueryResponseError(exception);
        }

        #endregion

        #region [ Properties ]
        public MessageType MessageType { get; private set; }

        public TQueryResult Result { get; private set; }

        public QueryResponseError Error { get; private set; }

        #endregion
    }
}