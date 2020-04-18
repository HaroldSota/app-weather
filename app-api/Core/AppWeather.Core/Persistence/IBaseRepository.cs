using AppWeather.Core.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppWeather.Core.Persistence
{
    /// <summary>
    /// Represents an entity repository
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TData">Data type</typeparam>
    public interface IBaseRepository<in TEntity, out TData>
        where TEntity : BaseEntity
        where TData : IData, new()
    {
        /// <summary>
        /// Add the entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Add(TEntity entity);
    }
}
