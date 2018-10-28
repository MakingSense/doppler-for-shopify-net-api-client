﻿using Newtonsoft.Json;

namespace Doppler.Shopify.ApiClient.Filters
{
    /// <summary>
    /// An object for filtering the results of <see cref="SmartCollectionService.Count(SmartCollectionFilter)"/> and <see cref="SmartCollectionService.List(SmartCollectionFilter)"/>.
    /// </summary>
    public class SmartCollectionFilter : PublishableListFilter
    {
        /// <summary>
        /// Filter to smart collections with the given title.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Filter by smart collection handle.
        /// </summary>
        [JsonProperty("handle")]
        public string Handle { get; set; }

        /// <summary>
        /// Filter to smart collections that includes given product.
        /// </summary>
        [JsonProperty("product_id")]
        public long? ProductId { get; set; }
    }
}
