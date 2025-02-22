namespace ClanChat.Core.DTOs.User
{
    public class AuthUserDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string UserToken { get; set; }
        public Guid ClanId { get; set; }
    }
}
