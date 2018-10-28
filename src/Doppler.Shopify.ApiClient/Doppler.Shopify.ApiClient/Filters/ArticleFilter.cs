using Newtonsoft.Json;

namespace Doppler.Shopify.ApiClient.Filters
{
    /// <summary>
    /// Options for filtering the results of <see cref="ArticleService.List(long, ArticleFilter)"/>.
    /// </summary>
    public class ArticleFilter : PublishableListFilter
    {
        /// <summary>
        /// Filter the results to this article handle.
        /// </summary>
        [JsonProperty("handle")]
        public string Handle { get; set; }
    }
}
