using iota_sdk.model.@event;
using iota_sdk.model.read.@object;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iota_sdk.model.read.transaction;

/// <summary>
/// Represents a response for an IOTA transaction block.
/// </summary>
public class IotaTransactionBlockResponse
{
	/// <summary>
	/// Balance changes caused by the transaction.
	/// </summary>
	[JsonProperty("balanceChanges", NullValueHandling = NullValueHandling.Ignore)]
	public List<BalanceChange>? BalanceChanges { get; set; }

	/// <summary>
	/// The checkpoint number when this transaction was included and hence finalized.
	/// This is only returned in the read API, not in the transaction execution API.
	/// </summary>
	[JsonProperty("checkpoint", NullValueHandling = NullValueHandling.Ignore)]
	public string? Checkpoint { get; set; }

	/// <summary>
	/// Indicates whether local execution was confirmed.
	/// </summary>
	[JsonProperty("confirmedLocalExecution", NullValueHandling = NullValueHandling.Ignore)]
	public bool? ConfirmedLocalExecution { get; set; }

	/// <summary>
	/// The digest of the transaction.
	/// </summary>
	[JsonProperty("digest")]
	public string Digest { get; set; } = string.Empty;

	/// <summary>
	/// Effects of the transaction.
	/// </summary>
	[JsonProperty("effects", NullValueHandling = NullValueHandling.Ignore)]
	public IotaTransactionBlockEffects? Effects { get; set; }

	/// <summary>
	/// Errors that occurred during transaction processing, if any.
	/// </summary>
	[JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
	public List<string>? Errors { get; set; }

	/// <summary>
	/// Events emitted during transaction processing.
	/// </summary>
	[JsonProperty("events", NullValueHandling = NullValueHandling.Ignore)]
	public List<IotaEvent>? Events { get; set; }

	///// <summary>
	///// Object changes caused by the transaction.
	///// </summary>
	//[JsonProperty("objectChanges", NullValueHandling = NullValueHandling.Ignore)]
	//public List<IotaObjectChange>? ObjectChanges { get; set; } // TODO SERIALIZATON FAILS

	/// <summary>
	/// Raw effects data.
	/// </summary>
	[JsonProperty("rawEffects", NullValueHandling = NullValueHandling.Ignore)]
	public int[]? RawEffects { get; set; }

	/// <summary>
	/// BCS encoded SenderSignedData that includes input object references.
	/// Returns empty array if show_raw_transaction is false.
	/// </summary>
	[JsonProperty("rawTransaction", NullValueHandling = NullValueHandling.Ignore)]
	public string? RawTransaction { get; set; }

	/// <summary>
	/// Timestamp in milliseconds when the transaction was processed.
	/// </summary>
	[JsonProperty("timestampMs", NullValueHandling = NullValueHandling.Ignore)]
	public string? TimestampMs { get; set; }

	/// <summary>
	/// Transaction input data.
	/// </summary>
	[JsonProperty("transaction", NullValueHandling = NullValueHandling.Ignore)]
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
	public string Amount { get; set; } = string.Empty;

	/// <summary>
	/// The type of coin that changed.
	/// </summary>
	[JsonProperty("coinType")]
	public string CoinType { get; set; } = string.Empty;

	/// <summary>
	/// Owner of the balance change.
	/// </summary>
	[JsonProperty("owner")]
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
	public TransactionBlockData Data { get; set; } = null!;

	/// <summary>
	/// Transaction signatures.
	/// </summary>
	[JsonProperty("txSignatures")]
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
	public IotaGasData GasData { get; set; } = null!;

	/// <summary>
	/// Message version.
	/// </summary>
	[JsonProperty("messageVersion")]
	public string MessageVersion { get; set; } = "v1";

	/// <summary>
	/// Sender address of the transaction.
	/// </summary>
	[JsonProperty("sender")]
	public string Sender { get; set; } = string.Empty;

	/// <summary>
	/// Transaction block kind.
	/// </summary>
	//[JsonProperty("transaction")]
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
	public string Budget { get; set; } = string.Empty;

	/// <summary>
	/// Owner of the gas payment.
	/// </summary>
	[JsonProperty("owner")]
	public string Owner { get; set; } = string.Empty;

	/// <summary>
	/// Payment objects for gas.
	/// </summary>
	[JsonProperty("payment")]
	public List<IotaObjectRef> Payment { get; set; } = new List<IotaObjectRef>();

	/// <summary>
	/// Gas price for the transaction.
	/// </summary>
	[JsonProperty("price")]
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
	public string Digest { get; set; } = string.Empty;

	/// <summary>
	/// Hex code as string representing the object id.
	/// </summary>
	[JsonProperty("objectId")]
	public string ObjectId { get; set; } = string.Empty;

	/// <summary>
	/// Object version.
	/// </summary>
	[JsonProperty("version")]
	[JsonConverter(typeof(VersionConverter))]
	public string Version { get; set; } = string.Empty;
}

/// <summary>
/// Newtonsoft.Json converter for version fields that can be either strings or numbers.
/// </summary>
public class VersionConverter : JsonConverter<string>
{
	public override string ReadJson(JsonReader reader, Type objectType, string existingValue, bool hasExistingValue, JsonSerializer serializer)
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

		throw new JsonSerializationException($"Unexpected token type: {token.Type}");
	}

	public override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer)
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
[JsonConverter(typeof(IotaObjectChangeConverter))]
public abstract class IotaObjectChange
{
	/// <summary>
	/// The type of object change.
	/// </summary>
	[JsonProperty("type")]
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
	public string Digest { get; set; } = string.Empty;

