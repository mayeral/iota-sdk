using System.Text.Json;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("data")]
        public IotaObjectData? Data { get; set; }

        /// <summary>
        /// Gets or sets the error information if the request failed.
        /// </summary>
        [JsonPropertyName("error")]
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
        [JsonPropertyName("bcs")]
        public RawData? Bcs { get; set; }

        /// <summary>
        /// Gets or sets the Move object content or package content.
        /// Default is null unless IotaObjectDataOptions.ShowContent is set to true.
        /// </summary>
        [JsonPropertyName("content")]
        public IotaParsedData? Content { get; set; }

        /// <summary>
        /// Gets or sets the Base64 string representing the object digest.
        /// </summary>
        [JsonPropertyName("digest")]
        public string Digest { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Display metadata for frontend UI rendering.
        /// Default is null unless IotaObjectDataOptions.ShowContent is set to true.
        /// This can also be null if the struct type does not have Display defined.
        /// </summary>
        [JsonPropertyName("display")]
        public DisplayFieldsResponse? Display { get; set; }

        /// <summary>
        /// Gets or sets the object ID.
        /// </summary>
        [JsonPropertyName("objectId")]
        public string ObjectId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the owner of this object.
        /// Default is null unless IotaObjectDataOptions.ShowOwner is set to true.
        /// </summary>
        [JsonPropertyName("owner")]
        public ObjectOwner? Owner { get; set; }

        /// <summary>
        /// Gets or sets the digest of the transaction that created or last mutated this object.
        /// Default is null unless IotaObjectDataOptions.ShowPreviousTransaction is set to true.
        /// </summary>
        [JsonPropertyName("previousTransaction")]
        public string? PreviousTransaction { get; set; }

        /// <summary>
        /// Gets or sets the amount of IOTA that would be rebated if this object gets deleted.
        /// This number is re-calculated each time the object is mutated based on the present storage gas price.
        /// </summary>
        [JsonPropertyName("storageRebate")]
        public string? StorageRebate { get; set; }

        /// <summary>
        /// Gets or sets the type of the object.
        /// Default is null unless IotaObjectDataOptions.ShowType is set to true.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets the object version.
        /// </summary>
        [JsonPropertyName("version")]
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
        [JsonPropertyName("data")]
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
        [JsonPropertyName("data")]
        public Dictionary<string, string>? Data { get; set; }

        /// <summary>
        /// Gets or sets the error information if the request failed.
        /// </summary>
        [JsonPropertyName("error")]
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
        [JsonPropertyName("AddressOwner")]
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
        [JsonPropertyName("ObjectOwner")]
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
        [JsonPropertyName("Shared")]
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
        [JsonPropertyName("initial_shared_version")]
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
        public override ObjectOwner? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string value = reader.GetString() ?? string.Empty;
                if (value == "Immutable")
                {
                    return new ImmutableOwner();
                }
                throw new JsonException($"Unexpected string value for ObjectOwner: {value}");
            }

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException($"Expected object or string token, got {reader.TokenType}");
            }

            // Read the property name to determine the type
            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected property name");
            }

            string propertyName = reader.GetString() ?? string.Empty;

            switch (propertyName)
            {
                case "AddressOwner":
                    reader.Read(); // Move to the value
                    if (reader.TokenType != JsonTokenType.String)
                    {
                        throw new JsonException("Expected string value for AddressOwner");
                    }
                    string address = reader.GetString() ?? string.Empty;
                    reader.Read(); // Move to EndObject
                    return new AddressOwner { Address = address };

                case "ObjectOwner":
                    reader.Read(); // Move to the value
                    if (reader.TokenType != JsonTokenType.String)
                    {
                        throw new JsonException("Expected string value for ObjectOwner");
                    }
                    string objectId = reader.GetString() ?? string.Empty;
                    reader.Read(); // Move to EndObject
                    return new ObjectIdOwner { ObjectId = objectId };

                case "Shared":
                    reader.Read(); // Move to the StartObject
                    if (reader.TokenType != JsonTokenType.StartObject)
                    {
                        throw new JsonException("Expected object value for Shared");
                    }

                    reader.Read(); // Move to PropertyName
                    if (reader.TokenType != JsonTokenType.PropertyName || reader.GetString() != "initial_shared_version")
                    {
                        throw new JsonException("Expected 'initial_shared_version' property in Shared object");
                    }

                    reader.Read(); // Move to the value
                    string initialVersion;
                    if (reader.TokenType == JsonTokenType.String)
                    {
                        initialVersion = reader.GetString() ?? string.Empty;
                    }
                    else if (reader.TokenType == JsonTokenType.Number)
                    {
                        // Handle numeric version
                        initialVersion = reader.GetInt64().ToString();
                    }
                    else
                    {
                        throw new JsonException($"Expected string or number value for initial_shared_version, but got {reader.TokenType}");
                    }

                    reader.Read(); // Move to EndObject for Shared
                    reader.Read(); // Move to EndObject for the wrapper

                    return new SharedOwner
                    {
                        SharedInfo = new SharedOwnerInfo { InitialSharedVersion = initialVersion }
                    };

                default:
                    throw new JsonException($"Unknown property name: {propertyName}");
            }
        }

        public override void Write(Utf8JsonWriter writer, ObjectOwner value, JsonSerializerOptions options)
        {
            if (value is ImmutableOwner)
            {
                writer.WriteStringValue("Immutable");
                return;
            }

            writer.WriteStartObject();

            switch (value)
            {
                case AddressOwner addressOwner:
                    writer.WriteString("AddressOwner", addressOwner.Address);
                    break;

                case ObjectIdOwner objectOwner:
                    writer.WriteString("ObjectOwner", objectOwner.ObjectId);
                    break;

                case SharedOwner sharedOwner:
                    writer.WritePropertyName("Shared");
                    writer.WriteStartObject();
                    writer.WriteString("initial_shared_version", sharedOwner.SharedInfo.InitialSharedVersion);
                    writer.WriteEndObject();
                    break;

                default:
                    throw new JsonException($"Unknown ObjectOwner type: {value.GetType().Name}");
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
        [JsonPropertyName("dataType")]
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
        [JsonPropertyName("fields")]
        public JsonElement Fields { get; set; }

        /// <summary>
        /// Gets or sets the type of the Move object.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }

    /// JSON converter for IotaParsedData to handle different data types.
    /// </summary>
    public class IotaParsedDataJsonConverter : JsonConverter<IotaParsedData>
    {
        public override IotaParsedData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = doc.RootElement;

                // Check if dataType property exists
                if (!root.TryGetProperty("dataType", out JsonElement dataTypeElement))
                {
                    throw new JsonException("Missing required 'dataType' property for IotaParsedData");
                }

                string dataType = dataTypeElement.GetString() ?? string.Empty;

                switch (dataType)
                {
                    case "moveObject":
                        var moveObject = new MoveObjectContent
                        {
                            DataType = dataType
                        };

                        // Get type property
                        if (root.TryGetProperty("type", out JsonElement typeElement))
                        {
                            moveObject.Type = typeElement.GetString() ?? string.Empty;
                        }

                        // Get fields property
                        if (root.TryGetProperty("fields", out JsonElement fieldsElement))
                        {
                            moveObject.Fields = fieldsElement;
                        }

                        return moveObject;

                    case "package":
                        var packageContent = new PackageContent
                        {
                            DataType = dataType
                        };

                        // Get disassembled property
                        if (root.TryGetProperty("disassembled", out JsonElement disassembledElement))
                        {
                            // Convert JsonElement to Dictionary<string, object>
                            var disassembled = JsonSerializer.Deserialize<Dictionary<string, object>>(
                                disassembledElement.GetRawText(),
                                options
                            );

                            if (disassembled != null)
                            {
                                packageContent.Disassembled = disassembled;
                            }
                        }

                        return packageContent;

                    default:
                        throw new JsonException($"Unknown IotaParsedData type: {dataType}");
                }
            }
        }

        public override void Write(Utf8JsonWriter writer, IotaParsedData value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            // Write common properties
            writer.WriteString("dataType", value.DataType);

            switch (value)
            {
                case MoveObjectContent moveObject:
                    // Write Move object specific properties
                    writer.WriteString("type", moveObject.Type);

                    // Write fields as raw JSON
                    writer.WritePropertyName("fields");
                    JsonSerializer.Serialize(writer, moveObject.Fields, options);
                    break;

                case PackageContent packageContent:
                    // Write package specific properties
                    writer.WritePropertyName("disassembled");
                    JsonSerializer.Serialize(writer, packageContent.Disassembled, options);
                    break;

                default:
                    throw new JsonException($"Unknown IotaParsedData type: {value.GetType().Name}");
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
        [JsonPropertyName("disassembled")]
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
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }
}