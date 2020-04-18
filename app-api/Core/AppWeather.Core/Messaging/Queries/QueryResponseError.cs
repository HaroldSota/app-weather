using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AppWeather.Core.Messaging.Queries
{
    public sealed class QueryResponseError
    {
        #region [ Ctor ]

        
        public QueryResponseError()
        {
            //Only for testing purpose
        }
       

        public QueryResponseError(string errorType, string message)
        {
            ErrorType = errorType;
            Message = message;
        }

        public QueryResponseError(Exception exception)
        {
            ErrorType = exception.GetType().Name;
            Message = exception.Message;
            Obj = exception;
        }

        #endregion

        #region  [ Properties ]

        [JsonPropertyName("ErrorType")]
        public string ErrorType { get; set; }

        [JsonPropertyName("Message")]
        public string Message { get; set; }

        [JsonPropertyName("Obj")]
        public Exception Obj { get; set; }

        #endregion
    }
}
