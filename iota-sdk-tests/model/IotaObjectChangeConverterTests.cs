using iota_sdk.model.read.@object;
using iota_sdk.model.read.transaction;
using Newtonsoft.Json;

namespace iota_sdk_tests.model;

[TestFixture]
public class IotaObjectChangeConverterTests
{
    private JsonSerializerSettings _settings;

    [SetUp]
    public void Setup()
    {
        _settings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new IotaObjectChangeConverter() }
        };
    }

    [Test]
    public void Deserialize_PublishedObjectChange_ShouldDeserializeCorrectly()
    {
        // Arrange
        string json = @"{
            ""type"": ""published"",
            ""digest"": ""abc123"",
            ""modules"": [""module1"", ""module2""],
            ""packageId"": ""pkg123"",
            ""version"": ""1.0.0""
        }";

        // Act
        var result = JsonConvert.DeserializeObject<IotaObjectChange>(json, _settings);

        // Assert
        Assert.IsInstanceOf<PublishedObjectChange>(result);
        var publishedChange = (PublishedObjectChange)result;
        Assert.AreEqual("published", publishedChange.Type);
        Assert.AreEqual("abc123", publishedChange.Digest);
        CollectionAssert.AreEqual(new List<string> { "module1", "module2" }, publishedChange.Modules);
        Assert.AreEqual("pkg123", publishedChange.PackageId);
        Assert.AreEqual("1.0.0", publishedChange.Version);
    }

    [Test]
    public void Deserialize_TransferredObjectChange_ShouldDeserializeCorrectly()
    {
        // Arrange
        string json = @"{
            ""type"": ""transferred"",
            ""digest"": ""abc123"",
            ""objectId"": ""obj123"",
            ""objectType"": ""Test::Type"",
            ""recipient"": { ""AddressOwner"": ""0x123"" },
            ""sender"": ""0x456"",
            ""version"": ""1.0.0""
        }";

        // Act
        var result = JsonConvert.DeserializeObject<IotaObjectChange>(json, _settings);

        // Assert
        Assert.IsInstanceOf<TransferredObjectChange>(result);
        var transferredChange = (TransferredObjectChange) result;
        Assert.AreEqual("transferred", transferredChange.Type);
        Assert.AreEqual("abc123", transferredChange.Digest);
        Assert.AreEqual("obj123", transferredChange.ObjectId);
        Assert.AreEqual("Test::Type", transferredChange.ObjectType);
        var a = (AddressOwner) transferredChange.Recipient;
        Assert.AreEqual("0x123", a.Address);
        Assert.AreEqual("0x456", transferredChange.Sender);
        Assert.AreEqual("1.0.0", transferredChange.Version);
    }

    [Test]
    public void Deserialize_MutatedObjectChange_ShouldDeserializeCorrectly()
    {
        // Arrange
        string json = @"{
            ""type"": ""mutated"",
            ""digest"": ""abc123"",
            ""objectId"": ""obj123"",
            ""objectType"": ""Test::Type"",
            ""owner"": { ""AddressOwner"": ""0x123"" },
            ""previousVersion"": ""0.9.0"",
            ""sender"": ""0x456"",
            ""version"": ""1.0.0""
        }";

        // Act
        var result = JsonConvert.DeserializeObject<IotaObjectChange>(json, _settings);

        // Assert
        Assert.IsInstanceOf<MutatedObjectChange>(result);
        var mutatedChange = (MutatedObjectChange)result;
        Assert.AreEqual("mutated", mutatedChange.Type);
        Assert.AreEqual("abc123", mutatedChange.Digest);
        Assert.AreEqual("obj123", mutatedChange.ObjectId);
        Assert.AreEqual("Test::Type", mutatedChange.ObjectType);
        var a = (AddressOwner)mutatedChange.Owner;
        Assert.AreEqual("0x123", a.Address);
        Assert.AreEqual("0.9.0", mutatedChange.PreviousVersion);
        Assert.AreEqual("0x456", mutatedChange.Sender);
        Assert.AreEqual("1.0.0", mutatedChange.Version);
    }

    [Test]
    public void Deserialize_DeletedObjectChange_ShouldDeserializeCorrectly()
    {
        // Arrange
        string json = @"{
            ""type"": ""deleted"",
            ""objectId"": ""obj123"",
            ""objectType"": ""Test::Type"",
            ""sender"": ""0x456"",
            ""version"": ""1.0.0""
        }";

        // Act
        var result = JsonConvert.DeserializeObject<IotaObjectChange>(json, _settings);

        // Assert
        Assert.IsInstanceOf<DeletedObjectChange>(result);
        var deletedChange = (DeletedObjectChange)result;
        Assert.AreEqual("deleted", deletedChange.Type);
        Assert.AreEqual("obj123", deletedChange.ObjectId);
        Assert.AreEqual("Test::Type", deletedChange.ObjectType);
        Assert.AreEqual("0x456", deletedChange.Sender);
        Assert.AreEqual("1.0.0", deletedChange.Version);
    }

    [Test]
    public void Deserialize_WrappedObjectChange_ShouldDeserializeCorrectly()
    {
        // Arrange
        string json = @"{
            ""type"": ""wrapped"",
            ""objectId"": ""obj123"",
            ""objectType"": ""Test::Type"",
            ""sender"": ""0x456"",
            ""version"": ""1.0.0""
        }";

        // Act
        var result = JsonConvert.DeserializeObject<IotaObjectChange>(json, _settings);

        // Assert
        Assert.IsInstanceOf<WrappedObjectChange>(result);
        var wrappedChange = (WrappedObjectChange)result;
        Assert.AreEqual("wrapped", wrappedChange.Type);
        Assert.AreEqual("obj123", wrappedChange.ObjectId);
        Assert.AreEqual("Test::Type", wrappedChange.ObjectType);
        Assert.AreEqual("0x456", wrappedChange.Sender);
        Assert.AreEqual("1.0.0", wrappedChange.Version);
    }

    [Test]
    public void Deserialize_CreatedObjectChange_ShouldDeserializeCorrectly()
    {
        // Arrange
        string json = @"{
            ""type"": ""created"",
            ""digest"": ""abc123"",
            ""objectId"": ""obj123"",
            ""objectType"": ""Test::Type"",
            ""owner"": { ""AddressOwner"": ""0x123"" },
            ""sender"": ""0x456"",
            ""version"": ""1.0.0""
        }";

        // Act
        var result = JsonConvert.DeserializeObject<IotaObjectChange>(json, _settings);

        // Assert
        Assert.IsInstanceOf<CreatedObjectChange>(result);
        var createdChange = (CreatedObjectChange)result;
        Assert.AreEqual("created", createdChange.Type);
        Assert.AreEqual("abc123", createdChange.Digest);
        Assert.AreEqual("obj123", createdChange.ObjectId);
        Assert.AreEqual("Test::Type", createdChange.ObjectType);
        var a =  (AddressOwner)createdChange.Owner;
        Assert.AreEqual("0x123", a.Address);
        Assert.AreEqual("0x456", createdChange.Sender);
        Assert.AreEqual("1.0.0", createdChange.Version);
    }
}