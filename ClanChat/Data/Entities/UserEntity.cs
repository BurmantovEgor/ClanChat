namespace ClanChat.Data.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public Guid ClanId { get; set; }


        public ClanEntity Clan { get; set; }
    }
}
