using iota_sdk.model.@event;
using Newtonsoft.Json;

namespace iota_sdk.model.read;

/// <summary>
/// Additional arguments supplied to dev inspect beyond what is allowed in today's API.
/// </summary>
public class DevInspectArgs
{
    /// <summary>
    /// The gas budget for the transaction.
    /// </summary>
    [JsonProperty("gasBudget")]
    public string? GasBudget { get; set; }

    /// <summary>
    /// The gas objects used to pay for the transaction.
    /// </summary>
    [JsonProperty("gasObjects")]
    public List<string[]>? GasObjects { get; set; }

    /// <summary>
    /// The sponsor of the gas for the transaction, might be different from the sender.
    /// </summary>
    [JsonProperty("gasSponsor")]
    public string? GasSponsor { get; set; }

    /// <summary>
    /// Whether to return the raw transaction data and effects.
    /// </summary>
    [JsonProperty("showRawTxnDataAndEffects")]
    public bool? ShowRawTxnDataAndEffects { get; set; }

    /// <summary>
    /// Whether to skip transaction checks for the transaction.
    /// </summary>
    [JsonProperty("skipChecks")]
    public bool? SkipChecks { get; set; }
}

/// <summary>
/// The response from processing a dev inspect transaction
/// </summary>
public class DevInspectResults
{
    /// <summary>
    /// Summary of effects that likely would be generated if the transaction is actually run. Note however,
    /// that not all dev-inspect transactions are actually usable as transactions so it might not be
    /// possible actually generate these effects from a normal transaction.
    /// </summary>
    [JsonProperty("effects", Required = Required.Always)]
    [JsonRequired]
    public TransactionEffects Effects { get; set; } = null!;

    /// <summary>
    /// Execution error from executing the transactions
    /// </summary>
    [JsonProperty("error")]
    public string? Error { get; set; }

    /// <summary>
    /// Events that likely would be generated if the transaction is actually run.
    /// </summary>
    [JsonProperty("events", Required = Required.Always)]
    [JsonRequired]
    public IotaEvent[] Events { get; set; } = null!;

    /// <summary>
    /// The raw effects of the transaction that was dev inspected.
    /// </summary>
    [JsonProperty("rawEffects")]
    public int[]? RawEffects { get; set; }

    /// <summary>
    /// The raw transaction data that was dev inspected.
    /// </summary>
    [JsonProperty("rawTxnData")]
    public int[]? RawTxnData { get; set; }

    /// <summary>
    /// Execution results (including return values) from executing the transactions
    /// </summary>
    //[JsonProperty("results")]
    //[JsonProperty("results")]
    //public IotaExecutionResult[]? Results { get; set; } // TODO
}

public class TransactionEffects
{
    // TODO
}