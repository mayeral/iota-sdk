using System.Text.Json;
using System.Text.Json.Serialization;

namespace iota_sdk.model.@event
{
    [JsonConverter(typeof(EventFilterJsonConverter))]
    public abstract class EventFilter
    {
        // Base class for all filter types

        public static EventFilter BySender(string sender) => new SenderFilter { Sender = sender };

        public static EventFilter ByTransaction(string transaction) => new TransactionFilter { Transaction = transaction }; 

        public static EventFilter ByPackage(string package) => new PackageFilter { Package = package }; 

        public static EventFilter ByMoveModule(string module, string package) => new MoveModuleFilter { MoveModule = new MoveModuleInfo { module = module, package = package } };   

        public static EventFilter ByMoveEventType(string moveEventType) => new MoveEventTypeFilter { MoveEventType = moveEventType };   

        public static EventFilter ByMoveEventModule(string module, string package) => new MoveEventModuleFilter { MoveEventModule = new MoveModuleInfo { module = module, package = package } };    

        public static EventFilter ByMoveEventField(string path, object value) => new MoveEventFieldFilter { MoveEventField = new MoveEventFieldInfo { path = path, value = value } };   

        public static EventFilter ByTimeRange(string startTime, string endTime) => new TimeRangeFilter { TimeRange = new TimeRangeInfo { startTime = startTime, endTime = endTime } };  

        public static EventFilter All(params EventFilter[] filters) => new EventFilterAll { All = filters };   

        public static EventFilter Any(params EventFilter[] filters) => new EventFilterAny { Any = filters };   

        public static EventFilter And(params EventFilter[] filters) => new EventFilterAnd { And = filters };    

        public static EventFilter Or(params EventFilter[] filters) => new EventFilterOr { Or = filters };   
    }

    public class SenderFilter : EventFilter
    {
        [JsonPropertyName("Sender")]
        public string Sender { get; set; }
    }

    public class TransactionFilter : EventFilter
    {
        [JsonPropertyName("Transaction")]
        public string Transaction { get; set; }
    }

    public class PackageFilter : EventFilter
    {
        [JsonPropertyName("Package")]
        public string Package { get; set; }
    }

    public class MoveModuleFilter : EventFilter
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

    public class MoveEventTypeFilter : EventFilter
    {
        [JsonPropertyName("MoveEventType")]
        public string MoveEventType { get; set; }
    }

    public class MoveEventModuleFilter : EventFilter
    {
        [JsonPropertyName("MoveEventModule")]
        public MoveModuleInfo MoveEventModule { get; set; }
    }

    public class MoveEventFieldFilter : EventFilter
    {
        [JsonPropertyName("MoveEventField")]
        public MoveEventFieldInfo MoveEventField { get; set; }
    }

    public class MoveEventFieldInfo
    {
        [JsonPropertyName("path")]
        public string path { get; set; }

        [JsonPropertyName("value")]
        public object value { get; set; }
    }

    public class TimeRangeFilter : EventFilter
    {
        [JsonPropertyName("TimeRange")]
        public TimeRangeInfo TimeRange { get; set; }
    }

    public class TimeRangeInfo
    {
        [JsonPropertyName("startTime")]
        public string startTime { get; set; }

        [JsonPropertyName("endTime")]
        public string endTime { get; set; }
    }

    public class EventFilterAll : EventFilter
    {
        [JsonPropertyName("All")]
        public EventFilter[] All { get; set; }
    }

    public class EventFilterAny : EventFilter
    {
        [JsonPropertyName("Any")]
        public EventFilter[] Any { get; set; }
    }

    public class EventFilterAnd : EventFilter
    {
        [JsonPropertyName("And")]
        public EventFilter[] And { get; set; }
    }

    public class EventFilterOr : EventFilter
    {
        [JsonPropertyName("Or")]
        public EventFilter[] Or { get; set; }
    }

    public class EventFilterJsonConverter : JsonConverter<EventFilter>
    {
        public override EventFilter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected start of object");
            }

