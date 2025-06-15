using Newtonsoft.Json;

/// <summary>
/// This is the JSON-RPC type for the IOTA validator. It flattens all inner
/// structures to top-level fields so that they are decoupled from the internal
/// definitions.
/// </summary>
public class IotaValidatorSummary
{
    // Metadata
    [JsonProperty("iotaAddress")]
    public string IotaAddress { get; set; }

    [JsonProperty("authorityPubkeyBytes")]
    public string AuthorityPubkeyBytes { get; set; }

    [JsonProperty("networkPubkeyBytes")]
    public string NetworkPubkeyBytes { get; set; }

    [JsonProperty("protocolPubkeyBytes")]
    public string ProtocolPubkeyBytes { get; set; }

    [JsonProperty("proofOfPossessionBytes")]
    public string ProofOfPossessionBytes { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("imageUrl")]
    public string ImageUrl { get; set; }

    [JsonProperty("projectUrl")]
    public string ProjectUrl { get; set; }

    [JsonProperty("netAddress")]
    public string NetAddress { get; set; }

    [JsonProperty("p2pAddress")]
    public string P2pAddress { get; set; }

    [JsonProperty("primaryAddress")]
    public string PrimaryAddress { get; set; }

    [JsonProperty("nextEpochAuthorityPubkeyBytes")]
    public string NextEpochAuthorityPubkeyBytes { get; set; }

    [JsonProperty("nextEpochProofOfPossession")]
    public string NextEpochProofOfPossession { get; set; }

    [JsonProperty("nextEpochNetworkPubkeyBytes")]
    public string NextEpochNetworkPubkeyBytes { get; set; }

    [JsonProperty("nextEpochProtocolPubkeyBytes")]
    public string NextEpochProtocolPubkeyBytes { get; set; }

    [JsonProperty("nextEpochNetAddress")]
    public string NextEpochNetAddress { get; set; }

    [JsonProperty("nextEpochP2pAddress")]
    public string NextEpochP2pAddress { get; set; }

    [JsonProperty("nextEpochPrimaryAddress")]
    public string NextEpochPrimaryAddress { get; set; }

    [JsonProperty("votingPower")]
    public ulong VotingPower { get; set; }

    [JsonProperty("operationCapId")]
    public string OperationCapId { get; set; }

    [JsonProperty("gasPrice")]
    public ulong GasPrice { get; set; }

    [JsonProperty("commissionRate")]
    public ulong CommissionRate { get; set; }

    [JsonProperty("nextEpochStake")]
    public ulong NextEpochStake { get; set; }

    [JsonProperty("nextEpochGasPrice")]
    public ulong NextEpochGasPrice { get; set; }

    [JsonProperty("nextEpochCommissionRate")]
    public ulong NextEpochCommissionRate { get; set; }

    // Staking pool information
    /// <summary>
    /// ID of the staking pool object.
    /// </summary>
    [JsonProperty("stakingPoolId")]
    public string StakingPoolId { get; set; }

    /// <summary>
    /// The epoch at which this pool became active.
    /// </summary>
    [JsonProperty("stakingPoolActivationEpoch")]
    public ulong? StakingPoolActivationEpoch { get; set; }

    /// <summary>
    /// The epoch at which this staking pool ceased to be active. `null` = {pre-active, active}
    /// </summary>
    [JsonProperty("stakingPoolDeactivationEpoch")]
    public ulong? StakingPoolDeactivationEpoch { get; set; }

    /// <summary>
    /// The total number of IOTA tokens in this pool.
    /// </summary>
    [JsonProperty("stakingPoolIotaBalance")]
    public ulong StakingPoolIotaBalance { get; set; }

    /// <summary>
    /// The epoch stake rewards will be added here at the end of each epoch.
    /// </summary>
    [JsonProperty("rewardsPool")]
    public ulong RewardsPool { get; set; }

    /// <summary>
    /// Total number of pool tokens issued by the pool.
    /// </summary>
    [JsonProperty("poolTokenBalance")]
    public ulong PoolTokenBalance { get; set; }

    /// <summary>
    /// Pending stake amount for this epoch.
    /// </summary>
    [JsonProperty("pendingStake")]
    public ulong PendingStake { get; set; }

    /// <summary>
    /// Pending stake withdrawn during the current epoch, emptied at epoch boundaries.
    /// </summary>
    [JsonProperty("pendingTotalIotaWithdraw")]
    public ulong PendingTotalIotaWithdraw { get; set; }

    /// <summary>
    /// Pending pool token withdrawn during the current epoch, emptied at epoch boundaries.
    /// </summary>
    [JsonProperty("pendingPoolTokenWithdraw")]
    public ulong PendingPoolTokenWithdraw { get; set; }

    /// <summary>
    /// ID of the exchange rate table object.
    /// </summary>
    [JsonProperty("exchangeRatesId")]
    public string ExchangeRatesId { get; set; }

    /// <summary>
    /// Number of exchange rates in the table.
    /// </summary>
    [JsonProperty("exchangeRatesSize")]
    public ulong ExchangeRatesSize { get; set; }
}