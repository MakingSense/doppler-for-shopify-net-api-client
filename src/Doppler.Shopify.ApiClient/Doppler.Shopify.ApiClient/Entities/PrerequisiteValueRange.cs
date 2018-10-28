using Newtonsoft.Json;

namespace Doppler.Shopify.ApiClient
{
    public class PrerequisiteValueRange
    {
        [JsonProperty("less_than_or_equal_to")]
        public decimal? LessThanOrEqualTo { get; set; }

        [JsonProperty("greater_than_or_equal_to")]
        public decimal? GreaterThanOrEqualTo { get; set; }
    }
}