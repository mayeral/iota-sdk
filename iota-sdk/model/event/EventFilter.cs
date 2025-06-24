using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        [JsonProperty("Sender")]
        public string Sender { get; set; }
    }

    public class TransactionFilter : EventFilter
    {
        [JsonProperty("Transaction")]
        public string Transaction { get; set; }
    }

    public class PackageFilter : EventFilter
    {
        [JsonProperty("Package")]
        public string Package { get; set; }
    }

    public class MoveModuleFilter : EventFilter
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

    public class MoveEventTypeFilter : EventFilter
    {
        [JsonProperty("MoveEventType")]
        public string MoveEventType { get; set; }
    }

    public class MoveEventModuleFilter : EventFilter
    {
        [JsonProperty("MoveEventModule")]
        public MoveModuleInfo MoveEventModule { get; set; }
    }

    public class MoveEventFieldFilter : EventFilter
    {
        [JsonProperty("MoveEventField")]
        public MoveEventFieldInfo MoveEventField { get; set; }
    }

    public class MoveEventFieldInfo
    {
        [JsonProperty("path")]
        public string path { get; set; }

        [JsonProperty("value")]
        public object value { get; set; }
    }

    public class TimeRangeFilter : EventFilter
    {
        [JsonProperty("TimeRange")]
        public TimeRangeInfo TimeRange { get; set; }
    }

    public class TimeRangeInfo
    {
        [JsonProperty("startTime")]
        public string startTime { get; set; }

        [JsonProperty("endTime")]
        public string endTime { get; set; }
    }

    public class EventFilterAll : EventFilter
    {
        [JsonProperty("All")]
        public EventFilter[] All { get; set; }
    }

    public class EventFilterAny : EventFilter
    {
        [JsonProperty("Any")]
        public EventFilter[] Any { get; set; }
    }

    public class EventFilterAnd : EventFilter
    {
        [JsonProperty("And")]
        public EventFilter[] And { get; set; }
    }

    public class EventFilterOr : EventFilter
    {
        [JsonProperty("Or")]
        public EventFilter[] Or { get; set; }
    }

    public class EventFilterJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(EventFilter).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartObject)
            {
                throw new JsonException("Expected start of object");
            }

            JObject jsonObject = JObject.Load(reader);

            // Check which property exists to determine the filter type
            if (jsonObject["Sender"] != null)
            {
                return new SenderFilter { Sender = jsonObject["Sender"].ToString() };
            }
            else if (jsonObject["Transaction"] != null)
            {
                return new TransactionFilter { Transaction = jsonObject["Transaction"].ToString() };
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
            else if (jsonObject["MoveEventType"] != null)
            {
                return new MoveEventTypeFilter { MoveEventType = jsonObject["MoveEventType"].ToString() };
            }
            else if (jsonObject["MoveEventModule"] != null)
            {
                return new MoveEventModuleFilter
                {
                    MoveEventModule = new MoveModuleInfo
                    {
                        module = jsonObject["MoveEventModule"]["module"].ToString(),
                        package = jsonObject["MoveEventModule"]["package"].ToString()
                    }
                };
            }
            else if (jsonObject["MoveEventField"] != null)
            {
                return new MoveEventFieldFilter
                {
                    MoveEventField = new MoveEventFieldInfo
                    {
                        path = jsonObject["MoveEventField"]["path"].ToString(),
                        value = jsonObject["MoveEventField"]["value"].ToObject<object>()
                    }
                };
            }
            else if (jsonObject["TimeRange"] != null)
            {
                return new TimeRangeFilter
                {
                    TimeRange = new TimeRangeInfo
                    {
                        startTime = jsonObject["TimeRange"]["startTime"].ToString(),
                        endTime = jsonObject["TimeRange"]["endTime"].ToString()
                    }
                };
            }
            else if (jsonObject["All"] != null)
            {
                return new EventFilterAll { All = jsonObject["All"].ToObject<EventFilter[]>(serializer) };
            }
            else if (jsonObject["Any"] != null)
            {
                return new EventFilterAny { Any = jsonObject["Any"].ToObject<EventFilter[]>(serializer) };
            }
            else if (jsonObject["And"] != null)
            {
                return new EventFilterAnd { And = jsonObject["And"].ToObject<EventFilter[]>(serializer) };
            }
            else if (jsonObject["Or"] != null)
            {
                return new EventFilterOr { Or = jsonObject["Or"].ToObject<EventFilter[]>(serializer) };
            }

            throw new JsonException("Unknown filter type");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            EventFilter filter = (EventFilter)value;
            JObject jsonObject = new JObject();

            switch (filter)
            {
                case SenderFilter senderFilter:
                    jsonObject["Sender"] = senderFilter.Sender;
                    break;
                case TransactionFilter txFilter:
                    jsonObject["Transaction"] = txFilter.Transaction;
                    break;
                case PackageFilter packageFilter:
                    jsonObject["Package"] = packageFilter.Package;
                    break;
                case MoveModuleFilter moveModuleFilter:
                    jsonObject["MoveModule"] = JObject.FromObject(moveModuleFilter.MoveModule);
                    break;
                case MoveEventTypeFilter moveEventTypeFilter:
                    jsonObject["MoveEventType"] = moveEventTypeFilter.MoveEventType;
                    break;
                case MoveEventModuleFilter moveEventModuleFilter:
                    jsonObject["MoveEventModule"] = JObject.FromObject(moveEventModuleFilter.MoveEventModule);
                    break;
                case MoveEventFieldFilter moveEventFieldFilter:
                    jsonObject["MoveEventField"] = new JObject
                    {
                        ["path"] = moveEventFieldFilter.MoveEventField.path,
                        ["value"] = JToken.FromObject(moveEventFieldFilter.MoveEventField.value, serializer)
                    };
                    break;
                case TimeRangeFilter timeRangeFilter:
                    jsonObject["TimeRange"] = new JObject
                    {
                        ["startTime"] = timeRangeFilter.TimeRange.startTime,
                        ["endTime"] = timeRangeFilter.TimeRange.endTime
                    };
                    break;
                case EventFilterAll allFilter:
                    jsonObject["All"] = JArray.FromObject(allFilter.All, serializer);
                    break;
                case EventFilterAny anyFilter:
                    jsonObject["Any"] = JArray.FromObject(anyFilter.Any, serializer);
                    break;
                case EventFilterAnd andFilter:
                    jsonObject["And"] = JArray.FromObject(andFilter.And, serializer);
                    break;
                case EventFilterOr orFilter:
                    jsonObject["Or"] = JArray.FromObject(orFilter.Or, serializer);
                    break;
            }

            jsonObject.WriteTo(writer);
        }
    }
}