using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iota_sdk.model.read.@object;

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
	public IotaObjectDataFilter? Filter { get; set; }

	/// <summary>
	/// Gets or sets the options that configure which fields to include in the response.
	/// By default, only digest is included.
	/// </summary>
	[JsonProperty("options")]
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
[JsonConverter(typeof(IotaObjectDataFilterConverter))]
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
	[JsonProperty("MatchAll")]
	public IotaObjectDataFilter[] MatchAll { get; set; }
}

public class MatchAnyFilter : IotaObjectDataFilter
{
	[JsonProperty("MatchAny")]
	public IotaObjectDataFilter[] MatchAny { get; set; }
}

public class MatchNoneFilter : IotaObjectDataFilter
{
	[JsonProperty("MatchNone")]
	public IotaObjectDataFilter[] MatchNone { get; set; }
}

public class PackageFilter : IotaObjectDataFilter
{
	[JsonProperty("Package")]
	public string Package { get; set; }
}

public class MoveModuleFilter : IotaObjectDataFilter
{
	[JsonProperty("MoveModule")]
	public MoveModuleInfo MoveModule { get; set; }
}

public class MoveModuleInfo
{
	[JsonProperty("module")]
	public string module { get; set; }

	[JsonProperty("package")]
	public string package { get; set; }
}

public class StructTypeFilter : IotaObjectDataFilter
{
	[JsonProperty("StructType")]
	public string StructType { get; set; }
}

public class AddressOwnerFilter : IotaObjectDataFilter
{
	[JsonProperty("AddressOwner")]
	public string AddressOwner { get; set; }
}

public class ObjectOwnerFilter : IotaObjectDataFilter
{
	[JsonProperty("ObjectOwner")]
	public string ObjectOwner { get; set; }
}

public class ObjectIdFilter : IotaObjectDataFilter
{
	[JsonProperty("ObjectId")]
	public string ObjectId { get; set; }
}

public class ObjectIdsFilter : IotaObjectDataFilter
{
	[JsonProperty("ObjectIds")]
	public string[] ObjectIds { get; set; }
}

public class VersionFilter : IotaObjectDataFilter
{
	[JsonProperty("Version")]
	public string Version { get; set; }
}

/// <summary>
/// Newtonsoft.Json converter for IotaObjectDataFilter.
/// </summary>
public class IotaObjectDataFilterConverter : JsonConverter
{
	public override bool CanConvert(Type objectType)
	{
		return typeof(IotaObjectDataFilter).IsAssignableFrom(objectType);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		if (reader.TokenType != JsonToken.StartObject)
		{
			throw new JsonSerializationException("Expected start of object");
		}

		JObject jsonObject = JObject.Load(reader);

		// Check for each filter type
		if (jsonObject["MatchAll"] != null)
		{
			return new MatchAllFilter { MatchAll = jsonObject["MatchAll"].ToObject<IotaObjectDataFilter[]>(serializer) };
		}
		else if (jsonObject["MatchAny"] != null)
		{
			return new MatchAnyFilter { MatchAny = jsonObject["MatchAny"].ToObject<IotaObjectDataFilter[]>(serializer) };
		}
		else if (jsonObject["MatchNone"] != null)
		{
			return new MatchNoneFilter { MatchNone = jsonObject["MatchNone"].ToObject<IotaObjectDataFilter[]>(serializer) };
		}
		else if (jsonObject["Package"] != null)
		{
			return new PackageFilter { Package = jsonObject["Package"].ToString() };
		}
		else if (jsonObject["MoveModule"] != null)
		{
			return new MoveModuleFilter
			{
				MoveModule = new MoveModuleInfo
				{
					module = jsonObject["MoveModule"]["module"].ToString(),
					package = jsonObject["MoveModule"]["package"].ToString()
				}
			};
		}
		else if (jsonObject["StructType"] != null)
		{
			return new StructTypeFilter { StructType = jsonObject["StructType"].ToString() };
		}
		else if (jsonObject["AddressOwner"] != null)
		{
			return new AddressOwnerFilter { AddressOwner = jsonObject["AddressOwner"].ToString() };
		}
		else if (jsonObject["ObjectOwner"] != null)
		{
			return new ObjectOwnerFilter { ObjectOwner = jsonObject["ObjectOwner"].ToString() };
		}
		else if (jsonObject["ObjectId"] != null)
		{
			return new ObjectIdFilter { ObjectId = jsonObject["ObjectId"].ToString() };
		}
		else if (jsonObject["ObjectIds"] != null)
		{
			return new ObjectIdsFilter { ObjectIds = jsonObject["ObjectIds"].ToObject<string[]>(serializer) };
		}
		else if (jsonObject["Version"] != null)
		{
			return new VersionFilter { Version = jsonObject["Version"].ToString() };
		}

		throw new JsonSerializationException("Unknown filter type");
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		IotaObjectDataFilter filter = (IotaObjectDataFilter)value;

		writer.WriteStartObject();

		switch (filter)
		{
			case MatchAllFilter matchAllFilter:
				writer.WritePropertyName("MatchAll");
				serializer.Serialize(writer, matchAllFilter.MatchAll);
				break;
			case MatchAnyFilter matchAnyFilter:
				writer.WritePropertyName("MatchAny");
				serializer.Serialize(writer, matchAnyFilter.MatchAny);
				break;
			case MatchNoneFilter matchNoneFilter:
				writer.WritePropertyName("MatchNone");
				serializer.Serialize(writer, matchNoneFilter.MatchNone);
				break;
			case PackageFilter packageFilter:
				writer.WritePropertyName("Package");
				writer.WriteValue(packageFilter.Package);
				break;
			case MoveModuleFilter moveModuleFilter:
				writer.WritePropertyName("MoveModule");
				writer.WriteStartObject();
				writer.WritePropertyName("module");
				writer.WriteValue(moveModuleFilter.MoveModule.module);
				writer.WritePropertyName("package");
				writer.WriteValue(moveModuleFilter.MoveModule.package);
				writer.WriteEndObject();
				break;
			case StructTypeFilter structTypeFilter:
				writer.WritePropertyName("StructType");
				writer.WriteValue(structTypeFilter.StructType);
				break;
			case AddressOwnerFilter addressOwnerFilter:
				writer.WritePropertyName("AddressOwner");
				writer.WriteValue(addressOwnerFilter.AddressOwner);
				break;
			case ObjectOwnerFilter objectOwnerFilter:
				writer.WritePropertyName("ObjectOwner");
				writer.WriteValue(objectOwnerFilter.ObjectOwner);
				break;
			case ObjectIdFilter objectIdFilter:
				writer.WritePropertyName("ObjectId");
				writer.WriteValue(objectIdFilter.ObjectId);
				break;
			case ObjectIdsFilter objectIdsFilter:
				writer.WritePropertyName("ObjectIds");
				serializer.Serialize(writer, objectIdsFilter.ObjectIds);
				break;
			case VersionFilter versionFilter:
				writer.WritePropertyName("Version");
				writer.WriteValue(versionFilter.Version);
				break;
			default:
				throw new JsonSerializationException($"Unknown filter type: {filter.GetType().Name}");
		}

		writer.WriteEndObject();
	}
}
