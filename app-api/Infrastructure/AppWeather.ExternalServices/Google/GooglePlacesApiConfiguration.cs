using AppWeather.Core.Configuration;
using AppWeather.Core.Configuration.Bindings;
using Microsoft.Extensions.Configuration;
using System;

namespace AppWeather.ExternalServices.Google
{
    public sealed class GooglePlacesApiConfiguration : BindingConfiguration, IGooglePlacesApiConfiguration
    {
        public GooglePlacesApiConfiguration(IConfiguration configuration)
        : base(configuration, "GooglePlacesBinding")
        {
        }
    }
}