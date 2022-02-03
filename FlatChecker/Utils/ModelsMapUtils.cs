using FlatChecker.Models;
using FlatChecker.Models.Dto;

namespace FlatChecker.Utils
{
    public class ModelsMapUtils
    {
        public static SuggestionDto Map(Suggestion suggestion)
        {
            return new SuggestionDto()
            {
                Id = suggestion.Id,
                Address = suggestion.Address,
                District = suggestion.District,
                Floor = suggestion.Floor,
                MonthPrice = suggestion.MonthPrice,
                OwnerId = suggestion.OwnerId,
                SettlerId = suggestion.SettlerId
            };
        }
    }
}
