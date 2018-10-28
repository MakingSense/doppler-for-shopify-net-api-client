using Newtonsoft.Json;
using System;

namespace Doppler.Shopify.ApiClient
{
    /// <summary>
    /// An object representing the image for a <see cref="SmartCollection"/>.
    /// </summary>
    public class SmartCollectionImage
    {
        /// <summary>
        /// The date the image was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// The image's source URL.
        /// </summary>
        [JsonProperty("src")]
        public string Src { get; set; }

        /// <summary>
        /// The image's base64 attachment, used when creating an image.
        /// </summary>
        [JsonProperty("attachment")]
        public string Attachment { get; set; }
    }
}
