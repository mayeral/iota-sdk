using Newtonsoft.Json;

namespace iota_sdk.model.coin;

/// <summary>
/// Provides a summary of the circulating IOTA supply.
/// </summary>
public class IotaCirculatingSupply
{
    /// <summary>
    /// Timestamp (UTC) when the circulating supply was calculated.
    /// </summary>
    [JsonProperty("atCheckpoint")]
    public ulong AtCheckpoint { get; set; }

    /// <summary>
    /// Percentage of total supply that is currently circulating (range: 0.0 to 1.0).
    /// </summary>
    [JsonProperty("circulatingSupplyPercentage")]
    public double CirculatingSupplyPercentage { get; set; }

    /// <summary>
    /// Circulating supply in NANOS at the given timestamp.
    /// </summary>
    [JsonProperty("value")]
    public ulong Value { get; set; }
}