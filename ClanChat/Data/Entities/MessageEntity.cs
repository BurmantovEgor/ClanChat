﻿namespace ClanChat.Data.Entities
{
    public class MessageEntity
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ClanId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedTime { get; set; }


        public UserEntity Sender { get; set; }

    }
}
