using iota_sdk.model.@event;
using iota_sdk.model.read;

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

    public async Task<IEnumerable<IotaEvent>> GetEventsAsync(TransactionDigest digest)
    {
        throw new NotImplementedException();
    }

    public async Task<EventPage> QueryEventsAsync(EventFilter query, EventID cursor = null, int? limit = null, bool descendingOrder = false)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<IotaEvent> GetEventsStreamAsync(EventFilter query, EventID cursor = null, bool descendingOrder = false)
    {
        throw new NotImplementedException();
    }
}