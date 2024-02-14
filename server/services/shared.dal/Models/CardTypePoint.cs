using Newtonsoft.Json;
using shared.dal.Models.Types;

namespace shared.dal.Models
{
    public class CardTypePoint
    {
        [JsonProperty(PropertyName = "c")]
        public CardType CardType { get; set; }

        [JsonProperty(PropertyName = "p")]
        public int? Point { get; set; }
    }
}
