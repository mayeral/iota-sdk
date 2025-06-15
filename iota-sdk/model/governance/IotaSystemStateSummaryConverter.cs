using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iota_sdk.model.governance;

/// <summary>
/// Custom JSON converter for IotaSystemStateSummary
/// </summary>
public class IotaSystemStateSummaryConverter : JsonConverter
{
    /// <inheritdoc />
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(IotaSystemStateSummary);
    }

    /// <inheritdoc />
    /// <inheritdoc />
    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        // Read the JSON into a JObject
        JObject jsonObject = JObject.Load(reader);

        // Check if there's a V2 property
        if (jsonObject["V2"] != null)
        {
            // The V2 format has a nested V2 object
            var v2 = jsonObject["V2"]?.ToObject<IotaSystemStateSummaryV2>(serializer);
            if (v2 != null) return new IotaSystemStateSummary { V2 = v2 };
        }
        else
        {
            // The V1 format is flattened
            // Validate that the JSON contains required V1 properties
            if (jsonObject["epoch"] != null)
            {
                var v1 = jsonObject.ToObject<IotaSystemStateSummaryV1>(serializer);
                if (v1 != null) return new IotaSystemStateSummary { V1 = v1 };
            }
        }

        throw new JsonSerializationException("Invalid JSON format returned for IotaSystemStateSummary.");
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        var summary = (IotaSystemStateSummary)value!;

        writer.WriteStartObject();

        if (summary.IsV2)
        {
            writer.WritePropertyName("V2");
            serializer.Serialize(writer, summary.V2);
        }
        else if (summary.IsV1)
        {
            // For V1, we should flatten the properties instead of nesting them under "V1"
            var v1Json = JObject.FromObject(summary.V1, serializer);
            foreach (var property in v1Json.Properties())
            {
                writer.WritePropertyName(property.Name);
                property.Value.WriteTo(writer);
            }
        }

        writer.WriteEndObject();
    }
}