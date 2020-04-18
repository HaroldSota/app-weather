using System;
using System.Collections.Generic;
using System.Text;

namespace AppWeather.Core.Messaging
{
    public enum MessageType
    {
        OK = 1,
        Validation = 2,
        NotFound = 3,
        Error = 4
    }
}