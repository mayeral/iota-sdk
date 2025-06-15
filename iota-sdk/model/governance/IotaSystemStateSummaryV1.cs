using Newtonsoft.Json;

namespace iota_sdk.model.governance;

/// <summary>
/// This is the JSON-RPC type for the IotaSystemStateV1 object.
/// It flattens all fields to make them top-level fields such that it has
/// minimum dependencies to the internal data structures of the IOTA system state type.
/// </summary>
public class IotaSystemStateSummaryV1
{
    /// <summary>
    /// The current epoch ID, starting from 0.
    /// </summary>
    [JsonProperty("epoch")]
    public ulong Epoch { get; set; }

    /// <summary>
    /// The current protocol version, starting from 1.
    /// </summary>
    [JsonProperty("protocolVersion")]
    public ulong ProtocolVersion { get; set; }

    /// <summary>
    /// The current version of the system state data structure type.
    /// </summary>
    [JsonProperty("systemStateVersion")]
    public ulong SystemStateVersion { get; set; }

    /// <summary>
    /// The current IOTA supply.
    /// </summary>
    [JsonProperty("iotaTotalSupply")]
    public ulong IotaTotalSupply { get; set; }

    /// <summary>
    /// The TreasuryCap<IOTA> object ID.
    /// </summary>
    [JsonProperty("iotaTreasuryCapId")]
    public string IotaTreasuryCapId { get; set; }

    /// <summary>
    /// The storage rebates of all the objects on-chain stored in the storage fund.
    /// </summary>
    [JsonProperty("storageFundTotalObjectStorageRebates")]
    public ulong StorageFundTotalObjectStorageRebates { get; set; }

    /// <summary>
    /// The non-refundable portion of the storage fund coming from non-refundable storage rebates and any leftover staking rewards.
    /// </summary>
    [JsonProperty("storageFundNonRefundableBalance")]
    public ulong StorageFundNonRefundableBalance { get; set; }

    /// <summary>
    /// The reference gas price for the current epoch.
    /// </summary>
    [JsonProperty("referenceGasPrice")]
    public ulong ReferenceGasPrice { get; set; }

    /// <summary>
    /// Whether the system is running in a downgraded safe mode due to a non-recoverable b ug.
    /// </summary>
    [JsonProperty("safeMode")]
    public bool SafeMode { get; set; }

    /// <summary>
    /// Amount of storage charges accumulated (and not yet distributed) during safe mode.
    /// </summary>
    [JsonProperty("safeModeStorageCharges")]
    public ulong SafeModeStorageCharges { get; set; }

    /// <summary>
    /// Amount of computation rewards accumulated (and not yet distributed) during safe mode.
    /// </summary>
    [JsonProperty("safeModeComputationRewards")]
    public ulong SafeModeComputationRewards { get; set; }

    /// <summary>
    /// Amount of storage rebates accumulated (and not yet burned) during safe mode.
    /// </summary>
    [JsonProperty("safeModeStorageRebates")]
    public ulong SafeModeStorageRebates { get; set; }

    /// <summary>
    /// Amount of non-refundable storage fee accumulated during safe mode.
    /// </summary>
    [JsonProperty("safeModeNonRefundableStorageFee")]
    public ulong SafeModeNonRefundableStorageFee { get; set; }

    /// <summary>
    /// Unix timestamp of the current epoch start
    /// </summary>
    [JsonProperty("epochStartTimestampMs")]
    public ulong EpochStartTimestampMs { get; set; }

    /// <summary>
    /// The duration of an epoch, in milliseconds.
    /// </summary>
    [JsonProperty("epochDurationMs")]
    public ulong EpochDurationMs { get; set; }

    /// <summary>
    /// Minimum number of active validators at any moment.
    /// </summary>
    [JsonProperty("minValidatorCount")]
    public ulong MinValidatorCount { get; set; }

