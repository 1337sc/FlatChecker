#nullable disable
using FlatChecker.Models.Enums;

namespace FlatChecker.Models
{
    public class MinsFromPublicTransportMap
    {
        public int Id { get; set; }

        public PublicTransportType PublicTransportType { get; set; }

        public MinsFromPublicTransport Mins { get; set; }

        public int SuggestionId { get; set; }
        public Suggestion Suggestion { get; set; }
    }
}
