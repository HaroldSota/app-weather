using AppWeather.Core.Configuration;
using AppWeather.Core.Infrastructure;
using AppWeather.Core.Infrastructure.TypeFinder;
using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace AppWeather.Core
{
    /// <inheritdoc />
    public class AppWeatherEngine : IEngine
    {
        private ITypeFinder _typeFinder { get; set; }

        #region [ Properties ]


        private IServiceProvider _serviceProvider { get; set; }

        /// <summary>
        ///     Gets or sets service provider
        /// </summary>
        public virtual IServiceProvider ServiceProvider => _serviceProvider;

        #endregion

        #region [ IEngine: Implementation ]
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            _typeFinder = new AppWeatherTypeFinder();
            var appConfig = new AppWeatherConfig(configuration);

            Singleton<IAppWeatherConfig>.Instance = appConfig;

            var startupConfigurations = _typeFinder.FindClassesOfType<IAppWeatherStartup>();

            var instances = startupConfigurations.Select(startup => (IAppWeatherStartup)Activator.CreateInstance(startup))
                                                 .OrderBy(startup => startup.Order);

            foreach (var instance in instances)
            {
                instance.ConfigureServices(services);
            }

            //register mapper configurations
            AddAutoMapper(services, _typeFinder);

            //resolve assemblies here.
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        public void RegisterDependencies(ContainerBuilder containerBuilder, IConfiguration configuration)
        {
            containerBuilder.RegisterInstance(Singleton<IAppWeatherConfig>.Instance).As<IAppWeatherConfig>().SingleInstance();
            containerBuilder.RegisterInstance(this).As<IEngine>().SingleInstance();
            containerBuilder.RegisterInstance(_typeFinder).As<ITypeFinder>().SingleInstance();

            //find dependency registrars provided by other assemblies
            var dependencyRegistrars = _typeFinder.FindClassesOfType<IAppWeatherStartup>();

            //create and sort instances of dependency registrars
            var instances = dependencyRegistrars
                .Select(dependencyRegistrar => (IAppWeatherStartup)Activator.CreateInstance(dependencyRegistrar))
                .OrderBy(dependencyRegistrar => dependencyRegistrar.Order);

            //register all provided dependencies
            foreach (var dependencyRegistrar in instances)
                dependencyRegistrar.RegisterDependencies(containerBuilder, _typeFinder, Singleton<IAppWeatherConfig>.Instance);
        }

        public void ConfigureRequestPipeline(IApplicationBuilder application)
        {
            _serviceProvider = application.ApplicationServices;
            var startupConfigurations = _typeFinder.FindClassesOfType<IAppWeatherStartup>();

            //create and sort instances of startup configurations
            var instances = startupConfigurations
                .Select(startup => (IAppWeatherStartup)Activator.CreateInstance(startup))
                .OrderBy(startup => startup.Order);

            //configure request pipeline
            foreach (var instance in instances)
            {
                instance.Configure(application);
            }
        }

        public T Resolve<T>() where T : class
        {
            return (T)Resolve(typeof(T));
        }

        public object Resolve(Type type)
        {
            return GetServiceProvider().GetRequiredService(type);
        }

       

        #endregion


        /// <summary>
        ///     Get IServiceProvider
        /// </summary>
        /// <returns>IServiceProvider</returns>
        protected virtual IServiceProvider GetServiceProvider()
        {
            var accessor = ServiceProvider.GetService<IHttpContextAccessor>();
            var context = accessor.HttpContext;
            return context?.RequestServices ?? ServiceProvider;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            if (assembly != null)
                return assembly;

            //get assembly from TypeFinder
            var tf = Resolve<ITypeFinder>();
            assembly = tf.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            return assembly;
        }

        protected virtual void AddAutoMapper(IServiceCollection services, ITypeFinder typeFinder)
        {
            var mapperConfigurations = typeFinder.FindClassesOfType<IMapperProfile>();
           
            var instances = mapperConfigurations
                .Select(mapperConfiguration => (IMapperProfile)Activator.CreateInstance(mapperConfiguration))
                .OrderBy(mapperConfiguration => mapperConfiguration.Order);

            var config = new MapperConfiguration(cfg =>
            {
                foreach (var instance in instances)
                {
                    cfg.AddProfile(instance.GetType());
                }
            });

            //register
            Singleton<IMapper>.Instance = config.CreateMapper();
        }
    }
}