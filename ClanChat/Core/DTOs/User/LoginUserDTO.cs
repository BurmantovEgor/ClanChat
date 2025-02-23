using System.ComponentModel.DataAnnotations;

namespace ClanChat.Core.DTOs.User
{
    public class LoginUserDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
