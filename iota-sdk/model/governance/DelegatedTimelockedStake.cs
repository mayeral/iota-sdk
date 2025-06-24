using Newtonsoft.Json;

namespace iota_sdk.model.governance;

/// <summary>
/// Represents a delegated timelocked stake.
/// </summary>
public class DelegatedTimelockedStake
{
    /// <summary>
    /// Gets or sets the validator address.
    /// </summary>
    [JsonProperty("validatorAddress")]
    public string ValidatorAddress { get; set; }

    /// <summary>
    /// Gets or sets the staking pool object ID.
    /// </summary>
    [JsonProperty("stakingPool")]
    public string StakingPool { get; set; }

    /// <summary>
    /// Gets or sets the collection of timelocked stakes.
    /// </summary>
    [JsonProperty("stakes")]
    public List<TimelockedStake> Stakes { get; set; }
}