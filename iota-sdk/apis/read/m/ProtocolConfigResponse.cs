using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Numerics;

namespace iota_sdk.apis.read.m
{
    /// <summary>
    /// Response containing protocol configuration information
    /// </summary>
    public class ProtocolConfigResponse
    {
        /// <summary>
        /// Minimum supported protocol version
        /// </summary>
        [JsonProperty("minSupportedProtocolVersion")]
        public BigInteger MinSupportedProtocolVersion { get; set; }

        /// <summary>
        /// Maximum supported protocol version
        /// </summary>
        [JsonProperty("maxSupportedProtocolVersion")]
        public BigInteger MaxSupportedProtocolVersion { get; set; }

        /// <summary>
        /// Current protocol version
        /// </summary>
        [JsonProperty("protocolVersion")]
        public BigInteger ProtocolVersion { get; set; }

        /// <summary>
        /// Feature flags for the protocol
        /// </summary>
        [JsonProperty("featureFlags")]
        public Dictionary<string, bool> FeatureFlags { get; set; } = new Dictionary<string, bool>();

        /// <summary>
        /// Protocol configuration attributes
        /// </summary>
        [JsonProperty("attributes")]
        public Dictionary<string, IotaProtocolConfigValue?> Attributes { get; set; } = new Dictionary<string, IotaProtocolConfigValue?>();
    }

    /// <summary>
    /// Represents a protocol configuration value which can be of different types
    /// </summary>
    [JsonConverter(typeof(IotaProtocolConfigValueConverter))]
    public class IotaProtocolConfigValue
    {
        /// <summary>
        /// The value stored in the configuration
        /// </summary>
        public object? Value { get; set; }
    }

    /// <summary>
    /// Custom JSON converter for IotaProtocolConfigValue to handle different value types
    /// </summary>
    public class IotaProtocolConfigValueConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IotaProtocolConfigValue);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var value = new IotaProtocolConfigValue();

            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    value.Value = reader.Value;
                    break;
                case JsonToken.Float:
                    value.Value = reader.Value;
                    break;
                case JsonToken.String:
                    value.Value = reader.Value;
                    break;
                case JsonToken.Boolean:
                    value.Value = reader.Value;
                    break;
                case JsonToken.StartObject:
                    // For complex objects
                    value.Value = JObject.Load(reader);
                    break;
                case JsonToken.StartArray:
                    // For arrays
                    value.Value = JArray.Load(reader);
                    break;
                default:
                    throw new JsonSerializationException($"Unexpected token type: {reader.TokenType}");
            }

            return value;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var configValue = value as IotaProtocolConfigValue;
            if (configValue == null || configValue.Value == null)
            {
                writer.WriteNull();
                return;
            }

            JToken.FromObject(configValue.Value).WriteTo(writer);
        }
    }
}