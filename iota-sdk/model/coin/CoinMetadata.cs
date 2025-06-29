using Newtonsoft.Json;

namespace iota_sdk.model.coin;

/// <summary>
/// Provides metadata information for a coin type.
/// </summary>
public class CoinMetadata
{
    /// <summary>
    /// Number of decimal places the coin uses.
    /// </summary>
    [JsonProperty("decimals")]
    public byte Decimals { get; set; }

    /// <summary>
    /// Description of the token.
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; }

    /// <summary>
    /// URL for the token logo.
    /// </summary>
    [JsonProperty("iconUrl")]
    public string? IconUrl { get; set; }

    /// <summary>
    /// Object id for the CoinMetadata object.
    /// </summary>
    [JsonProperty("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Name for the token.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Symbol for the token.
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; }
}