using System;
using System.Collections.Generic;
using System.Text;

namespace AppWeather.Core.Domain.BaseModel
{
    /// <summary>
    ///     Base class for entities
    /// </summary>
    public abstract class BaseEntity : IEntity
    {
        #region [ Ctor ]

        protected BaseEntity()
                 : this(Guid.NewGuid())
        {
        }

        protected BaseEntity(Guid id)
        {
            Id = id;
        }
        #endregion

        #region [ Properties ]

        /// <summary>
        ///     Gets or sets the entity identifier
        /// </summary>
        public Guid Id { get; private set; }

        #endregion
    }
}