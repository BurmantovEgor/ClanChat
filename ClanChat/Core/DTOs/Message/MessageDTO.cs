using ClanChat.Core.DTOs.User;

namespace ClanChat.Core.DTOs.Message
{
    public class MessageDTO
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public UserDTO User { get; set; }
        public DateTime CreatedTime { get; set; }

    }
}
