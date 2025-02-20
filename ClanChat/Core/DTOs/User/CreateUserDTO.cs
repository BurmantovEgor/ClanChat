namespace ClanChat.Core.DTOs.User
{
    public class CreateUserDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Guid ClanId { get; set; }
    }
}
