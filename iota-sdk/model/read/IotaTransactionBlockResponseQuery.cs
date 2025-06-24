using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace iota_sdk.model.read;

/// <summary>
/// Query parameters for IOTA transaction block responses.
/// </summary>
public class IotaTransactionBlockResponseQuery
{
    /// <summary>
    /// If null, no filter will be applied.
    /// </summary>
    [JsonProperty("filter", NullValueHandling = NullValueHandling.Ignore)]
    public TransactionFilter? Filter { get; set; }

    /// <summary>
    /// Config which fields to include in the response, by default only digest is included.
    /// </summary>
    [JsonProperty("options")]
    public IotaTransactionBlockResponseOptions? Options { get; set; }
}

/// <summary>
/// Options for IOTA transaction block responses.
/// </summary>
public class IotaTransactionBlockResponseOptions
{
    /// <summary>
    /// Whether to show balance_changes. Default is False.
    /// </summary>
    [JsonProperty("showBalanceChanges", NullValueHandling = NullValueHandling.Ignore)]
    public bool? ShowBalanceChanges { get; set; }

    /// <summary>
    /// Whether to show transaction effects. Default is False.
    /// </summary>
    [JsonProperty("showEffects", NullValueHandling = NullValueHandling.Ignore)]
    public bool? ShowEffects { get; set; }

    /// <summary>
    /// Whether to show transaction events. Default is False.
    /// </summary>
    [JsonProperty("showEvents", NullValueHandling = NullValueHandling.Ignore)]
    public bool? ShowEvents { get; set; }

    /// <summary>
    /// Whether to show transaction input data. Default is False.
    /// </summary>
    [JsonProperty("showInput", NullValueHandling = NullValueHandling.Ignore)]
    public bool? ShowInput { get; set; }

    /// <summary>
    /// Whether to show object_changes. Default is False.
    /// </summary>
    [JsonProperty("showObjectChanges", NullValueHandling = NullValueHandling.Ignore)]
    public bool? ShowObjectChanges { get; set; }

    /// <summary>
    /// Whether to show raw transaction effects. Default is False.
    /// </summary>
    [JsonProperty("showRawEffects", NullValueHandling = NullValueHandling.Ignore)]
    public bool? ShowRawEffects { get; set; }

    /// <summary>
    /// Whether to show bcs-encoded transaction input data.
    /// </summary>
    [JsonProperty("showRawInput", NullValueHandling = NullValueHandling.Ignore)]
    public bool? ShowRawInput { get; set; }
}



/// <summary>
/// Represents various filter criteria for querying transaction blocks.
/// </summary>
public class TransactionFilter
{
    /// <summary>
    /// Query by checkpoint identifier.
    /// </summary>
    [JsonProperty("Checkpoint")]
    public string Checkpoint { get; set; }

    /// <summary>
    /// Query by Move function.
    /// </summary>
    [JsonProperty("MoveFunction")]
    public MoveFunctionFilter MoveFunction { get; set; }

    /// <summary>
    /// Query by input object identifier.
    /// </summary>
    [JsonProperty("InputObject")]
    public string InputObject { get; set; }

    /// <summary>
    /// Query by changed object identifier, including created, mutated and unwrapped objects.
    /// </summary>
    [JsonProperty("ChangedObject")]
    public string ChangedObject { get; set; }

    /// <summary>
    /// Query by sender address.
    /// </summary>
    [JsonProperty("FromAddress")]
    public string FromAddress { get; set; }

    /// <summary>
    /// Query by recipient address.
    /// </summary>
    [JsonProperty("ToAddress")]
    public string ToAddress { get; set; }

    /// <summary>
    /// Query by both sender and recipient address.
    /// </summary>
    [JsonProperty("FromAndToAddress")]
    public FromAndToAddressFilter FromAndToAddress { get; set; }

    /// <summary>
    /// Query transactions that have a given address as either sender or recipient.
    /// </summary>
    [JsonProperty("FromOrToAddress")]
    public FromOrToAddressFilter FromOrToAddress { get; set; }

    /// <summary>
    /// Query by transaction kind.
    /// </summary>
    [JsonProperty("TransactionKind")]
    public IotaTransactionKind TransactionKind { get; set; }

    /// <summary>
    /// Query transactions of any given kind in the input array.
    /// </summary>
    [JsonProperty("TransactionKindIn")]
    public IotaTransactionKind[] TransactionKindIn { get; set; }

    /// <summary>
    /// Creates a new filter to query by checkpoint.
    /// </summary>
    public static TransactionFilter ByCheckpoint(string checkpoint)
    {
        return new TransactionFilter { Checkpoint = checkpoint };
    }

    /// <summary>
    /// Creates a new filter to query by Move function.
    /// </summary>
    public static TransactionFilter ByMoveFunction(string package, string module = null, string function = null)
    {
        return new TransactionFilter
        {
            MoveFunction = new MoveFunctionFilter
            {
                Package = package,
                Module = module,
                Function = function
            }
        };
    }

