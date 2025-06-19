using System.Text.Json.Serialization;
using iota_sdk.apis.read;
using iota_sdk.model.read;

namespace iota_sdk.model.governance;

/// <summary>
/// Represents an individual stake
/// </summary>
public class Stake
{
    /// <summary>
    /// The ID of the staked IOTA
    /// </summary>
    [JsonPropertyName("stakedIotaId")]
    public ObjectId StakedIotaId { get; set; }

    /// <summary>
    /// The epoch when the stake request was made
    /// </summary>
    [JsonPropertyName("stakeRequestEpoch")]
    public string StakeRequestEpoch { get; set; }

    /// <summary>
    /// The epoch when the stake becomes active
    /// </summary>
    [JsonPropertyName("stakeActiveEpoch")]
    public string StakeActiveEpoch { get; set; }

    /// <summary>
    /// The principal amount staked
    /// </summary>
    [JsonPropertyName("principal")]
    public string Principal { get; set; }

    /// <summary>
    /// The status of the stake (Active, Pending, Unstaked)
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; }

    /// <summary>
    /// The estimated reward for the stake (if applicable)
    /// </summary>
    [JsonPropertyName("estimatedReward")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string EstimatedReward { get; set; }
}