using ClanChat.Data.Entities;

namespace ClanChat.Abstractions.User
{
    public interface ITokenService
    {
        string GenerateJwtToken(UserEntity user);
    }
}