	/// <summary>
	/// The list of modules published.
	/// </summary>
	[JsonProperty("modules")]
	public List<string> Modules { get; set; } = new List<string>();

	/// <summary>
	/// The package ID.
	/// </summary>
	[JsonProperty("packageId")]
	public string PackageId { get; set; } = string.Empty;

	/// <summary>
	/// The version of the published module.
	/// </summary>
	[JsonProperty("version")]
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
	public string Digest { get; set; } = string.Empty;

	/// <summary>
	/// The ID of the transferred object.
	/// </summary>
	[JsonProperty("objectId")]
	public string ObjectId { get; set; } = string.Empty;

	/// <summary>
	/// The type of the transferred object.
	/// </summary>
	[JsonProperty("objectType")]
	public string ObjectType { get; set; } = string.Empty;

	/// <summary>
	/// The recipient of the transferred object.
	/// </summary>
	[JsonProperty("recipient")]
	public ObjectOwner Recipient { get; set; } = null!;

	/// <summary>
	/// The sender of the transferred object.
	/// </summary>
	[JsonProperty("sender")]
	public string Sender { get; set; } = string.Empty;

	/// <summary>
	/// The version of the transferred object.
	/// </summary>
	[JsonProperty("version")]
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
	public string Digest { get; set; } = string.Empty;

	/// <summary>
	/// The ID of the mutated object.
	/// </summary>
	[JsonProperty("objectId")]
	public string ObjectId { get; set; } = string.Empty;

	/// <summary>
	/// The type of the mutated object.
	/// </summary>
	[JsonProperty("objectType")]
	public string ObjectType { get; set; } = string.Empty;

	/// <summary>
	/// The owner of the mutated object.
	/// </summary>
	[JsonProperty("owner")]
	public ObjectOwner Owner { get; set; } = null!;

	/// <summary>
	/// The previous version of the mutated object.
	/// </summary>
	[JsonProperty("previousVersion")]
	public string PreviousVersion { get; set; } = string.Empty;

	/// <summary>
	/// The sender of the mutation.
	/// </summary>
	[JsonProperty("sender")]
	public string Sender { get; set; } = string.Empty;

	/// <summary>
	/// The new version of the mutated object.
	/// </summary>
	[JsonProperty("version")]
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
	public string ObjectId { get; set; } = string.Empty;

	/// <summary>
	/// The type of the deleted object.
	/// </summary>
	[JsonProperty("objectType")]
	public string ObjectType { get; set; } = string.Empty;

	/// <summary>
	/// The sender who deleted the object.
	/// </summary>
	[JsonProperty("sender")]
	public string Sender { get; set; } = string.Empty;

	/// <summary>
	/// The version of the deleted object.
	/// </summary>
	[JsonProperty("version")]
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
	public string ObjectId { get; set; } = string.Empty;

	/// <summary>
	/// The type of the wrapped object.
	/// </summary>
	[JsonProperty("objectType")]
	public string ObjectType { get; set; } = string.Empty;

	/// <summary>
	/// The sender who wrapped the object.
	/// </summary>
	[JsonProperty("sender")]
	public string Sender { get; set; } = string.Empty;

	/// <summary>
	/// The version of the wrapped object.
	/// </summary>
	[JsonProperty("version")]
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
	public string Digest { get; set; } = string.Empty;

	/// <summary>
	/// The ID of the created object.
	/// </summary>
	[JsonProperty("objectId")]
	public string ObjectId { get; set; } = string.Empty;

	/// <summary>
	/// The type of the created object.
	/// </summary>
	[JsonProperty("objectType")]
	public string ObjectType { get; set; } = string.Empty;

	/// <summary>
	/// The owner of the created object.
	/// </summary>
	[JsonProperty("owner")]
	public ObjectOwner Owner { get; set; } = null!;

	/// <summary>
	/// The sender who created the object.
	/// </summary>
	[JsonProperty("sender")]
	public string Sender { get; set; } = string.Empty;

	/// <summary>
	/// The version of the created object.
	/// </summary>
	[JsonProperty("version")]
	public string Version { get; set; } = string.Empty;
}
/// <summary>
/// JSON converter for IotaObjectChange to handle the different types of object changes.
/// </summary>
public class IotaObjectChangeConverter : JsonConverter
{
	public override bool CanConvert(Type objectType)
	{
		return typeof(IotaObjectChange).IsAssignableFrom(objectType);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		if (reader.TokenType != JsonToken.StartObject)
		{
			throw new JsonSerializationException("Expected start of object");
		}

		JObject jsonObject = JObject.Load(reader);

		if (!jsonObject.TryGetValue("type", out var typeToken))
		{
			throw new JsonSerializationException("Cannot determine the type of IotaObjectChange: missing 'type' property");
		}

		var type = typeToken.Value<string>();

		// Create the appropriate concrete class based on the type
		return type switch
		{
			"published" => jsonObject.ToObject<PublishedObjectChange>(serializer)!,
			"transferred" => jsonObject.ToObject<TransferredObjectChange>(serializer)!,
			"mutated" => jsonObject.ToObject<MutatedObjectChange>(serializer)!,
			"deleted" => jsonObject.ToObject<DeletedObjectChange>(serializer)!,
			"wrapped" => jsonObject.ToObject<WrappedObjectChange>(serializer)!,
			"created" => jsonObject.ToObject<CreatedObjectChange>(serializer)!,
			_ => throw new JsonSerializationException($"Unknown IotaObjectChange type: {type}")
		};
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		// Simply serialize the object as-is since each derived type has its own properties
		serializer.Serialize(writer, value);
	}
}