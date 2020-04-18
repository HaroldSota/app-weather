using System;
using System.Collections.Generic;
using System.Text;

namespace AppWeather.ExternalServices.Google.Model
{
    public class Places
    {
        public string status { get; set; }
        public Prediction[] predictions { get; set; }
    }
}
