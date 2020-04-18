using System;
using System.Collections.Generic;
using System.Text;

namespace AppWeather.Core.Integration
{
    public class IApiResponse<T, E>
    {
        public bool IsSucessful { get; }

        public T Response { get; }

        public E Error { get; }
    }
}
