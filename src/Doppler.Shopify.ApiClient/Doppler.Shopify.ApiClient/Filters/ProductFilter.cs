﻿using Newtonsoft.Json;

namespace Doppler.Shopify.ApiClient.Filters
{
    /// <summary>
    /// Options for filtering <see cref="ProductService.Count(ProductFilter)"/> and 
    /// <see cref="ProductService.List(ProductFilter)"/> results.
    /// </summary>
    public class ProductFilter : PublishableListFilter
    {
        /// <summary>
        /// Filter by product title.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Filter by product vendor.
        /// </summary>
        [JsonProperty("vendor")]
        public string Vendor { get; set; }

        /// <summary>
        /// Filter by product handle.
        /// </summary>
        [JsonProperty("handle")]
        public string Handle { get; set; }

        /// <summary>
        /// Filter by product type.
        /// </summary>
        [JsonProperty("product_type")]
        public string ProductType { get; set; }

        /// <summary>
        /// Filter by collection id.
        /// </summary>
        [JsonProperty("collection_id")]
        public long? CollectionId { get; set; }
    }
}
