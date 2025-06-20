using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Iota.Sdk.Model.Read;
using iota_sdk.model.@event;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace iota_sdk.model.read;

/// <summary>
/// Represents a response for an IOTA transaction block.
/// </summary>
public class IotaTransactionBlockResponse
{
    /// <summary>
    /// Balance changes caused by the transaction.
    /// </summary>
    [JsonProperty("balanceChanges", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("balanceChanges")]
    public List<BalanceChange>? BalanceChanges { get; set; }

    /// <summary>
    /// The checkpoint number when this transaction was included and hence finalized.
    /// This is only returned in the read API, not in the transaction execution API.
    /// </summary>
    [JsonProperty("checkpoint", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("checkpoint")]
    public string? Checkpoint { get; set; }

    /// <summary>
    /// Indicates whether local execution was confirmed.
    /// </summary>
    [JsonProperty("confirmedLocalExecution", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("confirmedLocalExecution")]
    public bool? ConfirmedLocalExecution { get; set; }

    /// <summary>
    /// The digest of the transaction.
    /// </summary>
    [JsonProperty("digest")]
    [JsonPropertyName("digest")]
    public string Digest { get; set; } = string.Empty;

    /// <summary>
    /// Effects of the transaction.
    /// </summary>
    [JsonProperty("effects", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("effects")]
    public IotaTransactionBlockEffects? Effects { get; set; }

    /// <summary>
    /// Errors that occurred during transaction processing, if any.
    /// </summary>
    [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("errors")]
    public List<string>? Errors { get; set; }

    /// <summary>
    /// Events emitted during transaction processing.
    /// </summary>
    [JsonProperty("events", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("events")]
    public List<IotaEvent>? Events { get; set; }

    /// <summary>
    /// Object changes caused by the transaction.
    /// </summary>
    [JsonProperty("objectChanges", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("objectChanges")]
    public List<IotaObjectChange>? ObjectChanges { get; set; }

    /// <summary>
    /// Raw effects data.
    /// </summary>
    [JsonProperty("rawEffects", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("rawEffects")]
    public int[]? RawEffects { get; set; }

    /// <summary>
    /// BCS encoded SenderSignedData that includes input object references.
    /// Returns empty array if show_raw_transaction is false.
    /// </summary>
    [JsonProperty("rawTransaction", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("rawTransaction")]
    public string? RawTransaction { get; set; }

    /// <summary>
    /// Timestamp in milliseconds when the transaction was processed.
    /// </summary>
    [JsonProperty("timestampMs", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("timestampMs")]
    public string? TimestampMs { get; set; }

    /// <summary>
    /// Transaction input data.
    /// </summary>
    [JsonProperty("transaction", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("transaction")]
    public IotaTransactionBlock? Transaction { get; set; }
}

/// <summary>
/// Represents a change in balance for a specific coin type.
/// </summary>
public class BalanceChange
{
    /// <summary>
    /// The amount indicating the balance value changes.
    /// Negative amount means spending coin value and positive means receiving coin value.
    /// </summary>
    [JsonProperty("amount")]
    [JsonPropertyName("amount")]
    public string Amount { get; set; } = string.Empty;

    /// <summary>
    /// The type of coin that changed.
    /// </summary>
    [JsonProperty("coinType")]
    [JsonPropertyName("coinType")]
    public string CoinType { get; set; } = string.Empty;

    /// <summary>
    /// Owner of the balance change.
    /// </summary>
    [JsonProperty("owner")]
    [JsonPropertyName("owner")]
    public ObjectOwner Owner { get; set; } = null!;
}

/// <summary>
/// Represents an IOTA transaction block with data and signatures.
/// </summary>
public class IotaTransactionBlock
{
    /// <summary>
    /// Transaction block data.
    /// </summary>
    [JsonProperty("data")]
    [JsonPropertyName("data")]
    public TransactionBlockData Data { get; set; } = null!;

    /// <summary>
    /// Transaction signatures.
    /// </summary>
    [JsonProperty("txSignatures")]
    [JsonPropertyName("txSignatures")]
    public List<string> TxSignatures { get; set; } = new List<string>();
}

/// <summary>
/// Represents the data of a transaction block.
/// </summary>
public class TransactionBlockData
{
    /// <summary>
    /// Gas data for the transaction.
    /// </summary>
    [JsonProperty("gasData")]
    [JsonPropertyName("gasData")]
    public IotaGasData GasData { get; set; } = null!;

    /// <summary>
    /// Message version.
    /// </summary>
    [JsonProperty("messageVersion")]
    [JsonPropertyName("messageVersion")]
    public string MessageVersion { get; set; } = "v1";

    /// <summary>
    /// Sender address of the transaction.
    /// </summary>
    [JsonProperty("sender")]
    [JsonPropertyName("sender")]
    public string Sender { get; set; } = string.Empty;

    /// <summary>
    /// Transaction block kind.
    /// </summary>
    //[JsonProperty("transaction")]
    //[JsonPropertyName("transaction")]
    //public IotaTransactionBlockKind Transaction { get; set; } = null!; // TODO
}

/// <summary>
/// Represents gas data for an IOTA transaction.
/// </summary>
public class IotaGasData
{
    /// <summary>
    /// Gas budget for the transaction.
    /// </summary>
    [JsonProperty("budget")]
    [JsonPropertyName("budget")]
    public string Budget { get; set; } = string.Empty;

    /// <summary>
    /// Owner of the gas payment.
    /// </summary>
    [JsonProperty("owner")]
    [JsonPropertyName("owner")]
    public string Owner { get; set; } = string.Empty;

    /// <summary>
    /// Payment objects for gas.
    /// </summary>
    [JsonProperty("payment")]
    [JsonPropertyName("payment")]
    public List<IotaObjectRef> Payment { get; set; } = new List<IotaObjectRef>();

    /// <summary>
    /// Gas price for the transaction.
    /// </summary>
    [JsonProperty("price")]
    [JsonPropertyName("price")]
    public string Price { get; set; } = string.Empty;
}

/// <summary>
/// Represents a reference to an IOTA object.
/// </summary>
public class IotaObjectRef
{
    /// <summary>
    /// Base64 string representing the object digest.
    /// </summary>
    [JsonProperty("digest")]
    [JsonPropertyName("digest")]
    public string Digest { get; set; } = string.Empty;

    /// <summary>
    /// Hex code as string representing the object id.
    /// </summary>
    [JsonProperty("objectId")]
    [JsonPropertyName("objectId")]
    public string ObjectId { get; set; } = string.Empty;

    /// <summary>
    /// Object version.
    /// </summary>
    [JsonProperty("version")]
    [JsonPropertyName("version")]
    [System.Text.Json.Serialization.JsonConverter(typeof(VersionConverter))]
    public string Version { get; set; } = string.Empty;
}

 /// <summary>
    /// Converter for handling version fields that can be either strings or numbers.
    /// </summary>
    public class VersionConverter : System.Text.Json.Serialization.JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return reader.GetString() ?? string.Empty;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt64().ToString();
            }
            
            throw new JsonException($"Unexpected token type: {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            // If the value is a valid number, write it as a number, otherwise as a string
            if (long.TryParse(value, out var number))
            {
                writer.WriteNumberValue(number);
            }
            else
            {
                writer.WriteStringValue(value);
            }
        }
    }

    /// <summary>
    /// Newtonsoft.Json converter for version fields that can be either strings or numbers.
    /// </summary>
    public class NewtonsoftVersionConverter : Newtonsoft.Json.JsonConverter<string>
    {
        public override string ReadJson(JsonReader reader, Type objectType, string existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            
            if (token.Type == JTokenType.String)
            {
                return token.Value<string>() ?? string.Empty;
            }
            else if (token.Type == JTokenType.Integer)
            {
                return token.Value<long>().ToString();
            }
            
            throw new Newtonsoft.Json.JsonException($"Unexpected token type: {token.Type}");
        }

        public override void WriteJson(JsonWriter writer, string value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (long.TryParse(value, out var number))
            {
                writer.WriteValue(number);
            }
            else
            {
                writer.WriteValue(value);
            }
        }
    }

/// <summary>
/// Base class for object changes in IOTA transactions.
/// Object changes are derived from object mutations in the TransactionEffect to provide richer object information.
/// </summary>
[System.Text.Json.Serialization.JsonConverter(typeof(IotaObjectChangeConverter))]
public abstract class IotaObjectChange
{
    /// <summary>
    /// The type of object change.
    /// </summary>
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
}

/// <summary>
/// Represents a module published change.
/// </summary>
public class PublishedObjectChange : IotaObjectChange
{
    public PublishedObjectChange()
    {
        Type = "published";
    }

    /// <summary>
    /// The digest of the published module.
    /// </summary>
    [JsonProperty("digest")]
    [JsonPropertyName("digest")]
    public string Digest { get; set; } = string.Empty;

    /// <summary>
    /// The list of modules published.
    /// </summary>
    [JsonProperty("modules")]
    [JsonPropertyName("modules")]
    public List<string> Modules { get; set; } = new List<string>();

    /// <summary>
    /// The package ID.
    /// </summary>
    [JsonProperty("packageId")]
    [JsonPropertyName("packageId")]
    public string PackageId { get; set; } = string.Empty;

    /// <summary>
    /// The version of the published module.
    /// </summary>
    [JsonProperty("version")]
    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;
}

/// <summary>
/// Represents a transferred object change.
/// </summary>
public class TransferredObjectChange : IotaObjectChange
{
    public TransferredObjectChange()
    {
        Type = "transferred";
    }

    /// <summary>
    /// The digest of the transferred object.
    /// </summary>
    [JsonProperty("digest")]
    [JsonPropertyName("digest")]
    public string Digest { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the transferred object.
    /// </summary>
    [JsonProperty("objectId")]
    [JsonPropertyName("objectId")]
    public string ObjectId { get; set; } = string.Empty;

    /// <summary>
    /// The type of the transferred object.
    /// </summary>
    [JsonProperty("objectType")]
    [JsonPropertyName("objectType")]
    public string ObjectType { get; set; } = string.Empty;

    /// <summary>
    /// The recipient of the transferred object.
    /// </summary>
    [JsonProperty("recipient")]
    [JsonPropertyName("recipient")]
    public ObjectOwner Recipient { get; set; } = null!;

    /// <summary>
    /// The sender of the transferred object.
    /// </summary>
    [JsonProperty("sender")]
    [JsonPropertyName("sender")]
    public string Sender { get; set; } = string.Empty;

    /// <summary>
    /// The version of the transferred object.
    /// </summary>
    [JsonProperty("version")]
    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;
}

/// <summary>
/// Represents a mutated object change.
/// </summary>
public class MutatedObjectChange : IotaObjectChange
{
    public MutatedObjectChange()
    {
        Type = "mutated";
    }

    /// <summary>
    /// The digest of the mutated object.
    /// </summary>
    [JsonProperty("digest")]
    [JsonPropertyName("digest")]
    public string Digest { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the mutated object.
    /// </summary>
    [JsonProperty("objectId")]
    [JsonPropertyName("objectId")]
    public string ObjectId { get; set; } = string.Empty;

    /// <summary>
    /// The type of the mutated object.
    /// </summary>
    [JsonProperty("objectType")]
    [JsonPropertyName("objectType")]
    public string ObjectType { get; set; } = string.Empty;

    /// <summary>
    /// The owner of the mutated object.
    /// </summary>
    [JsonProperty("owner")]
    [JsonPropertyName("owner")]
    public ObjectOwner Owner { get; set; } = null!;

    /// <summary>
    /// The previous version of the mutated object.
    /// </summary>
    [JsonProperty("previousVersion")]
    [JsonPropertyName("previousVersion")]
    public string PreviousVersion { get; set; } = string.Empty;

    /// <summary>
    /// The sender of the mutation.
    /// </summary>
    [JsonProperty("sender")]
    [JsonPropertyName("sender")]
    public string Sender { get; set; } = string.Empty;

    /// <summary>
    /// The new version of the mutated object.
    /// </summary>
    [JsonProperty("version")]
    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;
}

/// <summary>
/// Represents a deleted object change.
/// </summary>
public class DeletedObjectChange : IotaObjectChange
{
    public DeletedObjectChange()
    {
        Type = "deleted";
    }

    /// <summary>
    /// The ID of the deleted object.
    /// </summary>
    [JsonProperty("objectId")]
    [JsonPropertyName("objectId")]
    public string ObjectId { get; set; } = string.Empty;

    /// <summary>
    /// The type of the deleted object.
    /// </summary>
    [JsonProperty("objectType")]
    [JsonPropertyName("objectType")]
    public string ObjectType { get; set; } = string.Empty;

    /// <summary>
    /// The sender who deleted the object.
    /// </summary>
    [JsonProperty("sender")]
    [JsonPropertyName("sender")]
    public string Sender { get; set; } = string.Empty;

    /// <summary>
    /// The version of the deleted object.
    /// </summary>
    [JsonProperty("version")]
    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;
}

/// <summary>
/// Represents a wrapped object change.
/// </summary>
public class WrappedObjectChange : IotaObjectChange
{
    public WrappedObjectChange()
    {
        Type = "wrapped";
    }

    /// <summary>
    /// The ID of the wrapped object.
    /// </summary>
    [JsonProperty("objectId")]
    [JsonPropertyName("objectId")]
    public string ObjectId { get; set; } = string.Empty;

    /// <summary>
    /// The type of the wrapped object.
    /// </summary>
    [JsonProperty("objectType")]
    [JsonPropertyName("objectType")]
    public string ObjectType { get; set; } = string.Empty;

    /// <summary>
    /// The sender who wrapped the object.
    /// </summary>
    [JsonProperty("sender")]
    [JsonPropertyName("sender")]
    public string Sender { get; set; } = string.Empty;

    /// <summary>
    /// The version of the wrapped object.
    /// </summary>
    [JsonProperty("version")]
    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;
}



/// <summary>
/// Represents a created object change.
/// </summary>
public class CreatedObjectChange : IotaObjectChange
{
    public CreatedObjectChange()
    {
        Type = "created";
    }

    /// <summary>
    /// The digest of the created object.
    /// </summary>
    [JsonProperty("digest")]
    [JsonPropertyName("digest")]
    public string Digest { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the created object.
    /// </summary>
    [JsonProperty("objectId")]
    [JsonPropertyName("objectId")]
    public string ObjectId { get; set; } = string.Empty;

    /// <summary>
    /// The type of the created object.
    /// </summary>
    [JsonProperty("objectType")]
    [JsonPropertyName("objectType")]
    public string ObjectType { get; set; } = string.Empty;

    /// <summary>
    /// The owner of the created object.
    /// </summary>
    [JsonProperty("owner")]
    [JsonPropertyName("owner")]
    public ObjectOwner Owner { get; set; } = null!;

    /// <summary>
    /// The sender who created the object.
    /// </summary>
    [JsonProperty("sender")]
    [JsonPropertyName("sender")]
    public string Sender { get; set; } = string.Empty;

    /// <summary>
    /// The version of the created object.
    /// </summary>
    [JsonProperty("version")]
    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;
}
/// <summary>
/// JSON converter for IotaObjectChange to handle the different types of object changes.
/// </summary>
public class IotaObjectChangeConverter : System.Text.Json.Serialization.JsonConverter<IotaObjectChange>
{
    public override IotaObjectChange Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Save the current position
        var readerAtStart = reader;

        // Parse the JSON object to get the "type" property
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        if (!root.TryGetProperty("type", out var typeProperty))
        {
            throw new JsonException("Cannot determine the type of IotaObjectChange: missing 'type' property");
        }

        var type = typeProperty.GetString();
        var json = root.GetRawText();

        // Create the appropriate concrete class based on the type
        return type switch
        {
            "published" => JsonSerializer.Deserialize<PublishedObjectChange>(json, options)!,
            "transferred" => JsonSerializer.Deserialize<TransferredObjectChange>(json, options)!,
            "mutated" => JsonSerializer.Deserialize<MutatedObjectChange>(json, options)!,
            "deleted" => JsonSerializer.Deserialize<DeletedObjectChange>(json, options)!,
            "wrapped" => JsonSerializer.Deserialize<WrappedObjectChange>(json, options)!,
            "created" => JsonSerializer.Deserialize<CreatedObjectChange>(json, options)!,
            _ => throw new JsonException($"Unknown IotaObjectChange type: {type}")
        };
    }

    public override void Write(Utf8JsonWriter writer, IotaObjectChange value, JsonSerializerOptions options)
    {
        // Serialize the specific derived type
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}