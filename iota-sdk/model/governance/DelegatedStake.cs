
using Newtonsoft.Json;

namespace iota_sdk.model.governance;

/// <summary>
/// Represents delegated stake information for a validator
/// </summary>
public class DelegatedStake
{
    /// <summary>
    /// Validator's Address.
    /// </summary>
    [JsonProperty("validatorAddress")]
    public IotaAddress ValidatorAddress { get; set; }

    /// <summary>
    /// Staking pool object id.
    /// </summary>
    [JsonProperty("stakingPool")]
    public string StakingPool { get; set; }

    /// <summary>
    /// List of stakes for this validator
    /// </summary>
    [JsonProperty("stakes")]
    public List<Stake> Stakes { get; set; }
}