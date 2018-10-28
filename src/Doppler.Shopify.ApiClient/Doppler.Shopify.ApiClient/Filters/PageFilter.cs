using Newtonsoft.Json;

namespace Doppler.Shopify.ApiClient.Filters
{
    /// <summary>
    /// Options for filtering <see cref="PageService.Count(PageFilter)"/> and 
    /// <see cref="PageService.List(PageFilter)"/> results.
    /// </summary>
    public class PageFilter : PublishableListFilter
    {
        /// <summary>
        /// Filter by page title.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Filter by page handle.
        /// </summary>
        [JsonProperty("handle")]
        public string Handle { get; set; }
    }
}
