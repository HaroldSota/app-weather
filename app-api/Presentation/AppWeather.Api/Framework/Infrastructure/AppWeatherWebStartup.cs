using AppWeather.Api.Framework.Infrastructure.Extensions;
using AppWeather.Core.Configuration;
using AppWeather.Core.Infrastructure;
using AppWeather.Core.Infrastructure.TypeFinder;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AppWeather.Api.Framework.Infrastructure
{
    public class AppWeatherWebStartup : IAppWeatherStartup
    {
        public int Order => 2;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddControllers();
         
            services.AddVersionedApiExplorer();


            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                        builder => builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed((host) => true)
                        .AllowCredentials());
            });



            services.AddAppHttpContextAccessor();
        }

        public void RegisterDependencies(ContainerBuilder builder, ITypeFinder typeFinder, IAppWeatherConfig appWeatherConfig)
        {
            
        }

        public void Configure(IApplicationBuilder application)
        {
            application.UseCors("CorsPolicy");
            application.UseHttpsRedirection();
            //application.UseAppSwagger();
            application.UseRouting();
            application.UseAuthorization();
            application.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });           
        }


    }
}