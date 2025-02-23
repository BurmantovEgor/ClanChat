using ClanChat.Core.DTOs.Clan;
using CSharpFunctionalExtensions;

namespace ClanChat.Core.Models
{
    public class ClanModel
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private ClanModel(string name, string description)
        {
            Name = name;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }

        public static Result<ClanModel> Create(CreateClanDTO clanDTO)
        {
            if (string.IsNullOrWhiteSpace(clanDTO.Name) || clanDTO.Name.Length > 100)
            {
                return Result.Failure<ClanModel>("Название не может быть пустым и длиннее 100 символов");
            }

            if (string.IsNullOrWhiteSpace(clanDTO.Description) || clanDTO.Description.Length > 500)
            {
                return Result.Failure<ClanModel>("Описание не может быть пустым и длиннее 500 символов");
            }

            return Result.Success(new ClanModel(clanDTO.Name, clanDTO.Description));
        }
    }
}
