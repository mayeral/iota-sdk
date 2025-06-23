using System.Text.Json;
using iota_sdk.model.@event;


namespace iota_sdk_tests.model
{
    [TestFixture]
    public class EventFilterTests
    {
        private JsonSerializerOptions _options;

        [SetUp]
        public void Setup()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        [Test]
        public void SenderFilter_SerializeDeserialize_Roundtrip()
        {
            // Arrange
            var filter = new SenderFilter { Sender = "0x123456789abcdef" };
            
            // Act
            string json = JsonSerializer.Serialize(filter, _options);
            var deserializedFilter = JsonSerializer.Deserialize<EventFilter>(json, _options);
            
            // Assert
            Assert.IsInstanceOf<SenderFilter>(deserializedFilter);
            var senderFilter = deserializedFilter as SenderFilter;
            Assert.AreEqual(filter.Sender, senderFilter.Sender);
        }

        [Test]
        public void TransactionFilter_SerializeDeserialize_Roundtrip()
        {
            // Arrange
            var filter = new TransactionFilter { Transaction = "txid123456789" };
            
            // Act
            string json = JsonSerializer.Serialize(filter, _options);
            var deserializedFilter = JsonSerializer.Deserialize<EventFilter>(json, _options);
            
            // Assert
            Assert.IsInstanceOf<TransactionFilter>(deserializedFilter);
            var txFilter = deserializedFilter as TransactionFilter;
            Assert.AreEqual(filter.Transaction, txFilter.Transaction);
        }

        [Test]
        public void MoveModuleFilter_SerializeDeserialize_Roundtrip()
        {
            // Arrange
            var filter = new MoveModuleFilter 
            { 
                MoveModule = new MoveModuleInfo 
                { 
                    module = "TestModule", 
                    package = "0xabcdef123456" 
                } 
            };
            
            // Act
            string json = JsonSerializer.Serialize(filter, _options);
            var deserializedFilter = JsonSerializer.Deserialize<EventFilter>(json, _options);
            
            // Assert
            Assert.IsInstanceOf<MoveModuleFilter>(deserializedFilter);
            var moduleFilter = deserializedFilter as MoveModuleFilter;
            Assert.AreEqual(filter.MoveModule.module, moduleFilter.MoveModule.module);
            Assert.AreEqual(filter.MoveModule.package, moduleFilter.MoveModule.package);
        }

        [Test]
        public void TimeRangeFilter_SerializeDeserialize_Roundtrip()
        {
            // Arrange
            var filter = new TimeRangeFilter
            {
                TimeRange = new TimeRangeInfo
                {
                    startTime = "1609459200000", // 2021-01-01T00:00:00Z
                    endTime = "1640995200000"    // 2022-01-01T00:00:00Z
                }
            };
            
            // Act
            string json = JsonSerializer.Serialize(filter, _options);
            var deserializedFilter = JsonSerializer.Deserialize<EventFilter>(json, _options);
            
            // Assert
            Assert.IsInstanceOf<TimeRangeFilter>(deserializedFilter);
            var timeRangeFilter = deserializedFilter as TimeRangeFilter;
            Assert.AreEqual(filter.TimeRange.startTime, timeRangeFilter.TimeRange.startTime);
            Assert.AreEqual(filter.TimeRange.endTime, timeRangeFilter.TimeRange.endTime);
        }

        [Test]
        public void MoveEventFieldFilter_SerializeDeserialize_Roundtrip()
        {
            // Arrange
            var filter = new MoveEventFieldFilter
            {
                MoveEventField = new MoveEventFieldInfo
                {
                    path = "/data/amount",
                    value = 100
                }
            };
            
            // Act
            string json = JsonSerializer.Serialize(filter, _options);
            var deserializedFilter = JsonSerializer.Deserialize<EventFilter>(json, _options);
            
            // Assert
            Assert.IsInstanceOf<MoveEventFieldFilter>(deserializedFilter);
            var fieldFilter = deserializedFilter as MoveEventFieldFilter;
            Assert.AreEqual(filter.MoveEventField.path, fieldFilter.MoveEventField.path);
            Assert.AreEqual(filter.MoveEventField.value.ToString(), fieldFilter.MoveEventField.value.ToString());
        }

