using System.Text.Json;
using System.Text.Json.Serialization;
using Iota.Sdk.Model.Read;
using Newtonsoft.Json;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Iota.Sdk.Model.Read;

/// <summary>
/// Represents a query for IOTA object responses.
/// </summary>
public class IotaObjectResponseQuery
{
    /// <summary>
    /// Gets or sets the filter to apply to the query.
    /// If null, no filter will be applied.
    /// </summary>
    [JsonProperty("filter")]
    [JsonPropertyName("filter")]
    public IotaObjectDataFilter? Filter { get; set; }

    /// <summary>
    /// Gets or sets the options that configure which fields to include in the response.
    /// By default, only digest is included.
    /// </summary>
    [JsonProperty("options")]
    [JsonPropertyName("options")]
    public IotaObjectDataOptions? Options { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="IotaObjectResponseQuery"/> class.
    /// </summary>
    public IotaObjectResponseQuery()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IotaObjectResponseQuery"/> class with the specified filter and options.
    /// </summary>
    /// <param name="filter">The filter to apply to the query.</param>
    /// <param name="options">The options that configure which fields to include in the response.</param>
    public IotaObjectResponseQuery(IotaObjectDataFilter? filter, IotaObjectDataOptions? options)
    {
        Filter = filter;
        Options = options;
    }
}

/// <summary>
/// Represents a filter for IOTA object data.
/// </summary>
[System.Text.Json.Serialization.JsonConverter(typeof(IotaObjectDataFilterSystemConverter))]
public abstract class IotaObjectDataFilter
{
    /// <summary>
    /// Creates a filter that matches all of the provided filters.
    /// </summary>
    /// <param name="filters">The filters to match.</param>
    /// <returns>A filter that matches all of the provided filters.</returns>
    public static IotaObjectDataFilter MatchAll(params IotaObjectDataFilter[] filters) => new MatchAllFilter { MatchAll = filters };

    /// <summary>
    /// Creates a filter that matches any of the provided filters.
    /// </summary>
    /// <param name="filters">The filters to match.</param>
    /// <returns>A filter that matches any of the provided filters.</returns>
    public static IotaObjectDataFilter MatchAny(params IotaObjectDataFilter[] filters) => new MatchAnyFilter { MatchAny = filters };

    /// <summary>
    /// Creates a filter that matches none of the provided filters.
    /// </summary>
    /// <param name="filters">The filters to not match.</param>
    /// <returns>A filter that matches none of the provided filters.</returns>
    public static IotaObjectDataFilter MatchNone(params IotaObjectDataFilter[] filters) => new MatchNoneFilter { MatchNone = filters };

    /// <summary>
    /// Creates a filter that matches objects with the specified package.
    /// </summary>
    /// <param name="package">The package to match.</param>
    /// <returns>A filter that matches objects with the specified package.</returns>
    public static IotaObjectDataFilter Package(string package) => new PackageFilter { Package = package };

    /// <summary>
    /// Creates a filter that matches objects with the specified Move module.
    /// </summary>
    /// <param name="moduleName">The module name.</param>
    /// <param name="packageId">The Move package ID.</param>
    /// <returns>A filter that matches objects with the specified Move module.</returns>
    public static IotaObjectDataFilter MoveModule(string moduleName, string packageId) =>
        new MoveModuleFilter { MoveModule = new MoveModuleInfo { module = moduleName, package = packageId } };

    /// <summary>
    /// Creates a filter that matches objects with the specified struct type.
    /// </summary>
    /// <param name="structType">The struct type to match.</param>
    /// <returns>A filter that matches objects with the specified struct type.</returns>
    public static IotaObjectDataFilter StructType(string structType) => new StructTypeFilter { StructType = structType };

    /// <summary>
    /// Creates a filter that matches objects with the specified address owner.
    /// </summary>
    /// <param name="address">The address to match.</param>
    /// <returns>A filter that matches objects with the specified address owner.</returns>
    public static IotaObjectDataFilter AddressOwner(string address) => new AddressOwnerFilter { AddressOwner = address };

