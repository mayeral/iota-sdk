using Newtonsoft.Json;

namespace iota_sdk.model.read.@object
{
    /// <summary>
    /// Options for specifying what content to include when retrieving object data.
    /// </summary>
    public class IotaObjectDataOptions
    {
        /// <summary>
        /// Gets or sets whether to show the content in BCS format.
        /// Default is false.
        /// </summary>
        [JsonProperty("showBcs")]
        public bool? ShowBcs { get; set; }

        /// <summary>
        /// Gets or sets whether to show the content (i.e., package content or Move struct content) of the object.
        /// Default is false.
        /// </summary>
        [JsonProperty("showContent")]
        public bool? ShowContent { get; set; }

        /// <summary>
        /// Gets or sets whether to show the Display metadata of the object for frontend rendering.
        /// Default is false.
        /// </summary>
        [JsonProperty("showDisplay")]
        public bool? ShowDisplay { get; set; }

        /// <summary>
        /// Gets or sets whether to show the owner of the object.
        /// Default is false.
        /// </summary>
        [JsonProperty("showOwner")]
        public bool? ShowOwner { get; set; }

        /// <summary>
        /// Gets or sets whether to show the previous transaction digest of the object.
        /// Default is false.
        /// </summary>
        [JsonProperty("showPreviousTransaction")]
        public bool? ShowPreviousTransaction { get; set; }

        /// <summary>
        /// Gets or sets whether to show the storage rebate of the object.
        /// Default is false.
        /// </summary>
        [JsonProperty("showStorageRebate")]
        public bool? ShowStorageRebate { get; set; }

        /// <summary>
        /// Gets or sets whether to show the type of the object.
        /// Default is false.
        /// </summary>
        [JsonProperty("showType")]
        public bool? ShowType { get; set; }
    }
}