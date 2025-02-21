using ClanChat.Core.DTOs.Clan;

namespace ClanChat.Core.DTOs.User
{
    public class UserDTO
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }
        public ClanDTO Clan { get; set; }
    }
}
