using ClanChat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClanChat.Data.DbConfigurations
{
    public class MessageEntityConfiguration : IEntityTypeConfiguration<MessageEntity>
    {
        public void Configure(EntityTypeBuilder<MessageEntity> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Message).IsRequired().HasMaxLength(500);
            builder.Property(m => m.CreatedTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.HasOne(m => m.Sender)
                   .WithMany()
                   .HasForeignKey(m => m.SenderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<ClanEntity>() 
               .WithMany()
               .HasForeignKey(m => m.ClanId)
               .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
