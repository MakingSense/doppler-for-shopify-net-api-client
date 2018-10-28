using Newtonsoft.Json;

namespace Doppler.Shopify.ApiClient
{
    /// <summary>
    /// An object representing a properties for <see cref="LineItem.Properties"/>
    /// </summary>
    public class LineItemProperty
    {
        /// <summary>
        /// The name of the note attribute.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The value of the note attribute.
        /// </summary>
        [JsonProperty("value")]
        public object Value { get; set; }
    }
}
