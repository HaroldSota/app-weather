using System;
using System.Collections.Generic;
using System.Text;

namespace AppWeather.ExternalServices.Google.Model
{

    public class Error
    {
        public string status { get; set; }
        public string error_message { get; set; }
        public Prediction[] predictions { get; set; }
    }
}