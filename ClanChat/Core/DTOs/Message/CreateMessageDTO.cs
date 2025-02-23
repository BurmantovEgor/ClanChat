using System.ComponentModel.DataAnnotations;

namespace ClanChat.Core.DTOs.Message
{
    public class CreateMessageDTO
    {
        [Required]
        [MaxLength(500)]
        public string Message { get; set; }
    }
}
