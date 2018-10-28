using Newtonsoft.Json;
using System;

namespace Doppler.Shopify.ApiClient
{
    /// <summary>
    /// Represents the image for a <see cref="CustomCollection"/>
    /// </summary>
    public class CustomCollectionImage
    {
        /// <summary>
        /// An image attached to a shop's theme returned as Base64-encoded binary data.
        /// </summary>
        [JsonProperty("attachment")]
        public string Attachment { get; set; }

        /// <summary>
        /// Source URL that specifies the location of the image.
        /// </summary>
        [JsonProperty("src")]
        public string Src { get; set; }

        /// <summary>
        /// The date the image was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? Created_At { get; set; }
    }
}
