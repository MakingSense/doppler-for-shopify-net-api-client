using Newtonsoft.Json;

namespace Doppler.Shopify.ApiClient.Filters
{
    /// <summary>
    /// Options for filtering <see cref="CollectService.List(CollectFilter)"/> results.
    /// </summary>
    public class CollectFilter : ListFilter
    {
        /// <summary>
        /// An optional product id to retrieve. 
        /// </summary>
        [JsonProperty("product_id")]
        public long? ProductId { get; set; }

        /// <summary>
        /// An optional collection id to retrieve. 
        /// </summary>
        [JsonProperty("collection_id")]
        public long? CollectionId { get; set; }
    }
}
