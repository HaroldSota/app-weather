using AppWeather.Core.Configuration.Bindings;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppWeather.ExternalServices.OpenWeatherMap
{
    public sealed class OpenWeatherMapApiConfiguration : BindingConfiguration, IOpenWeatherMapConfiguration
    {
        public OpenWeatherMapApiConfiguration(IConfiguration configuration) 
            : base(configuration, "OpenWeatherMap")
        {
        }
    }
}
