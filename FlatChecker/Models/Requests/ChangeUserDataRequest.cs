#nullable disable
namespace FlatChecker.Models.Requests
{
    public class ChangeUserDataRequest
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
