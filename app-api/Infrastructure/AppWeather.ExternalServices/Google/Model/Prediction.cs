using System;
using System.Collections.Generic;
using System.Text;

namespace AppWeather.ExternalServices.Google.Model
{
    public class Prediction
    {
        public string description { get; set; }
        public Structured_Formatting structured_formatting { get; set; }
    }
}
