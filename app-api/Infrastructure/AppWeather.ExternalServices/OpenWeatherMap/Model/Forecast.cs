using System;
using System.Collections.Generic;
using System.Text;

namespace AppWeather.ExternalServices.OpenWeatherMap.Model
{
    public class Forecast
    {
        public List[] list { get; set; }
        public City city { get; set; }
    }
}
