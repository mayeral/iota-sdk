using Newtonsoft.Json;

namespace iota_sdk.model.coin;

/// <summary>
/// Represents a paginated result of Coin objects.
/// </summary>
public class CoinPage
{
    /// <summary>
    /// List of coins in the current page.
    /// </summary>
    [JsonProperty("data")]
    public List<Coin> Data { get; set; }

    /// <summary>
    /// Indicates if there are more pages available.
    /// </summary>
    [JsonProperty("hasNextPage")]
    public bool HasNextPage { get; set; }

    /// <summary>
    /// Cursor pointing to the last item in the page.
    /// Reading with this cursor will start from the next item after it.
    /// </summary>
    [JsonProperty("nextCursor")]
    public string NextCursor { get; set; }
}