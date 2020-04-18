using AppWeather.Core;
using Microsoft.AspNetCore.Builder;

namespace AppWeather.Api.Framework.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void ConfigureRequestPipeline(this IApplicationBuilder application)
        {
            EngineContext.Current.ConfigureRequestPipeline(application);
        }

        #region [ Swashbuckle ]

        /// <summary>
        ///   Configure Swagger
        /// </summary>
        /// <param name="app"></param>
        public static void UseAppSwagger(this IApplicationBuilder app)
       {
            app.UseSwagger();
            //set swager at app root
            //REF: https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("swagger/v1/swagger.json", "AppWeather API V1");
                c.RoutePrefix = string.Empty; 
            });
        }

        #endregion
    }
}
