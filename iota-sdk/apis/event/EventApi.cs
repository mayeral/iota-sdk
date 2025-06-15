using Iota.Model.Read;
using iota_sdk.model.@event;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iota_sdk.apis.@event;

public class EventApi : IEventApi
{

    private readonly IotaClient _client;

    /// <summary>
    /// Creates a new instance of <see cref="EventApi"/>
    /// </summary>
    /// <param name="client">The IOTA client to use for API calls</param>
    public EventApi(IotaClient client)
    {
        _client = client;
    }


    public async Task<IAsyncEnumerable<IotaEvent>> SubscribeEventAsync(EventFilter filter)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get a list of events for the given transaction digest.
    /// </summary>
    /// <param name="digest">The transaction digest</param>
    /// <returns>A list of IOTA events</returns>
    public async Task<EventsResponse> GetEventsAsync(TransactionDigest digest)
    {
        // Convert the transaction digest to a string representation
        string digestString = digest.ToString();

        // Invoke the RPC method with the digest parameter
        var jsonResponse = await _client.InvokeRpcMethodAsync<JArray>("iota_getEvents", digestString).ConfigureAwait(false);

        // Create and populate the EventsResponse object
        var response = new EventsResponse
        {
            Data = jsonResponse.ToObject<List<IotaEvent>>(),
            HasNextPage = false // Set default values as needed
        };

        return response;
    }

    public async Task<EventPage> QueryEventsAsync(EventFilter query, EventId cursor = null, int? limit = null, bool descendingOrder = false)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<IotaEvent> GetEventsStreamAsync(EventFilter query, EventId cursor = null, bool descendingOrder = false)
    {
        throw new NotImplementedException();
    }
}

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
    /// Cursor for pagination
    /// </summary>
    [JsonProperty("nextCursor")]
    public EventCursor NextCursor { get; set; }

    /// <summary>
    /// Indicates if there are more pages available
    /// </summary>
    [JsonProperty("hasNextPage")]
    public bool HasNextPage { get; set; }
}

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

/// <summary>
/// Event identifier
/// </summary>
public class EventId
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