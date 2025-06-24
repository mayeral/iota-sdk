using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iota_sdk.model.read
{
	/// <summary>
	/// Represents a response containing IOTA object data or an error.
	/// </summary>
	public class IotaObjectResponse
	{
		/// <summary>
		/// Gets or sets the object data.
		/// </summary>
		[JsonProperty("data")]
		public IotaObjectData? Data { get; set; }

		/// <summary>
		/// Gets or sets the error information if the request failed.
		/// </summary>
		[JsonProperty("error")]
		public ObjectResponseError? Error { get; set; }
	}

	/// <summary>
	/// Represents detailed data for an IOTA object.
	/// </summary>
	public class IotaObjectData
	{
		/// <summary>
		/// Gets or sets the Move object content or package content in BCS.
		/// Default is null unless IotaObjectDataOptions.ShowBcs is set to true.
		/// </summary>
		[JsonProperty("bcs")]
		public RawData? Bcs { get; set; }

		/// <summary>
		/// Gets or sets the Move object content or package content.
		/// Default is null unless IotaObjectDataOptions.ShowContent is set to true.
		/// </summary>
		[JsonProperty("content")]
		public IotaParsedData? Content { get; set; }

		/// <summary>
		/// Gets or sets the Base64 string representing the object digest.
		/// </summary>
		[JsonProperty("digest")]
		public string Digest { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the Display metadata for frontend UI rendering.
		/// Default is null unless IotaObjectDataOptions.ShowContent is set to true.
		/// This can also be null if the struct type does not have Display defined.
		/// </summary>
		[JsonProperty("display")]
		public DisplayFieldsResponse? Display { get; set; }

		/// <summary>
		/// Gets or sets the object ID.
		/// </summary>
		[JsonProperty("objectId")]
		public string ObjectId { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the owner of this object.
		/// Default is null unless IotaObjectDataOptions.ShowOwner is set to true.
		/// </summary>
		[JsonProperty("owner")]
		public ObjectOwner? Owner { get; set; }

		/// <summary>
		/// Gets or sets the digest of the transaction that created or last mutated this object.
		/// Default is null unless IotaObjectDataOptions.ShowPreviousTransaction is set to true.
		/// </summary>
		[JsonProperty("previousTransaction")]
		public string? PreviousTransaction { get; set; }

		/// <summary>
		/// Gets or sets the amount of IOTA that would be rebated if this object gets deleted.
		/// This number is re-calculated each time the object is mutated based on the present storage gas price.
		/// </summary>
		[JsonProperty("storageRebate")]
		public string? StorageRebate { get; set; }

		/// <summary>
		/// Gets or sets the type of the object.
		/// Default is null unless IotaObjectDataOptions.ShowType is set to true.
		/// </summary>
		[JsonProperty("type")]
		public string? Type { get; set; }

		/// <summary>
		/// Gets or sets the object version.
		/// </summary>
		[JsonProperty("version")]
		public string Version { get; set; } = string.Empty;
	}

	/// <summary>
	/// Represents raw data in a specific format.
	/// </summary>
	public class RawData
	{
		/// <summary>
		/// Gets or sets the raw data as a string.
		/// </summary>
		[JsonProperty("data")]
		public string Data { get; set; } = string.Empty;
	}

	/// <summary>
	/// Represents display fields for UI rendering.
	/// </summary>
	public class DisplayFieldsResponse
	{
		/// <summary>
		/// Gets or sets the display data as a dictionary of key-value pairs.
		/// </summary>
		[JsonProperty("data")]
		public Dictionary<string, string>? Data { get; set; }

		/// <summary>
		/// Gets or sets the error information if the request failed.
		/// </summary>
		[JsonProperty("error")]
		public ObjectResponseError? Error { get; set; }
	}

	/// <summary>
	/// Represents the owner of an object.
	/// This is a base class for different ownership types.
	/// </summary>
	[JsonConverter(typeof(ObjectOwnerJsonConverter))]
	public abstract class ObjectOwner
	{
		// Base class for the different owner types
	}

	/// <summary>
	/// Represents an address owner - object is exclusively owned by a single address and is mutable.
	/// </summary>
	public class AddressOwner : ObjectOwner
	{
		/// <summary>
		/// Gets or sets the address.
		/// </summary>
		[JsonProperty("AddressOwner")]
		public string Address { get; set; } = string.Empty;
	}

	/// <summary>
	/// Represents an object owner - object is exclusively owned by a single object and is mutable.
	/// </summary>
	public class ObjectIdOwner : ObjectOwner
	{
		/// <summary>
		/// Gets or sets the object ID.
		/// </summary>
		[JsonProperty("ObjectOwner")]
		public string ObjectId { get; set; } = string.Empty;
	}

	/// <summary>
	/// Represents a shared owner - object is shared, can be used by any address, and is mutable.
	/// </summary>
	public class SharedOwner : ObjectOwner
	{
		/// <summary>
		/// Gets or sets the shared ownership information.
		/// </summary>
		[JsonProperty("Shared")]
		public SharedOwnerInfo SharedInfo { get; set; } = new SharedOwnerInfo();
	}

	/// <summary>
	/// Represents shared ownership information.
	/// </summary>
	public class SharedOwnerInfo
	{
		/// <summary>
		/// Gets or sets the version at which the object became shared.
		/// </summary>
		[JsonProperty("initial_shared_version")]
		public string InitialSharedVersion { get; set; } = string.Empty;
	}

	/// <summary>
	/// Represents an immutable owner - object cannot be modified.
	/// </summary>
	public class ImmutableOwner : ObjectOwner
	{
		// This class represents the "Immutable" string value in the JSON
	}

	/// <summary>
	/// JSON converter for ObjectOwner to handle the different ownership types.
	/// </summary>
	public class ObjectOwnerJsonConverter : JsonConverter<ObjectOwner>
	{
		public override ObjectOwner ReadJson(JsonReader reader, Type objectType, ObjectOwner existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.String)
			{
				string value = reader.Value?.ToString() ?? string.Empty;
				if (value == "Immutable")
				{
					return new ImmutableOwner();
				}
				throw new JsonSerializationException($"Unexpected string value for ObjectOwner: {value}");
			}

			if (reader.TokenType != JsonToken.StartObject)
			{
				throw new JsonSerializationException($"Expected object or string token, got {reader.TokenType}");
			}

			// Load the entire object and examine its properties
			JObject jsonObject = JObject.Load(reader);

			// Check which property exists to determine the type
			if (jsonObject.TryGetValue("AddressOwner", out JToken addressToken))
			{
				string address = addressToken.Value<string>() ?? string.Empty;
				return new AddressOwner { Address = address };
			}

			if (jsonObject.TryGetValue("ObjectOwner", out JToken objectToken))
			{
				string objectId = objectToken.Value<string>() ?? string.Empty;
				return new ObjectIdOwner { ObjectId = objectId };
			}

			if (jsonObject.TryGetValue("Shared", out JToken sharedToken))
			{
				var sharedObject = sharedToken as JObject;
				if (sharedObject == null)
				{
					throw new JsonSerializationException("Expected object value for Shared");
				}

				if (!sharedObject.TryGetValue("initial_shared_version", out JToken versionToken))
				{
					throw new JsonSerializationException("Expected 'initial_shared_version' property in Shared object");
				}

				string initialVersion;
				if (versionToken.Type == JTokenType.String)
				{
					initialVersion = versionToken.Value<string>() ?? string.Empty;
				}
				else if (versionToken.Type == JTokenType.Integer)
				{
					// Handle numeric version
					initialVersion = versionToken.Value<long>().ToString();
				}
				else
				{
					throw new JsonSerializationException($"Expected string or number value for initial_shared_version, but got {versionToken.Type}");
				}

				return new SharedOwner
				{
					SharedInfo = new SharedOwnerInfo { InitialSharedVersion = initialVersion }
				};
			}

			throw new JsonSerializationException("Unknown ObjectOwner type: missing expected property");
		}

		public override void WriteJson(JsonWriter writer, ObjectOwner value, JsonSerializer serializer)
		{
			if (value is ImmutableOwner)
			{
				writer.WriteValue("Immutable");
				return;
			}

			writer.WriteStartObject();

			switch (value)
			{
				case AddressOwner addressOwner:
					writer.WritePropertyName("AddressOwner");
					writer.WriteValue(addressOwner.Address);
					break;

				case ObjectIdOwner objectOwner:
					writer.WritePropertyName("ObjectOwner");
					writer.WriteValue(objectOwner.ObjectId);
					break;

				case SharedOwner sharedOwner:
					writer.WritePropertyName("Shared");
					writer.WriteStartObject();
					writer.WritePropertyName("initial_shared_version");
					writer.WriteValue(sharedOwner.SharedInfo.InitialSharedVersion);
					writer.WriteEndObject();
					break;

				default:
					throw new JsonSerializationException($"Unknown ObjectOwner type: {value.GetType().Name}");
			}

			writer.WriteEndObject();
		}
	}

	/// <summary>
	/// Base class for parsed IOTA data.
	/// </summary>
	[JsonConverter(typeof(IotaParsedDataJsonConverter))]

	public abstract class IotaParsedData
	{
		/// <summary>
		/// Gets or sets the data type.
		/// </summary>
		[JsonProperty("dataType")]
		public string DataType { get; set; } = string.Empty;
	}

	/// <summary>
	/// Represents a Move object content.
	/// </summary>
	public class MoveObjectContent : IotaParsedData
	{
		/// <summary>
		/// Gets or sets the fields of the Move struct.
		/// </summary>
		[JsonProperty("fields")]
		public JToken Fields { get; set; }

		/// <summary>
		/// Gets or sets the type of the Move object.
		/// </summary>
		[JsonProperty("type")]
		public string Type { get; set; } = string.Empty;
	}
	/// <summary>
	/// JSON converter for IotaParsedData to handle different data types.
	/// </summary>
	public class IotaParsedDataJsonConverter : JsonConverter<IotaParsedData>
	{
		public override IotaParsedData ReadJson(JsonReader reader, Type objectType, IotaParsedData existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			JObject jsonObject = JObject.Load(reader);

			// Check if dataType property exists
			if (!jsonObject.TryGetValue("dataType", out JToken dataTypeToken))
			{
				throw new JsonSerializationException("Missing required 'dataType' property for IotaParsedData");
			}

			string dataType = dataTypeToken.Value<string>() ?? string.Empty;

			switch (dataType)
			{
				case "moveObject":
					var moveObject = new MoveObjectContent
					{
						DataType = dataType
					};

					// Get type property
					if (jsonObject.TryGetValue("type", out JToken typeToken))
					{
						moveObject.Type = typeToken.Value<string>() ?? string.Empty;
					}

					// Get fields property
					if (jsonObject.TryGetValue("fields", out JToken fieldsToken))
					{
						moveObject.Fields = fieldsToken;
					}

					return moveObject;

				case "package":
					var packageContent = new PackageContent
					{
						DataType = dataType
					};

					// Get disassembled property
					if (jsonObject.TryGetValue("disassembled", out JToken disassembledToken))
					{
						// Convert JToken to Dictionary<string, object>
						var disassembled = disassembledToken.ToObject<Dictionary<string, object>>(serializer);

						if (disassembled != null)
						{
							packageContent.Disassembled = disassembled;
						}
					}

					return packageContent;

				default:
					throw new JsonSerializationException($"Unknown IotaParsedData type: {dataType}");
			}
		}

		public override void WriteJson(JsonWriter writer, IotaParsedData value, JsonSerializer serializer)
		{
			writer.WriteStartObject();

			// Write common properties
			writer.WritePropertyName("dataType");
			writer.WriteValue(value.DataType);

			switch (value)
			{
				case MoveObjectContent moveObject:
					// Write Move object specific properties
					writer.WritePropertyName("type");
					writer.WriteValue(moveObject.Type);

					// Write fields as raw JSON
					writer.WritePropertyName("fields");
					serializer.Serialize(writer, moveObject.Fields);
					break;

				case PackageContent packageContent:
					// Write package specific properties
					writer.WritePropertyName("disassembled");
					serializer.Serialize(writer, packageContent.Disassembled);
					break;

				default:
					throw new JsonSerializationException($"Unknown IotaParsedData type: {value.GetType().Name}");
			}

			writer.WriteEndObject();
		}
	}

	/// <summary>
	/// Represents a package content.
	/// </summary>
	public class PackageContent : IotaParsedData
	{
		/// <summary>
		/// Gets or sets the disassembled package data.
		/// </summary>
		[JsonProperty("disassembled")]
		public Dictionary<string, object> Disassembled { get; set; } = new Dictionary<string, object>();
	}

	/// <summary>
	/// Represents an error in an object response.
	/// </summary>
	public class ObjectResponseError
	{
		/// <summary>
		/// Gets or sets the error code.
		/// </summary>
		[JsonProperty("code")]
		public string Code { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the error message.
		/// </summary>
		[JsonProperty("message")]
		public string Message { get; set; } = string.Empty;
	}
}