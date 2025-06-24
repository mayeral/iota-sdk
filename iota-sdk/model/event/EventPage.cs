using Newtonsoft.Json;

namespace iota_sdk.model.@event;

// EventPage class to match the response structure
public class EventPage
{
    [JsonProperty("data")]
    public List<IotaEvent> Data { get; set; }
    
    [JsonProperty("hasNextPage")]
    public bool HasNextPage { get; set; }
    
    [JsonProperty("nextCursor")]
    public EventId? NextCursor { get; set; }
}