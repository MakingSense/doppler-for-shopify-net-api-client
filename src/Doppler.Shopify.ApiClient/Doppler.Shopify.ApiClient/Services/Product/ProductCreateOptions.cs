using Newtonsoft.Json;

namespace Doppler.Shopify.ApiClient
{
    public class ProductCreateOptions : Parameterizable
    {
        [JsonProperty("published")]
        public bool? Published { get; set; }
    }
}
