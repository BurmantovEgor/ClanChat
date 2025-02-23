using System.ComponentModel.DataAnnotations;

namespace ClanChat.Core.DTOs.User
{
    public class LoginUserDTO
    {
        [Required]
        [MaxLength(75)]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
