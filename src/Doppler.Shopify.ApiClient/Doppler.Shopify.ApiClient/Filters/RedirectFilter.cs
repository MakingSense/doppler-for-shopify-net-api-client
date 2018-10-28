using Newtonsoft.Json;

namespace Doppler.Shopify.ApiClient.Filters
{
    /// <summary>
    /// Options for filtering <see cref="RedirectService.List(RedirectFilter)"/> results.
    /// </summary>
    public class RedirectFilter : ListFilter
    {
        /// <summary>
        /// An optional parameter that filters the result to redirects with the given path.
        /// </summary>
        [JsonProperty("path")]
        public string Path { get; set; }

        /// <summary>
        /// An optional parameter that filters the result to redirects with the given target.
        /// </summary>
        [JsonProperty("target")]
        public string Target { get; set; }
    }
}
