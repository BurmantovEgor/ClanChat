using ClanChat.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClanChat.Data.DbConfigurations
{
    public class ClanChatDbContext : DbContext
    {
        public ClanChatDbContext(DbContextOptions<ClanChatDbContext> options) : base(options) { }

        public DbSet<UserEntity> User { get; set; }
        public DbSet<MessageEntity> Message { get; set; }
        public DbSet<ClanEntity> Clan { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new MessageEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ClanEntityConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public void SeedClans()
        {
            if (!Clan.Any())
            {
                Clan.Add(new ClanEntity { Id = Guid.NewGuid(), Name = "Clan1", Description = "SeedClan #1", CreatedTime = DateTime.UtcNow });
                Clan.Add(new ClanEntity { Id = Guid.NewGuid(), Name = "Clan2", Description = "SeedClan #2",  CreatedTime = DateTime.UtcNow });
                Clan.Add(new ClanEntity { Id = Guid.NewGuid(), Name = "Clan3", Description = "SeedClan #3",  CreatedTime = DateTime.UtcNow });
                SaveChanges();
            }
        }

    }
}
