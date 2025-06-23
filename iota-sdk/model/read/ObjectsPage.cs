using System.Text.Json.Serialization;
using Iota.Sdk.Model.Read;
using iota_sdk.model.read;
using Newtonsoft.Json;

/// <summary>
/// Represents a page of IOTA objects.
/// </summary>
/// <remarks>
/// next_cursor points to the last item in the page; Reading with next_cursor will start from 
/// the next item after next_cursor if next_cursor is Some, otherwise it will start from the first item.
/// </remarks>
public class ObjectsPage
{
    /// <summary>
    /// Gets or sets the list of IOTA object responses in this page.
    /// </summary>
    [JsonProperty("data")]
    [JsonPropertyName("data")]
    public List<IotaObjectResponse> Data { get; set; } = new List<IotaObjectResponse>();

    /// <summary>
    /// Gets or sets a value indicating whether there are more pages available.
    /// </summary>
    [JsonProperty("hasNextPage")]
    [JsonPropertyName("hasNextPage")]
    public bool HasNextPage { get; set; }

    /// <summary>
    /// Gets or sets the cursor to the next page. This points to the last item in the current page.
    /// When used in a subsequent request, the response will start from the next item after this cursor.
    /// </summary>
    [JsonProperty("nextCursor")]
    [JsonPropertyName("nextCursor")]
    public string? NextCursor { get; set; }
}