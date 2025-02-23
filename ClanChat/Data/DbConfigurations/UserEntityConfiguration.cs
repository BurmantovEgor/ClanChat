using ClanChat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClanChat.Data.DbConfigurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.UserName).IsRequired().HasMaxLength(75);
            builder.HasIndex(u => u.UserName).IsUnique();
            builder.HasOne(u => u.Clan)
                .WithMany() 
                .HasForeignKey(u => u.ClanId)
                .OnDelete(DeleteBehavior.Cascade); 




        }
    }
}
