using Newtonsoft.Json;

namespace iota_sdk.model.coin;

/// <summary>
/// Represents the total supply of a coin type.
/// </summary>
public class CoinSupply
{
    /// <summary>
    /// The total supply value.
    /// </summary>
    [JsonProperty("value")]
    public string Value { get; set; }
}