using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AppWeather.Api.Framework.Infrastructure.Extensions;
using Autofac;
using AppWeather.Core.Infrastructure;

namespace AppWeather.Api
{
    public class Startup
    {
        #region [ Fields ]
        private readonly IConfiguration _configuration;
        private IEngine _engine;

        #endregion

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _engine = services.ConfigureApplicationServices(_configuration);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            _engine.RegisterDependencies(builder, _configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           app.ConfigureRequestPipeline();
        }
    }
}
