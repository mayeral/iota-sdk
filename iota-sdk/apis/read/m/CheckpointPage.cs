using System.Numerics;
using iota_sdk.apis.read.m;
using Newtonsoft.Json;

/// <summary>
/// Represents a paginated response of checkpoints
/// </summary>
public class CheckpointPage
{
    /// <summary>
    /// List of checkpoints in this page
    /// </summary>
    [JsonProperty("data")]
    public List<Checkpoint> Data { get; set; }

    /// <summary>
    /// Indicates if there are more pages available
    /// </summary>
    [JsonProperty("hasNextPage")]
    public bool HasNextPage { get; set; }

    /// <summary>
    /// Cursor pointing to the last item in the page.
    /// Used for fetching the next page of results.
    /// </summary>
    [JsonProperty("nextCursor")]
    public BigInteger? NextCursor { get; set; }
}