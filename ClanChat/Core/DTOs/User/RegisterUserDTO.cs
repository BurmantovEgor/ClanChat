using System.ComponentModel.DataAnnotations;

namespace ClanChat.Core.DTOs.User
{
    public class RegisterUserDTO
    {
        [Required]
        [MaxLength(75)]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public Guid ClanId { get; set; }
    }
}
