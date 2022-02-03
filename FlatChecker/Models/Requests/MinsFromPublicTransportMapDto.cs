using FlatChecker.Models.Enums;

namespace FlatChecker.Models.Requests
{
    public class MinsFromPublicTransportMapDto
    {
        public PublicTransportType PublicTransportType { get; set; }

        public MinsFromPublicTransport Mins { get; set; }
    }
}
