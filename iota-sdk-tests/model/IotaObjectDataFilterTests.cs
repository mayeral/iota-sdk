using System.Text.Json;
using Iota.Sdk.Model.Read;

namespace iota_sdk_tests.model
{
    [TestFixture]
    public class IotaObjectDataFilterTests
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
        public void Package_SerializeDeserialize_Roundtrip()
        {
            // Arrange
            var filter = new PackageFilter { Package = "0x123456789abcdef" };

            // Act
            string json = JsonSerializer.Serialize(filter, _options);
            var deserializedFilter = JsonSerializer.Deserialize<IotaObjectDataFilter>(json, _options);

            // Assert
            Assert.IsInstanceOf<PackageFilter>(deserializedFilter);
            var packageFilter = deserializedFilter as PackageFilter;
            Assert.AreEqual(filter.Package, packageFilter.Package);
        }

        [Test]
        public void MoveModule_SerializeDeserialize_Roundtrip()
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
            var deserializedFilter = JsonSerializer.Deserialize<IotaObjectDataFilter>(json, _options);

            // Assert
            Assert.IsInstanceOf<MoveModuleFilter>(deserializedFilter);
            var moduleFilter = deserializedFilter as MoveModuleFilter;
            Assert.AreEqual(filter.MoveModule.module, moduleFilter.MoveModule.module);
            Assert.AreEqual(filter.MoveModule.package, moduleFilter.MoveModule.package);
        }

        [Test]
        public void StructType_SerializeDeserialize_Roundtrip()
        {
            // Arrange
            var filter = new StructTypeFilter { StructType = "0x2::coin::Coin<0x2::iota::IOTA>" };

            // Act
            string json = JsonSerializer.Serialize(filter, _options);
            var deserializedFilter = JsonSerializer.Deserialize<IotaObjectDataFilter>(json, _options);

            // Assert
            Assert.IsInstanceOf<StructTypeFilter>(deserializedFilter);
            var structTypeFilter = deserializedFilter as StructTypeFilter;
            Assert.AreEqual(filter.StructType, structTypeFilter.StructType);
        }

        [Test]
        public void AddressOwner_SerializeDeserialize_Roundtrip()
        {
            // Arrange
            var filter = new AddressOwnerFilter { AddressOwner = "0x123456789abcdef" };

            // Act
            string json = JsonSerializer.Serialize(filter, _options);
            var deserializedFilter = JsonSerializer.Deserialize<IotaObjectDataFilter>(json, _options);

            // Assert
            Assert.IsInstanceOf<AddressOwnerFilter>(deserializedFilter);
            var addressOwnerFilter = deserializedFilter as AddressOwnerFilter;
            Assert.AreEqual(filter.AddressOwner, addressOwnerFilter.AddressOwner);
        }

        [Test]
        public void ObjectOwner_SerializeDeserialize_Roundtrip()
        {
            // Arrange
            var filter = new ObjectOwnerFilter { ObjectOwner = "0x123456789abcdef" };

            // Act
            string json = JsonSerializer.Serialize(filter, _options);
            var deserializedFilter = JsonSerializer.Deserialize<IotaObjectDataFilter>(json, _options);

            // Assert
            Assert.IsInstanceOf<ObjectOwnerFilter>(deserializedFilter);
            var objectOwnerFilter = deserializedFilter as ObjectOwnerFilter;
            Assert.AreEqual(filter.ObjectOwner, objectOwnerFilter.ObjectOwner);
        }

        [Test]
        public void ObjectId_SerializeDeserialize_Roundtrip()
        {
            // Arrange
            var filter = new ObjectIdFilter { ObjectId = "0x123456789abcdef" };

            // Act
            string json = JsonSerializer.Serialize(filter, _options);
            var deserializedFilter = JsonSerializer.Deserialize<IotaObjectDataFilter>(json, _options);

            // Assert
            Assert.IsInstanceOf<ObjectIdFilter>(deserializedFilter);
            var objectIdFilter = deserializedFilter as ObjectIdFilter;
            Assert.AreEqual(filter.ObjectId, objectIdFilter.ObjectId);
        }

        [Test]
        public void ObjectIds_SerializeDeserialize_Roundtrip()
        {
            // Arrange
            var filter = new ObjectIdsFilter { ObjectIds = new[] { "0x123", "0x456", "0x789" } };

            // Act
            string json = JsonSerializer.Serialize(filter, _options);
            var deserializedFilter = JsonSerializer.Deserialize<IotaObjectDataFilter>(json, _options);

            // Assert
            Assert.IsInstanceOf<ObjectIdsFilter>(deserializedFilter);
            var objectIdsFilter = deserializedFilter as ObjectIdsFilter;
            Assert.AreEqual(filter.ObjectIds.Length, objectIdsFilter.ObjectIds.Length);
            for (int i = 0; i < filter.ObjectIds.Length; i++)
            {
                Assert.AreEqual(filter.ObjectIds[i], objectIdsFilter.ObjectIds[i]);
            }
        }

        [Test]
        public void Version_SerializeDeserialize_Roundtrip()
        {
            // Arrange
            var filter = new VersionFilter { Version = "1234567" };

            // Act
            string json = JsonSerializer.Serialize(filter, _options);
            var deserializedFilter = JsonSerializer.Deserialize<IotaObjectDataFilter>(json, _options);

            // Assert
            Assert.IsInstanceOf<VersionFilter>(deserializedFilter);
            var versionFilter = deserializedFilter as VersionFilter;
            Assert.AreEqual(filter.Version, versionFilter.Version);
        }

        [Test]
        public void MatchAll_SerializeDeserialize_Roundtrip()
        {
            // Arrange
            var filter = new MatchAllFilter
            {
                MatchAll = new IotaObjectDataFilter[]
                {
                    new PackageFilter { Package = "0x123" },
                    new ObjectIdFilter { ObjectId = "0xabc" }
                }
            };

            // Act
            string json = JsonSerializer.Serialize(filter, _options);
            var deserializedFilter = JsonSerializer.Deserialize<IotaObjectDataFilter>(json, _options);

            // Assert
            Assert.IsInstanceOf<MatchAllFilter>(deserializedFilter);
            var matchAllFilter = deserializedFilter as MatchAllFilter;
            Assert.AreEqual(2, matchAllFilter.MatchAll.Length);
            Assert.IsInstanceOf<PackageFilter>(matchAllFilter.MatchAll[0]);
            Assert.IsInstanceOf<ObjectIdFilter>(matchAllFilter.MatchAll[1]);
        }
        [Test]
        public void MatchAny_SerializeDeserialize_Roundtrip()
        {
            // Arrange
            var filter = new MatchAnyFilter
            {
                MatchAny = new IotaObjectDataFilter[]
                {
                    new PackageFilter { Package = "0x123" },
                    new ObjectIdFilter { ObjectId = "0xabc" }
                }
            };

            // Act
            string json = JsonSerializer.Serialize(filter, _options);
            var deserializedFilter = JsonSerializer.Deserialize<IotaObjectDataFilter>(json, _options);

            // Assert
            Assert.IsInstanceOf<MatchAnyFilter>(deserializedFilter);
            var matchAnyFilter = deserializedFilter as MatchAnyFilter;
            Assert.AreEqual(2, matchAnyFilter.MatchAny.Length);
            Assert.IsInstanceOf<PackageFilter>(matchAnyFilter.MatchAny[0]);
            Assert.IsInstanceOf<ObjectIdFilter>(matchAnyFilter.MatchAny[1]);
        }

        [Test]
        public void MatchNone_SerializeDeserialize_Roundtrip()
        {
            // Arrange
            var filter = new MatchNoneFilter
            {
                MatchNone = new IotaObjectDataFilter[]
                {
                    new PackageFilter { Package = "0x123" },
                    new ObjectIdFilter { ObjectId = "0xabc" }
                }
            };

            // Act
            string json = JsonSerializer.Serialize(filter, _options);
            var deserializedFilter = JsonSerializer.Deserialize<IotaObjectDataFilter>(json, _options);

            // Assert
            Assert.IsInstanceOf<MatchNoneFilter>(deserializedFilter);
            var matchNoneFilter = deserializedFilter as MatchNoneFilter;
            Assert.AreEqual(2, matchNoneFilter.MatchNone.Length);
            Assert.IsInstanceOf<PackageFilter>(matchNoneFilter.MatchNone[0]);
            Assert.IsInstanceOf<ObjectIdFilter>(matchNoneFilter.MatchNone[1]);
        }

        [Test]
        public void DeserializeFromJsonString_PackageFilter()
        {
            // Arrange
            string json = "{\"Package\":\"0x123456789abcdef\"}";

            // Act
            var filter = JsonSerializer.Deserialize<IotaObjectDataFilter>(json, _options);

            // Assert
            Assert.IsInstanceOf<PackageFilter>(filter);
            var packageFilter = filter as PackageFilter;
            Assert.AreEqual("0x123456789abcdef", packageFilter.Package);
        }

        [Test]
        public void DeserializeFromJsonString_MoveModuleFilter()
        {
            // Arrange
            string json = "{\"MoveModule\":{\"module\":\"TestModule\",\"package\":\"0xabcdef123456\"}}";

            // Act
            var filter = JsonSerializer.Deserialize<IotaObjectDataFilter>(json, _options);

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
        ""MatchAll"": [
          {
            ""StructType"": ""0x2::coin::Coin<0x2::iota::IOTA>""
          },
          {
            ""AddressOwner"": ""0x0cd4bb4d4f520fe9bbf0cf1cebe3f2549412826c3c9261bff9786c240123749f""
          },
          {
            ""Version"": ""13488""
          }
        ]
    }";

            // Act
            var filter = JsonSerializer.Deserialize<IotaObjectDataFilter>(json, _options);

            // Assert
            Assert.IsInstanceOf<MatchAllFilter>(filter);
            var matchAllFilter = filter as MatchAllFilter;
            Assert.AreEqual(3, matchAllFilter.MatchAll.Length);
            Assert.IsInstanceOf<StructTypeFilter>(matchAllFilter.MatchAll[0]);
            Assert.IsInstanceOf<AddressOwnerFilter>(matchAllFilter.MatchAll[1]);
            Assert.IsInstanceOf<VersionFilter>(matchAllFilter.MatchAll[2]);

            var structTypeFilter = matchAllFilter.MatchAll[0] as StructTypeFilter;
            var addressOwnerFilter = matchAllFilter.MatchAll[1] as AddressOwnerFilter;
            var versionFilter = matchAllFilter.MatchAll[2] as VersionFilter;

            Assert.AreEqual("0x2::coin::Coin<0x2::iota::IOTA>", structTypeFilter.StructType);
            Assert.AreEqual("0x0cd4bb4d4f520fe9bbf0cf1cebe3f2549412826c3c9261bff9786c240123749f", addressOwnerFilter.AddressOwner);
            Assert.AreEqual("13488", versionFilter.Version);
        }

        [Test]
        public void InvalidJson_ThrowsJsonException()
        {
            // Arrange
            string json = "{\"invalid_filter_type\":\"value\"}";

            // Assert
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<IotaObjectDataFilter>(json, _options));
        }

        [Test]
        public void MultipleFilters_SerializeDeserialize_Roundtrip()
        {
            // Arrange
            var filters = new List<IotaObjectDataFilter>
            {
                new PackageFilter { Package = "0x123" },
                new ObjectIdFilter { ObjectId = "0xabc" },
                new StructTypeFilter { StructType = "0x2::coin::Coin" }
            };

            // Act
            string json = JsonSerializer.Serialize(filters, _options);
            var deserializedFilters = JsonSerializer.Deserialize<List<IotaObjectDataFilter>>(json, _options);

            // Assert
            Assert.AreEqual(3, deserializedFilters.Count);
            Assert.IsInstanceOf<PackageFilter>(deserializedFilters[0]);
            Assert.IsInstanceOf<ObjectIdFilter>(deserializedFilters[1]);
            Assert.IsInstanceOf<StructTypeFilter>(deserializedFilters[2]);

            var packageFilter = deserializedFilters[0] as PackageFilter;
            var objectIdFilter = deserializedFilters[1] as ObjectIdFilter;
            var structTypeFilter = deserializedFilters[2] as StructTypeFilter;

            Assert.AreEqual("0x123", packageFilter.Package);
            Assert.AreEqual("0xabc", objectIdFilter.ObjectId);
            Assert.AreEqual("0x2::coin::Coin", structTypeFilter.StructType);
        }
    }
}