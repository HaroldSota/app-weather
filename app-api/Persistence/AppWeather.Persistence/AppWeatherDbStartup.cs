using AppWeather.Core.Configuration;
using AppWeather.Core.Infrastructure;
using AppWeather.Core.Infrastructure.TypeFinder;
using AppWeather.Core.Persistence;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AppWeather.Persistence
{
    public class AppWeatherDbStartup : IAppWeatherStartup
    {
        #region [ IAppWeatherStartup: Implementation ] 

        public int Order => 10;

        public void Configure(IApplicationBuilder application)
        {
        }

        public void RegisterDependencies(ContainerBuilder builder, ITypeFinder typeFinder, IAppWeatherConfig appWeatherConfig)
        {
            builder
                .Register(context => new AppWeatherObjectContext(context.Resolve<DbContextOptions<AppWeatherObjectContext>>()))
                .As<IDbContext>()
                .InstancePerLifetimeScope();

            builder
                 .RegisterGeneric(typeof(BaseRepository<,>))
                 .As(typeof(IBaseRepository<,>));

            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsClosedTypesOf(typeof(IRepository<>));
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<AppWeatherObjectContext>(optionsBuilder =>
            {
                IAppWeatherConfig appConfig = Singleton<IAppWeatherConfig>.Instance;

                if (appConfig.IsTesting)
                    optionsBuilder.UseLazyLoadingProxies()
                        .UseInMemoryDatabase("AppWeatherTestDb");
                else
                    // production Db
                    optionsBuilder.UseLazyLoadingProxies()
                        .UseSqlServer(appConfig.DataConnectionString,
                            options => { options.MigrationsAssembly("AppWeather.Persistence.Migrations"); });
            });
        }

        #endregion
    }
}