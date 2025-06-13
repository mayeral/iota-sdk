using Newtonsoft.Json;
using System.Numerics;
using System.Text.Json.Serialization;
using JsonConverter = Newtonsoft.Json.JsonConverter;

/// <summary>
/// RPC representation of the Committee type.
/// </summary>
public class IotaCommittee
{
    /// <summary>
    /// The epoch identifier.
    /// </summary>
    [JsonPropertyName("epoch")]
    public BigInteger Epoch { get; set; }

    /// <summary>
    /// List of validators with their stake units.
    /// Each validator is represented by a tuple of (AuthorityPublicKeyBytes, StakeUnit).
    /// </summary>
    [JsonPropertyName("validators")]
    public List<ValidatorInfo> Validators { get; set; } = new List<ValidatorInfo>();

    public IotaCommittee()
    {
        Validators = new List<ValidatorInfo>();
    }
}

[Newtonsoft.Json.JsonConverter(typeof(ValidatorInfoConverter))]
public class ValidatorInfo
{
    [JsonPropertyName("AuthorityName")]
    public string AuthorityName { get; set; }

    [JsonPropertyName("StakeUnit")]
    public BigInteger StakeUnit { get; set; }

    public ValidatorInfo(string authorityName, BigInteger stakeUnit)
    {
        AuthorityName = authorityName;
        StakeUnit = stakeUnit;
    }
}

public class ValidatorInfoConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(ValidatorInfo);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.StartArray)
        {
            string authorityName = (string) reader.ReadAsString();
            reader.Read(); // Advance to the next token
            BigInteger stakeUnit = BigInteger.Parse((string)reader.Value ?? string.Empty);
            reader.Read(); // Advance to the next token
            return new ValidatorInfo(authorityName, stakeUnit);
        }

        throw new JsonSerializationException("Invalid JSON format returned for ValidatorInfo.");
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        ValidatorInfo validatorInfo = (ValidatorInfo)value;
        writer.WriteStartArray();
        writer.WriteValue(validatorInfo.AuthorityName);
        writer.WriteValue(validatorInfo.StakeUnit);
        writer.WriteEndArray();
    }
}