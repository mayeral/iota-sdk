using Newtonsoft.Json;

namespace iota_sdk.model.@event;

/// <summary>
/// Response object for events query
/// </summary>
public class EventsResponse
{
    /// <summary>
    /// List of events
    /// </summary>
    [JsonProperty("data")]
    public List<IotaEvent>? Data { get; set; }

    /// <summary>
    /// Indicates if there are more pages available
    /// </summary>
    [JsonProperty("hasNextPage")]
    public bool HasNextPage { get; set; }

    /// <summary>
    /// Cursor for pagination
    /// </summary>
    [JsonProperty("nextCursor")]
    public EventId NextCursor { get; set; }
}