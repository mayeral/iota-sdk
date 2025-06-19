using System.Text.Json.Serialization;

namespace iota_sdk.model.read
{
    /// <summary>
    /// Filter either by the digest, or the sequence number, or neither, to get the latest checkpoint.
    /// </summary>
    public class CheckpointId
    {
        /// <summary>
        /// The checkpoint digest.
        /// </summary>
        [JsonPropertyName("digest")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Digest { get; set; }
            
        /// <summary>
        /// The checkpoint sequence number.
        /// </summary>
        [JsonPropertyName("sequence_number")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
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