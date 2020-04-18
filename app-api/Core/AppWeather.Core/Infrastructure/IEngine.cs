using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AppWeather.Core.Infrastructure
{
    /// <summary>
    ///     Classes implementing this interface can serve as a portal for the various services composing the AppWeather engine.
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        ///     Add and configure services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        /// <returns>Service provider</returns>
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);

        /// <summary>
        ///     Configure HTTP request pipeline
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        void ConfigureRequestPipeline(IApplicationBuilder application);

        /// <summary>
        ///     Register dependencies
        /// </summary>
        /// <param name="containerBuilder">Container builder</param>
        void RegisterDependencies(ContainerBuilder containerBuilder, IConfiguration configuration);

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <typeparam name="T">Type of resolved service</typeparam>
        /// <returns>Resolved service</returns>
        T Resolve<T>() where T : class;

        /// <summary>
        ///     Resolve dependency
        /// </summary>
        /// <param name="type">Type of resolved service</param>
        /// <returns>Resolved service</returns>
        object Resolve(Type type);
    }
}