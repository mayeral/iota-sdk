using iota_sdk.model.governance;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iota_sdk_tests.model
{
    [TestFixture]
    public class IotaSystemStateSummaryConverterTests
    {
        private JsonSerializer _serializer;

        [SetUp]
        public void Setup()
        {
            _serializer = new JsonSerializer();
            _serializer.Converters.Add(new IotaSystemStateSummaryConverter());
        }

        [Test]
        public void ReadJson_V1FlattenedFormat_DeserializesCorrectly()
        {
            // Arrange - V1 format is flattened (no "V1" property)
            string json = @"{
                ""epoch"": ""41"",
                ""protocolVersion"": ""7"",
                ""systemStateVersion"": ""2"",
                ""iotaTotalSupply"": ""4631446260984000000"",
                ""iotaTreasuryCapId"": ""0x03f980dbd802f9cae9a91a95c7d8918d53e960a529224a4fe949f16ed0a0bfe6""
            }";

            // Act
            IotaSystemStateSummary summary;
            using (var stringReader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                summary = _serializer.Deserialize<IotaSystemStateSummary>(jsonReader);
            }

            // Assert
            Assert.IsNotNull(summary);
            Assert.IsTrue(summary.IsV1);
            Assert.IsFalse(summary.IsV2);
            Assert.IsNotNull(summary.V1);
            Assert.AreEqual(41UL, summary.V1.Epoch);
            Assert.AreEqual(7UL, summary.V1.ProtocolVersion);
            Assert.AreEqual(2UL, summary.V1.SystemStateVersion);
            Assert.AreEqual(4631446260984000000UL, summary.V1.IotaTotalSupply);
            Assert.AreEqual("0x03f980dbd802f9cae9a91a95c7d8918d53e960a529224a4fe949f16ed0a0bfe6", summary.V1.IotaTreasuryCapId);
        }

        [Test]
        public void ReadJson_V2NestedFormat_DeserializesCorrectly()
        {
            // Arrange - V2 format has a nested "V2" property
            string json = @"{
                ""V2"": {
                    ""epoch"": ""41"",
                    ""protocolVersion"": ""7"",
                    ""systemStateVersion"": ""2"",
                    ""iotaTotalSupply"": ""4631446260984000000"",
                    ""iotaTreasuryCapId"": ""0x03f980dbd802f9cae9a91a95c7d8918d53e960a529224a4fe949f16ed0a0bfe6"",
                    ""storageFundTotalObjectStorageRebates"": ""294421264000""
                }
            }";

            // Act
            IotaSystemStateSummary summary;
            using (var stringReader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                summary = _serializer.Deserialize<IotaSystemStateSummary>(jsonReader);
            }

            // Assert
            Assert.IsNotNull(summary);
            Assert.IsFalse(summary.IsV1);
            Assert.IsTrue(summary.IsV2);
            Assert.IsNotNull(summary.V2);
            Assert.AreEqual(41UL, summary.V2.Epoch);
            Assert.AreEqual(7UL, summary.V2.ProtocolVersion);
            Assert.AreEqual("0x03f980dbd802f9cae9a91a95c7d8918d53e960a529224a4fe949f16ed0a0bfe6", summary.V2.IotaTreasuryCapId);
            Assert.AreEqual(294421264000UL, summary.V2.StorageFundTotalObjectStorageRebates);
        }

        [Test]
        public void WriteJson_V1Format_SerializesFlattenedCorrectly()
        {
            // Arrange
            var summary = new IotaSystemStateSummary
            {
                V1 = new IotaSystemStateSummaryV1
                {
                    Epoch = 41,
                    ProtocolVersion = 7,
                    SystemStateVersion = 2,
                    IotaTotalSupply = 4631446260984000000,
                    IotaTreasuryCapId = "0x03f980dbd802f9cae9a91a95c7d8918d53e960a529224a4fe949f16ed0a0bfe6"
                }
            };

            // Act
            string json;
            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                _serializer.Serialize(jsonWriter, summary);
                json = stringWriter.ToString();
            }

            // Parse the JSON to verify its structure
            JObject jsonObj = JObject.Parse(json);

            // Assert
            Assert.IsFalse(jsonObj.ContainsKey("V1"), "JSON should not contain V1 property");
            Assert.IsTrue(jsonObj.ContainsKey("epoch"), "JSON should contain flattened epoch property");
            Assert.IsTrue(jsonObj.ContainsKey("protocolVersion"), "JSON should contain flattened protocolVersion property");
            Assert.IsTrue(jsonObj.ContainsKey("systemStateVersion"), "JSON should contain flattened systemStateVersion property");
            Assert.IsTrue(jsonObj.ContainsKey("iotaTotalSupply"), "JSON should contain flattened iotaTotalSupply property");
            Assert.IsTrue(jsonObj.ContainsKey("iotaTreasuryCapId"), "JSON should contain flattened iotaTreasuryCapId property");
            
            Assert.AreEqual(41, (long)jsonObj["epoch"]);
            Assert.AreEqual(7, (long)jsonObj["protocolVersion"]);
            Assert.AreEqual("0x03f980dbd802f9cae9a91a95c7d8918d53e960a529224a4fe949f16ed0a0bfe6", (string)jsonObj["iotaTreasuryCapId"]);
        }

        [Test]
        public void WriteJson_V2Format_SerializesCorrectly()
        {
            // Arrange
            var summary = new IotaSystemStateSummary
            {
                V2 = new IotaSystemStateSummaryV2
                {
                    Epoch = 41,
                    ProtocolVersion = 7,
                    SystemStateVersion = 2,
                    IotaTotalSupply = 4631446260984000000,
                    IotaTreasuryCapId = "0x03f980dbd802f9cae9a91a95c7d8918d53e960a529224a4fe949f16ed0a0bfe6",
                    StorageFundTotalObjectStorageRebates = 294421264000
                }
            };

            // Act
            string json;
            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                _serializer.Serialize(jsonWriter, summary);
                json = stringWriter.ToString();
            }

            // Parse the JSON to verify its structure
            JObject jsonObj = JObject.Parse(json);

            // Assert
            Assert.IsTrue(jsonObj.ContainsKey("V2"), "JSON should contain V2 property");
            Assert.IsTrue(jsonObj["V2"].HasValues);
            Assert.AreEqual(41, (long)jsonObj["V2"]["epoch"]);
            Assert.AreEqual(7, (long)jsonObj["V2"]["protocolVersion"]);
            Assert.AreEqual("0x03f980dbd802f9cae9a91a95c7d8918d53e960a529224a4fe949f16ed0a0bfe6", 
                (string)jsonObj["V2"]["iotaTreasuryCapId"]);
            Assert.AreEqual(294421264000, (long)jsonObj["V2"]["storageFundTotalObjectStorageRebates"]);
        }

        [Test]
        public void ReadJson_InvalidFormat_ThrowsException()
        {
            // Arrange
            string json = @"{ ""invalidProperty"": true }";

            // Act & Assert
            using (var stringReader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                Assert.Throws<JsonSerializationException>(() => 
                    _serializer.Deserialize<IotaSystemStateSummary>(jsonReader));
            }
        }
    }
}