    /// <summary>
    /// Creates a filter that matches objects with the specified object owner.
    /// </summary>
    /// <param name="owner">The object owner to match.</param>
    /// <returns>A filter that matches objects with the specified object owner.</returns>
    public static IotaObjectDataFilter ObjectOwner(string owner) => new ObjectOwnerFilter { ObjectOwner = owner };

    /// <summary>
    /// Creates a filter that matches objects with the specified object ID.
    /// </summary>
    /// <param name="objectId">The object ID to match.</param>
    /// <returns>A filter that matches objects with the specified object ID.</returns>
    public static IotaObjectDataFilter ObjectId(string objectId) => new ObjectIdFilter { ObjectId = objectId };

    /// <summary>
    /// Creates a filter that matches objects with any of the specified object IDs.
    /// </summary>
    /// <param name="objectIds">The object IDs to match.</param>
    /// <returns>A filter that matches objects with any of the specified object IDs.</returns>
    public static IotaObjectDataFilter ObjectIds(string[] objectIds) => new ObjectIdsFilter { ObjectIds = objectIds };

    /// <summary>
    /// Creates a filter that matches objects with the specified version.
    /// </summary>
    /// <param name="version">The version to match.</param>
    /// <returns>A filter that matches objects with the specified version.</returns>
    public static IotaObjectDataFilter Version(string version) => new VersionFilter { Version = version };
}

// Filter classes for different filter types
public class MatchAllFilter : IotaObjectDataFilter
{
    [JsonPropertyName("MatchAll")]
    public IotaObjectDataFilter[] MatchAll { get; set; }
}

public class MatchAnyFilter : IotaObjectDataFilter
{
    [JsonPropertyName("MatchAny")]
    public IotaObjectDataFilter[] MatchAny { get; set; }
}

public class MatchNoneFilter : IotaObjectDataFilter
{
    [JsonPropertyName("MatchNone")]
    public IotaObjectDataFilter[] MatchNone { get; set; }
}

public class PackageFilter : IotaObjectDataFilter
{
    [JsonPropertyName("Package")]
    public string Package { get; set; }
}

public class MoveModuleFilter : IotaObjectDataFilter
{
    [JsonPropertyName("MoveModule")]
    public MoveModuleInfo MoveModule { get; set; }
}

public class MoveModuleInfo
{
    [JsonPropertyName("module")]
    public string module { get; set; }
    
