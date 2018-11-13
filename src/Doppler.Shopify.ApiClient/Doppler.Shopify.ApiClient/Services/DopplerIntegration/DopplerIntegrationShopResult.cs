using Newtonsoft.Json;
using System;

namespace Doppler.Shopify.ApiClient
{
    public class DopplerIntegrationShopResult
    {
        [JsonProperty("shop")]
        public string Shop { get; set; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("connectedOn")]
        public DateTime? ConnectedOn { get; set; }
    }
}
