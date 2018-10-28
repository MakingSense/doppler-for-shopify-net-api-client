using Newtonsoft.Json;

namespace ShopifySharp.Filters
{
    /// <summary>
    /// Options for filtering <see cref="CheckoutService.Count(CheckoutFilter)"/> and 
    /// <see cref="CheckoutService.List(CheckoutFilter)"/> results.
    /// </summary>
    public class CheckoutFilter : ListFilter
    {
        /// <summary>
        /// An optional, parameter to determine which carts to retrieve.
        /// open - All open abandoned checkouts (default)
        /// closed - Show only closed abandoned checkouts
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
