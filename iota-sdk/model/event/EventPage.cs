using System.Text.Json.Serialization;

namespace iota_sdk.model.@event;

// EventPage class to match the response structure
public class EventPage
{
    [JsonPropertyName("data")]
    public List<IotaEvent> Data { get; set; }
    
    [JsonPropertyName("hasNextPage")]
    public bool HasNextPage { get; set; }
    
    [JsonPropertyName("nextCursor")]
    public EventId? NextCursor { get; set; }
}