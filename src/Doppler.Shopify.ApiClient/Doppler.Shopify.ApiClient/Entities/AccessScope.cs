using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Doppler.Shopify.ApiClient
{
    /// <summary>
    /// An object representing an access scope
    /// </summary>
    public class AccessScope
    {
        /// <summary>
        /// The scope's handle, such as "read_orders", "write_products", etc...
        /// </summary>
        [JsonProperty("handle")]
        public string Handle { get; set; }
    }
}
