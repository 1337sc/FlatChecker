#nullable enable
using FlatChecker.Models.Enums;

namespace FlatChecker.Models
{
    public class Suggestion
    {
        public int Id { get; set; }

        public decimal MonthPrice { get; set; }

        public string Address { get; set; }

        public int Floor { get; set; }

        public District District { get; set; }

        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public int? SettlerId { get; set; }
        public User? Settler { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
