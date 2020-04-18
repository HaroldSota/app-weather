using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AppWeather.Persistence.Mapping
{
    public partial class EntityMappinConfiguration<TEntity> : IMappingConfiguration, IEntityTypeConfiguration<TEntity>
        where TEntity : class, new()
    {

        #region [ IMappingConfiguration: Implemantation ]

        public void ApplyConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(this);
        }
        #endregion

        #region [ IEntityTypeConfiguration: Implemantation ]
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            var tblName = typeof(TEntity).Name;
            var customTableAttribute = typeof(TEntity).GetCustomAttributes(false);
            if (customTableAttribute != null && customTableAttribute.Length > 0)
            {
                var attribute = customTableAttribute.FirstOrDefault(a => a.GetType().Equals(typeof(TableAttribute)));

                if (attribute != null)
                    tblName = ((TableAttribute)attribute).Name;
            }

            builder.ToTable(tblName);
        }

        #endregion

    }
}
