using System.Text.Json.Serialization;

namespace iota_sdk.model.coin
{
    /// <summary>
    /// Provides metadata information for a coin type.
    /// </summary>
    public class CoinMetadata
    {
        /// <summary>
        /// Number of decimal places the coin uses.
        /// </summary>
        [JsonPropertyName("decimals")]
        public byte Decimals { get; set; }

        /// <summary>
        /// Description of the token.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// URL for the token logo.
        /// </summary>
        [JsonPropertyName("iconUrl")]
        public string IconUrl { get; set; }

        /// <summary>
        /// Object id for the CoinMetadata object.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Name for the token.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Symbol for the token.
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
    }
}