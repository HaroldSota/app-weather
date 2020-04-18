using System;
using System.Collections.Generic;
using System.Text;

namespace AppWeather.ExternalServices.OpenWeatherMap.Model
{
    public class List
    {
        public int dt { get; set; }
        public Main main { get; set; }
        public Wind wind { get; set; }
        public string dt_txt { get; set; }
    }
}
