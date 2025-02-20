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

        public static Result<ClanModel> Create(ClanDTO clanDTO)
        {
            if (string.IsNullOrWhiteSpace(clanDTO.Name) || clanDTO.Name.Length > 100)
            {
                return Result.Failure<ClanModel>("Name cannot be empty and must be under 100 characters.");
            }

            if (string.IsNullOrWhiteSpace(clanDTO.Description) || clanDTO.Description.Length > 500)
            {
                return Result.Failure<ClanModel>("Description cannot be empty and must be under 500 characters.");
            }

            return Result.Success(new ClanModel(clanDTO.Name, clanDTO.Description));
        }
    }
}
