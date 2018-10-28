using Newtonsoft.Json;

namespace Doppler.Shopify.ApiClient.Filters 
{
    public class DraftOrderFilter : ListFilter 
    {
        /// <summary>
        /// Only return orders with the given status. Known values are "open" (default), "invoice_sent", and "completed".
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}