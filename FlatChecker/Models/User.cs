#nullable disable
using FlatChecker.Models.Enums;

namespace FlatChecker.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }
        
        public Role Role { get; set; }
        
        public string PasswordHash { get; set; }
        
        public DateTime UpdatedDate { get; set; }
    }
}
