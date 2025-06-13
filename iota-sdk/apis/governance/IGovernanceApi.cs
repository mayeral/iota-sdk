using iota_sdk.model.governance;
using System.Numerics;

namespace iota_sdk.apis.governance
{
    /// <summary>
    /// Defines methods to get committee and staking info.
    /// </summary>
    public interface IGovernanceApi
    {
        /// <summary>
        /// Get a list of delegated stakes for the given address.
        /// </summary>
        /// <param name="address">The IOTA address</param>
        /// <returns>A list of delegated stakes</returns>
        Task<IEnumerable<DelegatedStake>> GetStakesAsync(string address);

        /// <summary>
        /// Get a list of delegated timelocked stakes for the given address.
        /// </summary>
        /// <param name="address">The IOTA address</param>
        /// <returns>A list of delegated timelocked stakes</returns>
        Task<IEnumerable<DelegatedTimelockedStake>> GetTimelockedStakesAsync(string address);

        /// <summary>
        /// Get committee information for the given epoch.
        /// The epoch defaults to the current epoch.
        /// </summary>
        /// <param name="epoch">Optional epoch number</param>
        /// <returns>Committee information</returns>
        Task<IotaCommittee> GetCommitteeInfoAsync(BigInteger? epoch = null);

        /// <summary>
        /// Get the latest IOTA system state object on-chain.
        /// Use this method to access system information, such as the current epoch,
        /// the protocol version, the reference gas price, the total stake, active
        /// validators, and much more.
        /// Return the latest IOTA system state object on networks supporting protocol version &lt; 5. These are networks with node software release version &lt; 0.11.
        /// This is the JSON-RPC type for the IotaSystemStateV1 object. It flattens all fields to make them top-level fields such that it as minimum dependencies to the internal data structures of the IOTA system state type.
        /// </summary>
        /// <returns>The latest IOTA system state summary</returns>
        Task<IotaSystemStateSummary> GetLatestIotaSystemStateAsync();

        /// <summary>
        /// Get the latest IOTA system state object on-chain.
        /// Use this method to access system information, such as the current epoch,
        /// the protocol version, the reference gas price, the total stake, active
        /// validators, and much more.
        /// </summary>
        /// <returns>The latest IOTA system state summary</returns>
        /// Return the latest IOTA system state object on networks supporting protocol version >= 5. These are networks with node software release version >= 0.11.
        Task<IotaSystemStateSummary> GetLatestIotaSystemStateV2Async();

        /// <summary>
        /// Get the reference gas price for the network.
        /// </summary>
        /// <returns>The reference gas price</returns>
        Task<ulong> GetReferenceGasPriceAsync();

        /// <summary>
        /// Get the validators APY.
        /// </summary>
        /// <returns>Validators APY information</returns>
        Task<ValidatorApys> GetValidatorsApyAsync();
    }
}