    /// <summary>
    /// Creates a new filter to query by input object.
    /// </summary>
    public static TransactionFilter ByInputObject(string objectId)
    {
        return new TransactionFilter { InputObject = objectId };
    }

    /// <summary>
    /// Creates a new filter to query by changed object.
    /// </summary>
    public static TransactionFilter ByChangedObject(string objectId)
    {
        return new TransactionFilter { ChangedObject = objectId };
    }

    /// <summary>
    /// Creates a new filter to query by sender address.
    /// </summary>
    public static TransactionFilter ByFromAddress(string address)
    {
        return new TransactionFilter { FromAddress = address };
    }

    /// <summary>
    /// Creates a new filter to query by recipient address.
    /// </summary>
    public static TransactionFilter ByToAddress(string address)
    {
        return new TransactionFilter { ToAddress = address };
    }

    /// <summary>
    /// Creates a new filter to query by both sender and recipient address.
    /// </summary>
    public static TransactionFilter ByFromAndToAddress(string from, string to)
    {
        return new TransactionFilter
        {
            FromAndToAddress = new FromAndToAddressFilter
            {
                From = from,
                To = to
            }
        };
    }

    /// <summary>
    /// Creates a new filter to query by either sender or recipient address.
    /// </summary>
    public static TransactionFilter ByFromOrToAddress(string address)
    {
        return new TransactionFilter
        {
            FromOrToAddress = new FromOrToAddressFilter
            {
                Address = address
            }
        };
    }

    /// <summary>
    /// Creates a new filter to query by transaction kind.
    /// </summary>
    public static TransactionFilter ByTransactionKind(IotaTransactionKind kind)
    {
        return new TransactionFilter { TransactionKind = kind };
    }

    /// <summary>
    /// Creates a new filter to query by multiple transaction kinds.
    /// </summary>
    public static TransactionFilter ByTransactionKindIn(params IotaTransactionKind[] kinds)
    {
        return new TransactionFilter { TransactionKindIn = kinds };
    }
}

/// <summary>
/// Filter for Move function queries.
/// </summary>
public class MoveFunctionFilter
{
    /// <summary>
    /// Optional function name.
    /// </summary>
    [JsonProperty("function")]
    public string Function { get; set; }

    /// <summary>
    /// Optional module name.
    /// </summary>
    [JsonProperty("module")]
    public string Module { get; set; }

    /// <summary>
    /// Required package identifier.
    /// </summary>
    [JsonProperty("package")]
    public string Package { get; set; }
}

/// <summary>
/// Filter for queries by both sender and recipient address.
/// </summary>
public class FromAndToAddressFilter
{
    /// <summary>
    /// Sender address.
    /// </summary>
    [JsonProperty("from")]
    public string From { get; set; }

    /// <summary>
    /// Recipient address.
    /// </summary>
    [JsonProperty("to")]
    public string To { get; set; }
}

/// <summary>
/// Filter for queries by either sender or recipient address.
/// </summary>
public class FromOrToAddressFilter
{
    /// <summary>
    /// Address to match as either sender or recipient.
    /// </summary>
    [JsonProperty("addr")]
    public string Address { get; set; }
}

/// <summary>
/// Parameters for transferring an object.
/// </summary>
public class TransferObjectParams
{
    /// <summary>
    /// ID of the object to transfer.
    /// </summary>
    [JsonProperty("objectId")]
    public string ObjectId { get; set; }

    /// <summary>
    /// Recipient address.
    /// </summary>
    [JsonProperty("recipient")]
    public string Recipient { get; set; }
}

/// <summary>
/// Identifies a struct and the module it was defined in.
/// This class provides information about the origin of a type within the Move programming language.
/// </summary>
public class TypeOrigin
{
    /// <summary>
    /// Name of the data type (struct).
    /// </summary>
    [JsonProperty("datatype_name")]
    public string DatatypeName { get; set; }

    /// <summary>
    /// Name of the module where the type is defined.
    /// </summary>
    [JsonProperty("module_name")]
    public string ModuleName { get; set; }

    /// <summary>
    /// Package identifier containing the module.
    /// </summary>
    [JsonProperty("package")]
    public string Package { get; set; }
}

[JsonConverter(typeof(StringEnumConverter))]
public enum IotaTransactionKind
{
    [EnumMember(Value = "ProgrammableTransaction")]
    ProgrammableTransaction,
    
    [EnumMember(Value = "Genesis")]
    Genesis,
    
    [EnumMember(Value = "ConsensusCommitPrologueV1")]
    ConsensusCommitPrologueV1,
    
    [EnumMember(Value = "AuthenticatorStateUpdateV1")]
    AuthenticatorStateUpdateV1,
    
    [EnumMember(Value = "RandomnessStateUpdate")]
    RandomnessStateUpdate,
    
    [EnumMember(Value = "EndOfEpochTransaction")]
    EndOfEpochTransaction,
    
    [EnumMember(Value = "SystemTransaction")]
    SystemTransaction
}