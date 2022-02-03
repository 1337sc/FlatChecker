#nullable disable
using FlatChecker.Models.Enums;

namespace FlatChecker.Models.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public List<SuggestionDto> OwnedSuggestions { get; set; }
    }
}