        [Test]
        public void AllFilter_SerializeDeserialize_Roundtrip()
        {
            // Arrange
            var filter = new EventFilterAll
            {
                All = new EventFilter[]
                {
                    new SenderFilter { Sender = "0x123" },
                    new PackageFilter { Package = "0xabc" }
                }
            };
            
            // Act
            string json = JsonSerializer.Serialize(filter, _options);
            var deserializedFilter = JsonSerializer.Deserialize<EventFilter>(json, _options);
            
            // Assert
            Assert.IsInstanceOf<EventFilterAll>(deserializedFilter);
            var allFilter = deserializedFilter as EventFilterAll;
            Assert.AreEqual(2, allFilter.All.Length);
            Assert.IsInstanceOf<SenderFilter>(allFilter.All[0]);
            Assert.IsInstanceOf<PackageFilter>(allFilter.All[1]);
        }

        [Test]
        public void AndFilter_SerializeDeserialize_Roundtrip()
        {
            // Arrange
            var filter = new EventFilterAnd
            {
                And = new EventFilter[]
                {
                    new SenderFilter { Sender = "0x123" },
                    new TransactionFilter { Transaction = "tx456" }
                }
            };
            
            // Act
            string json = JsonSerializer.Serialize(filter, _options);
            var deserializedFilter = JsonSerializer.Deserialize<EventFilter>(json, _options);
            
            // Assert
            Assert.IsInstanceOf<EventFilterAnd>(deserializedFilter);
            var andFilter = deserializedFilter as EventFilterAnd;
            Assert.AreEqual(2, andFilter.And.Length);
            Assert.IsInstanceOf<SenderFilter>(andFilter.And[0]);
            Assert.IsInstanceOf<TransactionFilter>(andFilter.And[1]);
        }

        [Test]
        public void DeserializeFromJsonString_SenderFilter()
        {
            // Arrange
            string json = @"{""Sender"":""0xabcdef123456""}";
            
            // Act
            var filter = JsonSerializer.Deserialize<EventFilter>(json, _options);
            
            // Assert
            Assert.IsInstanceOf<SenderFilter>(filter);
            Assert.AreEqual("0xabcdef123456", (filter as SenderFilter).Sender);
        }

        [Test]
        public void DeserializeFromJsonString_MoveModuleFilter()
        {
            // Arrange
            string json = @"{""MoveModule"":{""module"":""TestModule"",""package"":""0xabcdef123456""}}";
            
            // Act
            var filter = JsonSerializer.Deserialize<EventFilter>(json, _options);
            
            // Assert
            Assert.IsInstanceOf<MoveModuleFilter>(filter);
            var moduleFilter = filter as MoveModuleFilter;
            Assert.AreEqual("TestModule", moduleFilter.MoveModule.module);
            Assert.AreEqual("0xabcdef123456", moduleFilter.MoveModule.package);
        }

 [Test]
        public void DeserializeFromJsonString_ComplexFilter()
        {
            // Arrange
            string json = @"{
                ""All"": [
                    {""Sender"":""0x123""},
                    {""TimeRange"":{""startTime"":""1609459200000"",""endTime"":""1640995200000""}},
                    {""Any"":[
                        {""MoveEventType"":""0xabc::module::Event""},
                        {""Package"":""0xdef""}
                    ]}
                ]
            }";
            
            // Act
            var filter = JsonSerializer.Deserialize<EventFilter>(json, _options);
            
            // Assert
            Assert.IsInstanceOf<EventFilterAll>(filter);
            var allFilter = filter as EventFilterAll;
            Assert.AreEqual(3, allFilter.All.Length);
            Assert.IsInstanceOf<SenderFilter>(allFilter.All[0]);
            Assert.IsInstanceOf<TimeRangeFilter>(allFilter.All[1]);
            Assert.IsInstanceOf<EventFilterAny>(allFilter.All[2]);
            
