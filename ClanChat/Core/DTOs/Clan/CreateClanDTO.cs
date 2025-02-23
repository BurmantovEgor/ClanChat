using System.ComponentModel.DataAnnotations;

namespace ClanChat.Core.DTOs.Clan
{

    public class CreateClanDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
    }

}
