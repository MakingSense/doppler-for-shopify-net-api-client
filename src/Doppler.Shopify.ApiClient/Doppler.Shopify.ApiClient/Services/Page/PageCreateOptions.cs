using Newtonsoft.Json;

namespace Doppler.Shopify.ApiClient
{
    public class PageCreateOptions : Parameterizable
    {
        [JsonProperty("published")]
        public bool? Published { get; set; }
    }
}