    /// <summary>
    /// Maximum number of active validators at any moment.
    /// </summary>
    [JsonProperty("maxValidatorCount")]
    public ulong MaxValidatorCount { get; set; }

    /// <summary>
    /// Lower-bound on the amount of stake required to become a validator.
    /// </summary>
    [JsonProperty("minValidatorJoiningStake")]
    public ulong MinValidatorJoiningStake { get; set; }

    /// <summary>
    /// Validators with stake amount below this threshold are considered to have low stake.
    /// </summary>
    [JsonProperty("validatorLowStakeThreshold")]
    public ulong ValidatorLowStakeThreshold { get; set; }

    /// <summary>
    /// Validators with stake below this threshold will be removed immediately at epoch change.
    /// </summary>
    [JsonProperty("validatorVeryLowStakeThreshold")]
    public ulong ValidatorVeryLowStakeThreshold { get; set; }

    /// <summary>
    /// A validator can have stake below validatorLowStakeThreshold for this many epochs before being kicked out.
    /// </summary>
    [JsonProperty("validatorLowStakeGracePeriod")]
    public ulong ValidatorLowStakeGracePeriod { get; set; }

    /// <summary>
    /// Total amount of stake from all active validators at the beginning of the epoch.
    /// </summary>
    [JsonProperty("totalStake")]
    public ulong TotalStake { get; set; }

    /// <summary>
    /// The list of active validators in the current epoch.
    /// </summary>
    [JsonProperty("activeValidators")]
    public List<IotaValidatorSummary> ActiveValidators { get; set; }

    /// <summary>
    /// ID of the object that contains the list of new validators that will join at the end of the epoch.
    /// </summary>
    [JsonProperty("pendingActiveValidatorsId")]
    public string PendingActiveValidatorsId { get; set; }

    /// <summary>
    /// Number of new validators that will join at the end of the epoch.
    /// </summary>
    [JsonProperty("pendingActiveValidatorsSize")]
    public ulong PendingActiveValidatorsSize { get; set; }

    /// <summary>
    /// Removal requests from the validators. Each element is an index pointing to activeValidators.
    /// </summary>
    [JsonProperty("pendingRemovals")]
    public List<ulong> PendingRemovals { get; set; }

    /// <summary>
    /// ID of the object that maps from staking pool's ID to the iota address of a validator.
    /// </summary>
    [JsonProperty("stakingPoolMappingsId")]
    public string StakingPoolMappingsId { get; set; }

    /// <summary>
    /// Number of staking pool mappings.
    /// </summary>
    [JsonProperty("stakingPoolMappingsSize")]
    public ulong StakingPoolMappingsSize { get; set; }

    /// <summary>
    /// ID of the object that maps from a staking pool ID to the inactive validator that has that pool as its staking pool.
    /// </summary>
    [JsonProperty("inactivePoolsId")]
    public string InactivePoolsId { get; set; }

    /// <summary>
    /// Number of inactive staking pools.
    /// </summary>
    [JsonProperty("inactivePoolsSize")]
    public ulong InactivePoolsSize { get; set; }

    /// <summary>
    /// ID of the object that stores preactive validators, mapping their addresses to their Validator structs.
    /// </summary>
    [JsonProperty("validatorCandidatesId")]
    public string ValidatorCandidatesId { get; set; }

    /// <summary>
    /// Number of preactive validators.
    /// </summary>
    [JsonProperty("validatorCandidatesSize")]
    public ulong ValidatorCandidatesSize { get; set; }

    /// <summary>
    /// Map storing the number of epochs for which each validator has been below the low stake threshold.
    /// </summary>
    [JsonProperty("atRiskValidators")]
    public List<KeyValuePair<string, ulong>> AtRiskValidators { get; set; }

    /// <summary>
    /// A map storing the records of validator reporting each other.
    /// </summary>
    [JsonProperty("validatorReportRecords")]
    public List<KeyValuePair<string, List<string>>> ValidatorReportRecords { get; set; }

}