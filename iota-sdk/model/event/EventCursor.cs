using Newtonsoft.Json;

namespace iota_sdk.model.@event;

/// <summary>
/// Cursor for event pagination
/// </summary>
public class EventCursor
{
    /// <summary>
    /// Transaction digest
    /// </summary>
    [JsonProperty("txDigest")]
    public string TxDigest { get; set; }

    /// <summary>
    /// Event sequence number
    /// </summary>
    [JsonProperty("eventSeq")]
    public string EventSeq { get; set; }
}