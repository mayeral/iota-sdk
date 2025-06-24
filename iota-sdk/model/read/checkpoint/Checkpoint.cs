using Newtonsoft.Json;

namespace iota_sdk.model.read.checkpoint
{
    /// <summary>
    /// The Checkpoint Class
    /// </summary>
    public class Checkpoint
    {
        /// <summary>
        /// Checkpoint's epoch ID
        /// </summary>
        [JsonProperty("epoch")]
        public ulong Epoch { get; set; }

        /// <summary>
        /// Checkpoint sequence number
        /// </summary>
        [JsonProperty("sequenceNumber")]
        public ulong SequenceNumber { get; set; }

        /// <summary>
        /// Checkpoint digest
        /// </summary>
        [JsonProperty("digest")]
        public string Digest { get; set; }

        /// <summary>
        /// Total number of transactions committed since genesis, including those in this checkpoint.
        /// </summary>
        [JsonProperty("networkTotalTransactions")]
        public ulong NetworkTotalTransactions { get; set; }

        /// <summary>
        /// Digest of the previous checkpoint
        /// </summary>
        [JsonProperty("previousDigest")]
        public string PreviousDigest { get; set; }

        /// <summary>
        /// The running total gas costs of all transactions included in the current epoch so far until this checkpoint.
        /// </summary>
        [JsonProperty("epochRollingGasCostSummary")]
        public GasCostSummary EpochRollingGasCostSummary { get; set; }

        /// <summary>
        /// Timestamp of the checkpoint - number of milliseconds from the Unix epoch
        /// </summary>
        [JsonProperty("timestampMs")]
        public ulong TimestampMs { get; set; }

        /// <summary>
        /// Present only on the final checkpoint of the epoch.
        /// </summary>
        //[JsonProperty("endOfEpochData")] // TODO
        //public EndOfEpochData EndOfEpochData { get; set; }

        /// <summary>
        /// Transaction digests
        /// </summary>
        [JsonProperty("transactions")]
        public List<string> Transactions { get; set; }


        /// <summary>
        /// Validator Signature
        /// </summary>
        //[JsonProperty("validatorSignature")] // TODO
        //public AggregateAuthoritySignature ValidatorSignature { get; set; }
    }


    /// <summary>
    /// Summary of gas costs for transactions
    /// </summary>
    public class GasCostSummary
    {
        /// <summary>
        /// Cost of computation/execution
        /// </summary>
        [JsonProperty("computationCost")]
        public ulong ComputationCost { get; set; }

        /// <summary>
        /// The burned component of the computation/execution costs
        /// </summary>
        [JsonProperty("computationCostBurned")]
        public ulong ComputationCostBurned { get; set; }

        /// <summary>
        /// Storage cost, it's the sum of all storage cost for all objects
        /// created or mutated.
        /// </summary>
        [JsonProperty("storageCost")]
        public ulong StorageCost { get; set; }

        /// <summary>
        /// The amount of storage cost refunded to the user for all objects
        /// deleted or mutated in the transaction.
        /// </summary>
        [JsonProperty("storageRebate")]
        public ulong StorageRebate { get; set; }

        /// <summary>
        /// The fee for the rebate. The portion of the storage rebate kept by
        /// the system.
        /// </summary>
        [JsonProperty("nonRefundableStorageFee")]
        public ulong NonRefundableStorageFee { get; set; }

        /// <summary>
        /// Creates a new instance of the GasCostSummary class with default values
        /// </summary>
        public GasCostSummary()
        {
            // Default constructor
        }
    }
}