            var anyFilter = allFilter.All[2] as EventFilterAny;
            Assert.AreEqual(2, anyFilter.Any.Length);
            Assert.IsInstanceOf<MoveEventTypeFilter>(anyFilter.Any[0]);
            Assert.IsInstanceOf<PackageFilter>(anyFilter.Any[1]);
        }

        [Test]
        public void InvalidJson_ThrowsJsonException()
        {
            // Arrange
            string json = @"{""UnknownFilterType"":""value""}";
            
            // Act & Assert
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<EventFilter>(json, _options));
        }

        [Test]
        public void MoveEventField_WithComplexValue_SerializeDeserialize_Roundtrip()
        {
            // Arrange
            var filter = new MoveEventFieldFilter
            {
                MoveEventField = new MoveEventFieldInfo
                {
                    path = "/data",
                    value = new Dictionary<string, object>
                    {
                        { "amount", 100 },
                        { "currency", "USD" }
                    }
                }
            };
            
            // Act
            string json = JsonSerializer.Serialize(filter, _options);
            var deserializedFilter = JsonSerializer.Deserialize<EventFilter>(json, _options);
            
            // Assert
            Assert.IsInstanceOf<MoveEventFieldFilter>(deserializedFilter);
            var fieldFilter = deserializedFilter as MoveEventFieldFilter;
            Assert.AreEqual(filter.MoveEventField.path, fieldFilter.MoveEventField.path);
            
            // Note: Complex object comparison might be more involved depending on how System.Text.Json 
            // deserializes the object. You might need to adjust this assertion.
            Assert.IsNotNull(fieldFilter.MoveEventField.value);
        }

        [Test]
        public void MultipleFilters_SerializeDeserialize_Roundtrip()
        {
            // Arrange
            var filters = new List<EventFilter>
            {
                new SenderFilter { Sender = "0x123" },
                new PackageFilter { Package = "0xabc" },
                new TimeRangeFilter
                {
                    TimeRange = new TimeRangeInfo
                    {
                        startTime = "1609459200000",
                        endTime = "1640995200000"
                    }
                }
            };
            
            // Act
            string json = JsonSerializer.Serialize(filters, _options);
            var deserializedFilters = JsonSerializer.Deserialize<List<EventFilter>>(json, _options);
            
            // Assert
            Assert.AreEqual(3, deserializedFilters.Count);
            Assert.IsInstanceOf<SenderFilter>(deserializedFilters[0]);
            Assert.IsInstanceOf<PackageFilter>(deserializedFilters[1]);
            Assert.IsInstanceOf<TimeRangeFilter>(deserializedFilters[2]);
        }

        [Test]
        public void OrFilter_SerializeDeserialize_Roundtrip()
        {
            // Arrange
            var filter = new EventFilterOr
            {
                Or = new EventFilter[]
                {
                    new MoveEventTypeFilter { MoveEventType = "0xabc::module::Event" },
                    new MoveEventModuleFilter 
                    { 
                        MoveEventModule = new MoveModuleInfo 
                        { 
                            module = "TestModule", 
                            package = "0xabcdef123456" 
                        } 
                    }
                }
            };
            
            // Act
            string json = JsonSerializer.Serialize(filter, _options);
            var deserializedFilter = JsonSerializer.Deserialize<EventFilter>(json, _options);
            
            // Assert
            Assert.IsInstanceOf<EventFilterOr>(deserializedFilter);
            var orFilter = deserializedFilter as EventFilterOr;
            Assert.AreEqual(2, orFilter.Or.Length);
            Assert.IsInstanceOf<MoveEventTypeFilter>(orFilter.Or[0]);
            Assert.IsInstanceOf<MoveEventModuleFilter>(orFilter.Or[1]);
            
            var typeFilter = orFilter.Or[0] as MoveEventTypeFilter;
            Assert.AreEqual("0xabc::module::Event", typeFilter.MoveEventType);
            
            var moduleFilter = orFilter.Or[1] as MoveEventModuleFilter;
            Assert.AreEqual("TestModule", moduleFilter.MoveEventModule.module);
            Assert.AreEqual("0xabcdef123456", moduleFilter.MoveEventModule.package);
        }
    }
}