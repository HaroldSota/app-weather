using AppWeather.Core.Domain.BaseModel;
using AppWeather.Core.Infrastructure;
using AppWeather.Core.Persistence;
using AppWeather.Persistence.Mapping;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AppWeather.Persistence
{
    /// <inheritdoc/>
    public class BaseRepository<TEntity, TData> : IBaseRepository<TEntity, TData>
        where TEntity : BaseEntity
        where TData : class, IData, new()
    {
        private readonly IDbContext _dbContext;

        private DbSet<TData> _entities;

        public BaseRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected virtual DbSet<TData> Entities => _entities ?? (_entities = _dbContext.Set<TData>());

        public void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                var data = Singleton<IMapper>.Instance.Map<TData>(entity);
                Entities.Add(data);
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                throw new Exception(GetErroAndRollback(exception), exception);
            }
        }


        protected string GetErroAndRollback(DbUpdateException exception)
        {
            if (_dbContext is DbContext dbContext)
            {
                try
                {
                    dbContext.ChangeTracker.Entries()
                        .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                        .ToList()
                        .ForEach(entry => entry.State = EntityState.Unchanged);
                }
                catch (Exception ex)
                {
                    exception = new DbUpdateException(exception.ToString(), ex);
                }
            }

            _dbContext.SaveChanges();
            return exception.ToString();
        }
    }
}
