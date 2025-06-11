using System.Text.Json.Serialization;

namespace iota_sdk.model.governance;

/// <summary>
/// Represents a delegated timelocked stake.
/// </summary>
public class DelegatedTimelockedStake
{
    /// <summary>
    /// Gets or sets the validator address.
    /// </summary>
    [JsonPropertyName("validatorAddress")]
    public string ValidatorAddress { get; set; }

    /// <summary>
    /// Gets or sets the staking pool object ID.
    /// </summary>
    [JsonPropertyName("stakingPool")]
    public string StakingPool { get; set; }

    /// <summary>
    /// Gets or sets the collection of timelocked stakes.
    /// </summary>
    [JsonPropertyName("stakes")]
    public List<TimelockedStake> Stakes { get; set; }
}