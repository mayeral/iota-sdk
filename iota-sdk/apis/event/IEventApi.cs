using iota_sdk.model.@event;
using iota_sdk.model.read;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;

namespace iota_sdk.apis.@event
{
    /// <summary>
    /// Defines methods to fetch, query, or subscribe to events on the IOTA network.
    /// </summary>
    public interface IEventApi
    {
        /// <summary>
        /// Subscribe to receive a stream of filtered events.
        /// Subscription is only possible via WebSockets.
        /// </summary>
        /// <param name="filter">The event filter to apply</param>
        /// <returns>A stream of events</returns>
        Task<IAsyncEnumerable<IotaEvent>> SubscribeEventAsync(EventFilter filter);

        /// <summary>
        /// Get a list of events for the given transaction digest.
        /// </summary>
        /// <param name="digest">The transaction digest</param>
        /// <returns>A list of events</returns>
        Task<IEnumerable<IotaEvent>> GetEventsAsync(TransactionDigest digest);

        /// <summary>
        /// Get a list of filtered events.
        /// The response is paginated and can be ordered ascending or descending.
        /// </summary>
        /// <param name="query">The event filter to apply</param>
        /// <param name="cursor">Optional cursor for pagination</param>
        /// <param name="limit">Optional limit for results per page</param>
        /// <param name="descendingOrder">Whether to order results in descending order</param>
        /// <returns>A page of events</returns>
        Task<EventPage> QueryEventsAsync(EventFilter query, EventID cursor = null, int? limit = null, bool descendingOrder = false);

        /// <summary>
        /// Get a stream of filtered events which can be ordered ascending or descending.
        /// </summary>
        /// <param name="query">The event filter to apply</param>
        /// <param name="cursor">Optional cursor for pagination</param>
        /// <param name="descendingOrder">Whether to order results in descending order</param>
        /// <returns>A stream of events</returns>
        IAsyncEnumerable<IotaEvent> GetEventsStreamAsync(EventFilter query, EventID cursor = null, bool descendingOrder = false);
    }
}