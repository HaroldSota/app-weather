using System;
using System.Collections.Generic;
using System.Text;

namespace AppWeather.ExternalServices.OpenWeatherMap.Model
{
    public class City
    {
        public int id { get; set; }
        public string name { get; set; }

        public Coordinates coord { get; set; }
    }
}