    [JsonPropertyName("package")]
    public string package { get; set; }
}

public class StructTypeFilter : IotaObjectDataFilter
{
    [JsonPropertyName("StructType")]
    public string StructType { get; set; }
}

public class AddressOwnerFilter : IotaObjectDataFilter
{
    [JsonPropertyName("AddressOwner")]
    public string AddressOwner { get; set; }
}

public class ObjectOwnerFilter : IotaObjectDataFilter
{
    [JsonPropertyName("ObjectOwner")]
    public string ObjectOwner { get; set; }
}

public class ObjectIdFilter : IotaObjectDataFilter
{
    [JsonPropertyName("ObjectId")]
    public string ObjectId { get; set; }
}

public class ObjectIdsFilter : IotaObjectDataFilter
{
    [JsonPropertyName("ObjectIds")]
    public string[] ObjectIds { get; set; }
}

public class VersionFilter : IotaObjectDataFilter
{
    [JsonPropertyName("Version")]
    public string Version { get; set; }
}
/// <summary>
/// System.Text.Json converter for IotaObjectDataFilter.
/// </summary>
public class IotaObjectDataFilterSystemConverter : System.Text.Json.Serialization.JsonConverter<IotaObjectDataFilter>
{
    public override IotaObjectDataFilter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        using (JsonDocument document = JsonDocument.ParseValue(ref reader))
        {
            JsonElement root = document.RootElement;

            // Check which property exists to determine the filter type
            if (root.TryGetProperty("MatchAll", out JsonElement matchAllElement))
            {
                return new MatchAllFilter { MatchAll = JsonSerializer.Deserialize<IotaObjectDataFilter[]>(matchAllElement.GetRawText(), options) };
            }
            else if (root.TryGetProperty("MatchAny", out JsonElement matchAnyElement))
            {
                return new MatchAnyFilter { MatchAny = JsonSerializer.Deserialize<IotaObjectDataFilter[]>(matchAnyElement.GetRawText(), options) };
            }
            else if (root.TryGetProperty("MatchNone", out JsonElement matchNoneElement))
            {
                return new MatchNoneFilter { MatchNone = JsonSerializer.Deserialize<IotaObjectDataFilter[]>(matchNoneElement.GetRawText(), options) };
            }
            else if (root.TryGetProperty("Package", out JsonElement packageElement))
            {
                return new PackageFilter { Package = packageElement.GetString() };
            }
            else if (root.TryGetProperty("MoveModule", out JsonElement moveModuleElement))
            {
                return new MoveModuleFilter
                {
                    MoveModule = new MoveModuleInfo
                    {
                        module = moveModuleElement.GetProperty("module").GetString(),
                        package = moveModuleElement.GetProperty("package").GetString()
                    }
                };
            }
            else if (root.TryGetProperty("StructType", out JsonElement structTypeElement))
            {
                return new StructTypeFilter { StructType = structTypeElement.GetString() };
            }
            else if (root.TryGetProperty("AddressOwner", out JsonElement addressOwnerElement))
            {
                return new AddressOwnerFilter { AddressOwner = addressOwnerElement.GetString() };
            }
            else if (root.TryGetProperty("ObjectOwner", out JsonElement objectOwnerElement))
            {
                return new ObjectOwnerFilter { ObjectOwner = objectOwnerElement.GetString() };
            }
            else if (root.TryGetProperty("ObjectId", out JsonElement objectIdElement))
            {
                return new ObjectIdFilter { ObjectId = objectIdElement.GetString() };
            }
            else if (root.TryGetProperty("ObjectIds", out JsonElement objectIdsElement))
            {
                return new ObjectIdsFilter { ObjectIds = JsonSerializer.Deserialize<string[]>(objectIdsElement.GetRawText(), options) };
            }
            else if (root.TryGetProperty("Version", out JsonElement versionElement))
            {
                return new VersionFilter { Version = versionElement.GetString() };
            }

            throw new JsonException("Unknown IotaObjectDataFilter type");
        }
    }

    public override void Write(Utf8JsonWriter writer, IotaObjectDataFilter value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        switch (value)
        {
            case MatchAllFilter matchAllFilter:
                writer.WritePropertyName("MatchAll");
                JsonSerializer.Serialize(writer, matchAllFilter.MatchAll, options);
                break;
            case MatchAnyFilter matchAnyFilter:
                writer.WritePropertyName("MatchAny");
                JsonSerializer.Serialize(writer, matchAnyFilter.MatchAny, options);
                break;
            case MatchNoneFilter matchNoneFilter:
                writer.WritePropertyName("MatchNone");
                JsonSerializer.Serialize(writer, matchNoneFilter.MatchNone, options);
                break;
            case PackageFilter packageFilter:
                writer.WriteString("Package", packageFilter.Package);
                break;
            case MoveModuleFilter moveModuleFilter:
                writer.WritePropertyName("MoveModule");
                writer.WriteStartObject();
                writer.WriteString("module", moveModuleFilter.MoveModule.module);
                writer.WriteString("package", moveModuleFilter.MoveModule.package);
                writer.WriteEndObject();
                break;
            case StructTypeFilter structTypeFilter:
                writer.WriteString("StructType", structTypeFilter.StructType);
                break;
            case AddressOwnerFilter addressOwnerFilter:
                writer.WriteString("AddressOwner", addressOwnerFilter.AddressOwner);
                break;
            case ObjectOwnerFilter objectOwnerFilter:
                writer.WriteString("ObjectOwner", objectOwnerFilter.ObjectOwner);
                break;
            case ObjectIdFilter objectIdFilter:
                writer.WriteString("ObjectId", objectIdFilter.ObjectId);
                break;
            case ObjectIdsFilter objectIdsFilter:
                writer.WritePropertyName("ObjectIds");
                JsonSerializer.Serialize(writer, objectIdsFilter.ObjectIds, options);
                break;
            case VersionFilter versionFilter:
                writer.WriteString("Version", versionFilter.Version);
                break;
            default:
                throw new JsonException($"Unknown filter type: {value.GetType().Name}");
        }

        writer.WriteEndObject();
    }
}