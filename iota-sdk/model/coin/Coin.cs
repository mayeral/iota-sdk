using System.Text.Json.Serialization;

namespace iota_sdk.model.coin;

/// <summary>
/// Represents a coin object in the IOTA network.
/// </summary>
public class Coin
{
    /// <summary>
    /// The type of the coin.
    /// </summary>
    [JsonPropertyName("coinType")]
    public string CoinType { get; set; }

    /// <summary>
    /// The unique identifier of the coin object.
    /// </summary>
    [JsonPropertyName("coinObjectId")]
    public string CoinObjectId { get; set; }

    /// <summary>
    /// The version of the coin object.
    /// </summary>
    [JsonPropertyName("version")]
    public string Version { get; set; }

    /// <summary>
    /// The digest of the coin object.
    /// </summary>
    [JsonPropertyName("digest")]
    public string Digest { get; set; }

    /// <summary>
    /// The balance of the coin.
    /// </summary>
    [JsonPropertyName("balance")]
    public string Balance { get; set; }

    /// <summary>
    /// The hash of the previous transaction.
    /// </summary>
    [JsonPropertyName("previousTransaction")]
    public string PreviousTransaction { get; set; }
}