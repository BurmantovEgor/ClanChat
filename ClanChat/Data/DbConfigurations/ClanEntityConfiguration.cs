using ClanChat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClanChat.Data.DbConfigurations
{
    public class ClanEntityConfiguration : IEntityTypeConfiguration<ClanEntity>
    {
        public void Configure(EntityTypeBuilder<ClanEntity> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(120);
            builder.Property(m => m.CreatedTime).HasDefaultValueSql("CURRENT_TIMESTAMP");

        }
    }
}
