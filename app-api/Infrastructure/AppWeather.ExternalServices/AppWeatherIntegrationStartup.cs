using AppWeather.Core.Configuration;
using AppWeather.Core.Infrastructure;
using AppWeather.Core.Infrastructure.TypeFinder;
using AppWeather.ExternalServices.Google;
using AppWeather.ExternalServices.OpenWeatherMap;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AppWeather.ExternalServices
{
    public class AppWeatherIntegrationStartup : IAppWeatherStartup
    {
        #region [ IAppWeatherStartup: Implementation ]

        public int Order => 20;

        public void Configure(IApplicationBuilder application)
        {
        }

        public void RegisterDependencies(ContainerBuilder builder, ITypeFinder typeFinder, IAppWeatherConfig appWeatherConfig)
        {

            builder
                 .RegisterType<GooglePlacesApiService>()
                 .As<IGooglePlacesApiService>()
                 .InstancePerLifetimeScope();

            builder
                .RegisterInstance(new GooglePlacesApiConfiguration(appWeatherConfig.Bindings))
                .As<IGooglePlacesApiConfiguration>()
                .SingleInstance();

            builder
                 .RegisterType<OpenWeatherMapApiService>()
                 .As<IOpenWeatherMapApiService>()
                 .InstancePerLifetimeScope();

            builder
                .RegisterInstance(new OpenWeatherMapApiConfiguration(appWeatherConfig.Bindings))
                .As<IOpenWeatherMapConfiguration>()
                .SingleInstance();
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }

        #endregion
    }
}
