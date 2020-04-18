using System;
using System.Collections.Generic;
using System.Text;

namespace AppWeather.ExternalServices.OpenWeatherMap.Model
{
    public class Time
    {
        public Main Temperature { get; set; }
        public Wind Wind { get; set; }
        public Humidity Humidity { get; set; }
    }
}
