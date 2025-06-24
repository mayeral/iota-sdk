using Newtonsoft.Json;

namespace iota_sdk.model.coin;

/// <summary>
/// Represents a coin object in the IOTA network.
/// </summary>
public class Coin
{
    /// <summary>
    /// The type of the coin.
    /// </summary>
    [JsonProperty("coinType")]
    public string CoinType { get; set; }

    /// <summary>
    /// The unique identifier of the coin object.
    /// </summary>
    [JsonProperty("coinObjectId")]
    public string CoinObjectId { get; set; }

    /// <summary>
    /// The version of the coin object.
    /// </summary>
    [JsonProperty("version")]
    public string Version { get; set; }

    /// <summary>
    /// The digest of the coin object.
    /// </summary>
    [JsonProperty("digest")]
    public string Digest { get; set; }

    /// <summary>
    /// The balance of the coin.
    /// </summary>
    [JsonProperty("balance")]
    public string Balance { get; set; }

    /// <summary>
    /// The hash of the previous transaction.
    /// </summary>
    [JsonProperty("previousTransaction")]
    public string PreviousTransaction { get; set; }
}