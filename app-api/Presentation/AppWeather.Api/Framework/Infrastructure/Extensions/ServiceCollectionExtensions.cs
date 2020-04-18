using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using AppWeather.Core.Configuration;
using AppWeather.Core.Infrastructure;
using AppWeather.Core;
using Microsoft.AspNetCore.Http;
using AppWeather.ExternalServices.Google;

namespace AppWeather.Api.Framework.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IEngine ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {


            Singleton<IAppWeatherFileProvider>.Instance = new AppWeatherFileProvider(AppContext.BaseDirectory);

            //create engine and configure service provider
            var engine = EngineContext.Create();

           engine.ConfigureServices(services, configuration);
            return engine;
        }

        public static void AddAppHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        #region [ Swashbuckle ]

        /// <summary>
        ///     Configure Swagger
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddAppSwagger(this IServiceCollection services)
        {

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AppWeather API", Version = "v1" });
            });
        }

        #endregion
    }
}