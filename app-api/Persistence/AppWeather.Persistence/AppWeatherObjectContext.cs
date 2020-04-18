using AppWeather.Core;
using AppWeather.Core.Infrastructure.TypeFinder;
using AppWeather.Persistence.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AppWeather.Persistence
{
    public class AppWeatherObjectContext : DbContext, IDbContext
    {
        #region [ Ctor ]

        public AppWeatherObjectContext(DbContextOptions<AppWeatherObjectContext> options)
            : base(options)
        {
        }

        #endregion

        #region [ IDbContext: Implementation ]

        public new virtual DbSet<TEntity> Set<TEntity>() where TEntity : class, new()
        {
            return base.Set<TEntity>();
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //https://stackoverflow.com/questions/23662755/how-to-add-new-table-to-existing-database-code-first
            var typeConfigurations = Assembly.GetExecutingAssembly()
                                    .GetTypes()
                                    .Where(type => (type.BaseType?.IsGenericType ?? false)
                                                && type.BaseType.GetGenericTypeDefinition() == typeof(EntityMappinConfiguration<>)
                                            );

            foreach (var typeConfiguration in typeConfigurations)
            {
                var configuration = (IMappingConfiguration)Activator.CreateInstance(typeConfiguration);
                configuration.ApplyConfiguration(modelBuilder);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
