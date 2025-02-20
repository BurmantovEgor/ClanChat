using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;

namespace ClanChat.Core.DTOs
{
    public class ClanDTO
    {
        public string Name { get;  set; }
        public string Description { get;  set; }
    }
}
