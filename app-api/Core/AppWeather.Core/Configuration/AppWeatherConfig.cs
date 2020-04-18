using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppWeather.Core.Configuration
{
    public class AppWeatherConfig : BaseConfigurationProvider, IAppWeatherConfig
    {

        public AppWeatherConfig(IConfiguration configuration)
            : base(configuration, "AppWeatherConfig")
        {
           
        }


        #region [ Properties]

        public bool IsTesting => base.GetConfiguration<bool>("IsTesting");

        private string _userSearchCookie;
        public string UserSearchCookie => _userSearchCookie ??= base.GetConfiguration<string>("UserSearchCookie");


        private string _dataConnectionString;
        public string DataConnectionString => _dataConnectionString ??= base.GetConfiguration<string>("DataConnectionString");


        private IConfigurationSection _bindings;
        public IConfigurationSection Bindings => _bindings??= base.GetSection("Bindings");

        #endregion
    }
}
