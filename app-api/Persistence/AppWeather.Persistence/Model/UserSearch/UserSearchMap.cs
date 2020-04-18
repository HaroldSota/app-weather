using AppWeather.Persistence.Mapping;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppWeather.Persistence.Model
{
    internal class UserSearchMap: EntityMappinConfiguration<UserSearchData>
    {
        public override void Configure(EntityTypeBuilder<UserSearchData> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(t => t.Id);

            entityTypeBuilder.Property(t => t.UserId)
                .HasMaxLength(36) //Guid.NewGuid().ToString().Length
                .IsRequired();

            entityTypeBuilder.Property(t => t.CityName)
                .HasMaxLength(50)
                .IsRequired();

            entityTypeBuilder.Property(t => t.SearchTime)
                .IsRequired();

            base.Configure(entityTypeBuilder);
        }
    }
}
