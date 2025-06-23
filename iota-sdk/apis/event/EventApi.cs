using iota_sdk.model.@event;
using iota_sdk.model.read;
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
        // :::note The subscribeEvent and subscribeTransaction methods are deprecated.
        // Please use queryEvents and queryTransactionBlocks instead.
        throw new NotImplementedException("subscribe event is deprecated use query events method");
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

    public async Task<EventPage> QueryEventsAsync(EventFilter filter, EventId? cursor = null, int? limit = null, bool descendingOrder = false)
    {
        // Create the parameters array for the RPC call
        var parameters = new List<object> { filter };

        // Add optional parameters if provided
        if (cursor != null)
        {
            parameters.Add(cursor);

            if (limit.HasValue)
                parameters.Add(limit.Value);

            parameters.Add(descendingOrder);
        }

        // Make the RPC call to iotax_queryEvents
        var response = await _client.InvokeRpcMethodAsync<EventPage>("iotax_queryEvents", parameters.ToArray()).ConfigureAwait(false);

        return response;
    }

    public async IAsyncEnumerable<IotaEvent> GetEventsStreamAsync(EventFilter query, EventId? cursor = null, bool descendingOrder = false)
    {
        EventId? currentCursor = cursor;
        bool hasMorePages = true;

        while (hasMorePages)
        {
            // Query the next page of events
            var page = await QueryEventsAsync(query, currentCursor, null, descendingOrder).ConfigureAwait(false);

            // Yield each event in the page
            foreach (var eventItem in page.Data)
            {
                yield return eventItem;
            }

            // Update cursor and check if more pages exist
            hasMorePages = page.HasNextPage;
            currentCursor = page.NextCursor;

            // If no more pages or no next cursor, break the loop
            if (!hasMorePages)
                break;
        }
    }
}