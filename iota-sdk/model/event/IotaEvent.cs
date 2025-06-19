using Newtonsoft.Json;

namespace iota_sdk.model.@event;

/// <summary>
/// Represents an IOTA event
/// </summary>
public class IotaEvent
{
    /// <summary>
    /// Event identifier
    /// </summary>
    [JsonProperty("id")]
    public EventId Id { get; set; }

    /// <summary>
    /// Package ID
    /// </summary>
    [JsonProperty("packageId")]
    public string PackageId { get; set; }

    /// <summary>
    /// Transaction module
    /// </summary>
    [JsonProperty("transactionModule")]
    public string TransactionModule { get; set; }

    /// <summary>
    /// Sender address
    /// </summary>
    [JsonProperty("sender")]
    public string Sender { get; set; }

    /// <summary>
    /// Event type
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; set; }

    /// <summary>
    /// Parsed JSON content
    /// </summary>
    [JsonProperty("parsedJson")]
    public dynamic ParsedJson { get; set; }

    /// <summary>
    /// BCS encoding format
    /// </summary>
    [JsonProperty("bcsEncoding")]
    public string BcsEncoding { get; set; }

    /// <summary>
    /// BCS encoded data
    /// </summary>
    [JsonProperty("bcs")]
    public string Bcs { get; set; }
}