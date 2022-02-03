using FlatChecker.Models.Enums;

namespace FlatChecker.Models.Dto
{
    public class SuggestionDto
    {
        public int Id { get; set; }

        public decimal MonthPrice { get; set; }

        public string Address { get; set; }

        public int Floor { get; set; }

        public District District { get; set; }

        public int OwnerId { get; set; }

        public int? SettlerId { get; set; }
    }
}
