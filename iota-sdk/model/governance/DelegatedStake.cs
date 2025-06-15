using System.Text.Json.Serialization;

namespace iota_sdk.model.governance
{
    /// <summary>
    /// Represents delegated stake information for a validator
    /// </summary>
    public class DelegatedStake
    {
        /// <summary>
        /// Validator's Address.
        /// </summary>
        [JsonPropertyName("validatorAddress")]
        public IotaAddress ValidatorAddress { get; set; }

        /// <summary>
        /// Staking pool object id.
        /// </summary>
        [JsonPropertyName("stakingPool")]
        public ObjectID StakingPool { get; set; }

        /// <summary>
        /// List of stakes for this validator
        /// </summary>
        [JsonPropertyName("stakes")]
        public List<Stake> Stakes { get; set; }
    }
}