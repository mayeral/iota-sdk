using System.Reflection;
using Iota.Model.Read;

namespace iota_sdk.model.@event;

/// <summary>
/// Filter for querying events
/// </summary>
public class EventFilter
{
    /// <summary>
    /// Filter events by sender address
    /// </summary>
    public IotaAddress? Sender { get; set; }

    /// <summary>
    /// Filter events by transaction digest
    /// </summary>
    public TransactionDigest? TransactionDigest { get; set; }

    /// <summary>
    /// Events emitted by a particular module. An event is emitted by a
    /// particular module if some function in the module is called by a
    /// PTB and emits an event.
    /// 
    /// Modules can be filtered by their package, or package::module.
    /// We currently do not support filtering by emitting module and event type
    /// at the same time so if both are provided in one filter, the query will
    /// error.
    /// </summary>
    //public ModuleFilter? EmittingModule { get; set; }

    /// <summary>
    /// This field is used to specify the type of event emitted.
    /// 
    /// Events can be filtered by their type's package, package::module,
    /// or their fully qualified type name.
    /// 
    /// Generic types can be queried by either the generic type name, e.g.
    /// `0x2::coin::Coin`, or by the full type name, such as
    /// `0x2::coin::Coin&lt;0x2::iota::IOTA&gt;`.
    /// </summary>
    public TypeFilter? EventType { get; set; }

    // Future enhancements (post-MVP):
    // public DateTime? StartTime { get; set; }
    // public DateTime? EndTime { get; set; }
    // public EventFilter[] Any { get; set; }
    // public EventFilter[] All { get; set; }
    // public EventFilter Not { get; set; }
}