using AppWeather.Core.Configuration;
using AppWeather.Core.Infrastructure.TypeFinder;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AppWeather.Core.Infrastructure
{
    public interface IAppWeatherStartup
    {
        void ConfigureServices(IServiceCollection services);

        void RegisterDependencies(ContainerBuilder builder, ITypeFinder typeFinder, IAppWeatherConfig appWeatherConfig);

        void Configure(IApplicationBuilder application);
        int Order { get; }
    }
}
