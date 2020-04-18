using System;
using System.Collections.Generic;
using System.Text;

namespace AppWeather.ExternalServices.OpenWeatherMap.Model
{
    public partial class Humidity
    {
        public byte Value { get; set; }
        public string Unit { get; set; }
    }
}
