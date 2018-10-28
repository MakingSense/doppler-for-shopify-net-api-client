using Newtonsoft.Json;

namespace Doppler.Shopify.ApiClient.Filters
{
    /// <summary>
    /// Options for filtering <see cref="MetaFieldService.Count(long?, string, MetaFieldFilter)"/> and 
    /// <see cref="MetaFieldService.List(long?, string, MetaFieldFilter)"/> results.
    /// </summary>
    public class MetaFieldFilter : ListFilter
    {
        /// <summary>
        /// Filter by namespace.
        /// </summary>
        [JsonProperty("namespace")]
        public string Namespace{ get; set; }

        /// <summary>
        /// Filter by key value.
        /// </summary>
        [JsonProperty("key")]
        public string Key{ get; set; }

        /// <summary>
        /// Filter by value_type.
        /// </summary>
        [JsonProperty("value_type")]
        public string ValueType{ get; set; }
    }
}
