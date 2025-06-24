using Newtonsoft.Json;

namespace iota_sdk.model.read.checkpoint
{
    /// <summary>
    /// Filter either by the digest, or the sequence number, or neither, to get the latest checkpoint.
    /// </summary>
    public class CheckpointId
    {
        /// <summary>
        /// The checkpoint digest.
        /// </summary>
        [JsonProperty("digest", NullValueHandling = NullValueHandling.Ignore)]
        public string? Digest { get; set; }
            
        /// <summary>
        /// The checkpoint sequence number.
        /// </summary>
        [JsonProperty("sequence_number", NullValueHandling = NullValueHandling.Ignore)]
        public ulong? SequenceNumber { get; set; }

        /// <summary>
        /// Creates a CheckpointId by sequence number.
        /// </summary>
        /// <param name="seqNum">The sequence number</param>
        /// <returns>A CheckpointId with only sequence number set</returns>
        public static CheckpointId BySequenceNumber(ulong seqNum)
        {
            return new CheckpointId
            {
                SequenceNumber = seqNum,
                Digest = null
            };
        }

        /// <summary>
        /// Creates a CheckpointId by digest.
        /// </summary>
        /// <param name="digest">The checkpoint digest</param>
        /// <returns>A CheckpointId with only digest set</returns>
        public static CheckpointId ByDigest(string digest)
        {
            return new CheckpointId
            {
                Digest = digest,
                SequenceNumber = null
            };
        }
    }
}