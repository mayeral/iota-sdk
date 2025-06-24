using Newtonsoft.Json;
namespace iota_sdk.model.governance;

/// <summary>
/// Represents an individual stake
/// </summary>
public class Stake
{
    /// <summary>
    /// The ID of the staked IOTA
    /// </summary>
    [JsonProperty("stakedIotaId")]
    public string StakedIotaId { get; set; }

    /// <summary>
    /// The epoch when the stake request was made
    /// </summary>
    [JsonProperty("stakeRequestEpoch")]
    public string StakeRequestEpoch { get; set; }

    /// <summary>
    /// The epoch when the stake becomes active
    /// </summary>
    [JsonProperty("stakeActiveEpoch")]
    public string StakeActiveEpoch { get; set; }

    /// <summary>
    /// The principal amount staked
    /// </summary>
    [JsonProperty("principal")]
    public string Principal { get; set; }

    /// <summary>
    /// The status of the stake (Active, Pending, Unstaked)
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; }

    /// <summary>
    /// The estimated reward for the stake (if applicable)
    /// </summary>
    [JsonProperty("estimatedReward", NullValueHandling = NullValueHandling.Ignore)]
    public string EstimatedReward { get; set; }
}