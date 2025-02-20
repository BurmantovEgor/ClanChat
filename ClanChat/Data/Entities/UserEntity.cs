using Microsoft.AspNetCore.Identity;

namespace ClanChat.Data.Entities
{
    public class UserEntity : IdentityUser<Guid>
    {
        public Guid ClanId { get; set; }
        public ClanEntity Clan { get; set; }
    }
}
