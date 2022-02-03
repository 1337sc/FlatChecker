#nullable disable
using FlatChecker.Models.Enums;

namespace FlatChecker.Models.Requests
{
    public class EditSuggestionRequest
    {
        public int SuggestionId { get; set; }

        public decimal MonthPrice { get; set; }

        public string Address { get; set; }

        public int Floor { get; set; }

        public District District { get; set; }
    }
}
