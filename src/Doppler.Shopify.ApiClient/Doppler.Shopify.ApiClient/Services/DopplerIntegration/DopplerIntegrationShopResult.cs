using Newtonsoft.Json;
using System;

namespace Doppler.Shopify.ApiClient
{
    public class DopplerIntegrationShopResult
    {
        [JsonProperty("shopName")]
        public string ShopName { get; set; }

        [JsonProperty("shopifyAccessToken")]
        public string ShopifyAccessToken { get; set; }

        [JsonProperty("connectedOn")]
        public DateTime? ConnectedOn { get; set; }

        [JsonProperty("syncProcessDopplerImportSubscribersTaskId")]
        public string SyncProcessDopplerImportSubscribersTaskId { get; set; }

        [JsonProperty("dopplerListId")]
        public long DopplerListId { get; set; }

        [JsonProperty("syncProcessInProgress")]
        public bool SyncProcessInProgress { get; set; }

        [JsonProperty("importedCustomersCount")]
        public int? ImportedCustomersCount { get; set; }

        [JsonProperty("syncProcessLastRunDate")]
        public DateTime? SyncProcessLastRunDate { get; set; }
    }
}