            using (JsonDocument document = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = document.RootElement;

                // Check which property exists to determine the filter type
                if (root.TryGetProperty("Sender", out JsonElement senderElement))
                {
                    return new SenderFilter { Sender = senderElement.GetString() };
                }
                else if (root.TryGetProperty("Transaction", out JsonElement txElement))
                {
                    return new TransactionFilter { Transaction = txElement.GetString() };
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
                else if (root.TryGetProperty("MoveEventType", out JsonElement moveEventTypeElement))
                {
                    return new MoveEventTypeFilter { MoveEventType = moveEventTypeElement.GetString() };
                }
                else if (root.TryGetProperty("MoveEventModule", out JsonElement moveEventModuleElement))
                {
                    return new MoveEventModuleFilter
                    {
                        MoveEventModule = new MoveModuleInfo
                        {
                            module = moveEventModuleElement.GetProperty("module").GetString(),
                            package = moveEventModuleElement.GetProperty("package").GetString()
                        }
                    };
                }
                else if (root.TryGetProperty("MoveEventField", out JsonElement moveEventFieldElement))
                {
                    return new MoveEventFieldFilter
                    {
                        MoveEventField = new MoveEventFieldInfo
                        {
                            path = moveEventFieldElement.GetProperty("path").GetString(),
                            value = JsonSerializer.Deserialize<object>(moveEventFieldElement.GetProperty("value").GetRawText(), options)
                        }
                    };
                }
                else if (root.TryGetProperty("TimeRange", out JsonElement timeRangeElement))
                {
                    return new TimeRangeFilter
                    {
                        TimeRange = new TimeRangeInfo
                        {
                            startTime = timeRangeElement.GetProperty("startTime").GetString(),
                            endTime = timeRangeElement.GetProperty("endTime").GetString()
                        }
                    };
                }
                else if (root.TryGetProperty("All", out JsonElement allElement))
                {
                    return new EventFilterAll
                    {
                        All = JsonSerializer.Deserialize<EventFilter[]>(allElement.GetRawText(), options)
                    };
                }
                else if (root.TryGetProperty("Any", out JsonElement anyElement))
                {
                    return new EventFilterAny
                    {
                        Any = JsonSerializer.Deserialize<EventFilter[]>(anyElement.GetRawText(), options)
                    };
                }
                else if (root.TryGetProperty("And", out JsonElement andElement))
                {
                    return new EventFilterAnd
                    {
                        And = JsonSerializer.Deserialize<EventFilter[]>(andElement.GetRawText(), options)
                    };
                }
                else if (root.TryGetProperty("Or", out JsonElement orElement))
                {
                    return new EventFilterOr
                    {
                        Or = JsonSerializer.Deserialize<EventFilter[]>(orElement.GetRawText(), options)
                    };
                }

                throw new JsonException("Unknown filter type");
            }
        }

        public override void Write(Utf8JsonWriter writer, EventFilter value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            switch (value)
            {
                case SenderFilter senderFilter:
                    writer.WriteString("Sender", senderFilter.Sender);
                    break;
                case TransactionFilter txFilter:
                    writer.WriteString("Transaction", txFilter.Transaction);
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
                case MoveEventTypeFilter moveEventTypeFilter:
                    writer.WriteString("MoveEventType", moveEventTypeFilter.MoveEventType);
                    break;
                case MoveEventModuleFilter moveEventModuleFilter:
                    writer.WritePropertyName("MoveEventModule");
                    writer.WriteStartObject();
                    writer.WriteString("module", moveEventModuleFilter.MoveEventModule.module);
                    writer.WriteString("package", moveEventModuleFilter.MoveEventModule.package);
                    writer.WriteEndObject();
                    break;
                case MoveEventFieldFilter moveEventFieldFilter:
                    writer.WritePropertyName("MoveEventField");
                    writer.WriteStartObject();
                    writer.WriteString("path", moveEventFieldFilter.MoveEventField.path);
                    writer.WritePropertyName("value");
                    JsonSerializer.Serialize(writer, moveEventFieldFilter.MoveEventField.value, options);
                    writer.WriteEndObject();
                    break;
                case TimeRangeFilter timeRangeFilter:
                    writer.WritePropertyName("TimeRange");
                    writer.WriteStartObject();
                    writer.WriteString("startTime", timeRangeFilter.TimeRange.startTime);
                    writer.WriteString("endTime", timeRangeFilter.TimeRange.endTime);
                    writer.WriteEndObject();
                    break;
                case EventFilterAll allFilter:
                    writer.WritePropertyName("All");
                    JsonSerializer.Serialize(writer, allFilter.All, options);
                    break;
                case EventFilterAny anyFilter:
                    writer.WritePropertyName("Any");
                    JsonSerializer.Serialize(writer, anyFilter.Any, options);
                    break;
                case EventFilterAnd andFilter:
                    writer.WritePropertyName("And");
                    JsonSerializer.Serialize(writer, andFilter.And, options);
                    break;
                case EventFilterOr orFilter:
                    writer.WritePropertyName("Or");
                    JsonSerializer.Serialize(writer, orFilter.Or, options);
                    break;
                default:
                    throw new JsonException($"Unknown filter type: {value.GetType().Name}");
            }

            writer.WriteEndObject();
        }
    }
}