using AppWeather.Api.Messaging.Handlers.Places;
using AppWeather.Core.Configuration;
using AppWeather.Core.Infrastructure;
using AppWeather.Core.Infrastructure.TypeFinder;
using AppWeather.Core.Messaging;
using AppWeather.Core.Messaging.Queries;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace AppWeather.Api.Messaging
{
    public class AppWeatherMessagingStartup : IAppWeatherStartup
    {
        #region [ IAppWeatherStartup: Implementation ]
        public int Order => 30;

        public void ConfigureServices(IServiceCollection services)
        {

        }

        public void RegisterDependencies(ContainerBuilder builder, ITypeFinder typeFinder, IAppWeatherConfig appWeatherConfig)
        {
            #region [ Register Messaging Bus ]

            builder.RegisterType<Bus>().As<IBus>().SingleInstance();

            #endregion

            #region [ Register Query Handlers ]

            builder
                    .RegisterAssemblyTypes(typeof(GetSuggestionHandler).Assembly)
                    .As(
                        type => type.GetInterfaces()
                            .Where(interfaceType => interfaceType.IsClosedTypeOf(typeof(IQueryHandler<,>)))
                            .Select(interfaceType => interfaceType)
                    )
                    .InstancePerLifetimeScope();

            #endregion
        }

        public void Configure(IApplicationBuilder application)
        {
            
        }

        #endregion
    }
}