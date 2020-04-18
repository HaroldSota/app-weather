using System;
using System.Collections.Generic;
using System.Text;

namespace AppWeather.Core.Messaging
{
    public interface IHandlerResolver
    {
        Type Get(Type type);
    }
}
