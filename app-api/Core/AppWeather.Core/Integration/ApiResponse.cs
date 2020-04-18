using System;
using System.Collections.Generic;
using System.Text;

namespace AppWeather.Core.Integration
{
    public class ApiResponse<T, E> : IApiResponse<T, E>
    {
        public bool IsSucessful => Error == null;

        public T Response { get; set; }

        public E Error { get; set; }
    }
}
