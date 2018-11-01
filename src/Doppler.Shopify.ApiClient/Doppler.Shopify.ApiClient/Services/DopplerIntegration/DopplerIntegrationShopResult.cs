using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doppler.Shopify.ApiClient.Services.DopplerIntegration
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
