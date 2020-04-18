using System;
using System.Collections.Generic;
using System.Text;

namespace AppWeather.ExternalServices.OpenWeatherMap.Model
{
    public class Error
    {
        public string cod { get; set; }
        public string message { get; set; }
    }
}
