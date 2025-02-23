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
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.HasIndex(c => c.Name).IsUnique();
            builder.Property(c => c.Description).IsRequired().HasMaxLength(500);
            builder.Property(m => m.CreatedTime).HasDefaultValueSql("CURRENT_TIMESTAMP");

        }
    }
}
