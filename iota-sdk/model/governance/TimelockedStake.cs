using System.Numerics;
using System.Text.Json.Serialization;

namespace iota_sdk.model.governance;

/// <summary>
/// Represents a timelocked stake.
/// </summary>
public class TimelockedStake
{
    /// <summary>
    /// Gets or sets the timelocked staked IOTA ID.
    /// </summary>
    [JsonPropertyName("timelockedStakedIotaId")]
    public string TimelockedStakedIotaId { get; set; }

    /// <summary>
    /// Gets or sets the stake request epoch.
    /// </summary>
    [JsonPropertyName("stakeRequestEpoch")]
    public BigInteger StakeRequestEpoch { get; set; }

    /// <summary>
    /// Gets or sets the stake active epoch.
    /// </summary>
    [JsonPropertyName("stakeActiveEpoch")]
    public BigInteger StakeActiveEpoch { get; set; }

    /// <summary>
    /// Gets or sets the principal amount.
    /// </summary>
    [JsonPropertyName("principal")]
    public ulong Principal { get; set; }

    /// <summary>
    /// Gets or sets the stake status.
    /// </summary>
    [JsonPropertyName("status")]
    public StakeStatus Status { get; set; }

    /// <summary>
    /// The estimated reward for the stake (if applicable)
    /// </summary>
    [JsonPropertyName("estimatedReward")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ulong? EstimatedReward { get; set; }

    /// <summary>
    /// Gets or sets the expiration timestamp in milliseconds.
    /// </summary>
    [JsonPropertyName("expirationTimestampMs")]
    public ulong ExpirationTimestampMs { get; set; }

    /// <summary>
    /// Gets or sets the optional label for the stake.
    /// </summary>
    [JsonPropertyName("label")]
    public string Label { get; set; }
}