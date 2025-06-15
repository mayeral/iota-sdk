using Newtonsoft.Json;

namespace iota_sdk.model.governance
{
    /// <summary>
    /// This is the JSON-RPC type for IOTA system state objects.
    /// It is an enum type that can represent either V1 or V2 system state objects.
    /// </summary>
    [JsonConverter(typeof(IotaSystemStateSummaryConverter))]
    public class IotaSystemStateSummary
    {
        /// <summary>
        /// V1 of the system state summary
        /// </summary>
        public IotaSystemStateSummaryV1? V1 { get; set; }

        /// <summary>
        /// V2 of the system state summary
        /// </summary>
        public IotaSystemStateSummaryV2? V2 { get; set; }

        /// <summary>
        /// Whether this is a V1 system state summary
        /// </summary>
        [JsonIgnore]
        public bool IsV1 => V1 != null;

        /// <summary>
        /// Whether this is a V2 system state summary
        /// </summary>
        [JsonIgnore]
        public bool IsV2 => V2 != null;
    }
}