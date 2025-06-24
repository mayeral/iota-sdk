using System.Numerics;
using Newtonsoft.Json;

namespace iota_sdk.model.governance;

/// <summary>
/// Represents a timelocked stake.
/// </summary>
public class TimelockedStake
{
    /// <summary>
    /// Gets or sets the timelocked staked IOTA ID.
    /// </summary>
    [JsonProperty("timelockedStakedIotaId")]
    public string TimelockedStakedIotaId { get; set; }

    /// <summary>
    /// Gets or sets the stake request epoch.
    /// </summary>
    [JsonProperty("stakeRequestEpoch")]
    public BigInteger StakeRequestEpoch { get; set; }

    /// <summary>
    /// Gets or sets the stake active epoch.
    /// </summary>
    [JsonProperty("stakeActiveEpoch")]
    public BigInteger StakeActiveEpoch { get; set; }

    /// <summary>
    /// Gets or sets the principal amount.
    /// </summary>
    [JsonProperty("principal")]
    public ulong Principal { get; set; }

    /// <summary>
    /// Gets or sets the stake status.
    /// </summary>
    [JsonProperty("status")]
    public StakeStatus Status { get; set; }

    /// <summary>
    /// The estimated reward for the stake (if applicable)
    /// </summary>
    [JsonProperty("estimatedReward", NullValueHandling = NullValueHandling.Ignore)]
    public ulong? EstimatedReward { get; set; }

    /// <summary>
    /// Gets or sets the expiration timestamp in milliseconds.
    /// </summary>
    [JsonProperty("expirationTimestampMs")]
    public ulong ExpirationTimestampMs { get; set; }

    /// <summary>
    /// Gets or sets the optional label for the stake.
    /// </summary>
    [JsonProperty("label")]
    public string Label { get; set; }
}