using iota_sdk;
using iota_sdk.apis.read;
using iota_sdk.apis.read.m;

namespace iota_sdk_tests.apis;

[TestFixture]
public class ReadApiTests
{
    private IotaClient _client;
    private ReadApi _target;

    [SetUp]
    public async Task Setup()
    {
        // Initialize the IotaClient using IotaClientBuilder
        var clientBuilder = new IotaClientBuilder()
            .RequestTimeout(TimeSpan.FromSeconds(30))
            .MaxConcurrentRequests(10);

        // Use the main net endpoint for testing purposes
        _client = (IotaClient) await clientBuilder.BuildMainnet().ConfigureAwait(false);

        // Initialize the ReadApi with the client
        _target = (ReadApi) _client.ReadApi();
    }

    [Test]
    public async Task GetChainIdentifierAsync_ReturnsChainIdentifier()
    {
        // act
        var result = await _target!.GetChainIdentifierAsync().ConfigureAwait(false);

        // assert
        Assert.IsNotNull(result);
        Assert.That(result.Length, Is.EqualTo(8));               // Chain identifier should be 8 characters (4 bytes as hex)
        Assert.That(result, Does.Match("^[0-9a-f]{8}$")); // Should be 8 hex characters
    }

    [Test]
    public async Task GetCheckpointAsync_WithSequenceNumber500_ReturnsValidCheckpoint()
    {
        // Arrange
        var checkpointId = new CheckpointId
        {
            SequenceNumber = 500
        };

        // Act
        var result = await _target!.GetCheckpointAsync(checkpointId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(500, result.SequenceNumber);
        Assert.IsNotNull(result.Digest);
        Assert.IsNotEmpty(result.Digest);
        Assert.IsNotNull(result.Epoch);
        Assert.IsNotNull(result.TimestampMs);
            Assert.Greater(result.TimestampMs, 0);
        
        // Additional assertions for other properties
        Assert.IsNotNull(result.NetworkTotalTransactions);
        Assert.IsNotNull(result.Transactions);
        
        // Log some information about the checkpoint for debugging
        Console.WriteLine($"Checkpoint {result.SequenceNumber} details:");
        Console.WriteLine($"  Digest: {result.Digest}");
        Console.WriteLine($"  Epoch: {result.Epoch}");
        Console.WriteLine($"  Timestamp: {result.TimestampMs}");
        Console.WriteLine($"  Network Total Transactions: {result.NetworkTotalTransactions}");
        Console.WriteLine($"  Transaction Count: {result.Transactions.Count}");
    }

    [Test]
    public async Task GetCheckpointAsync_WithDigest_ReturnsValidCheckpoint()
    {
        // Arrange
        var checkpointIdPRev = new CheckpointId
        {
            SequenceNumber = 500
        };


        var prevResult = await _target!.GetCheckpointAsync(checkpointIdPRev);


        var knownDigest = prevResult.Digest;
        var checkpointId = new CheckpointId
        {
            Digest = knownDigest
        };

        // Act
        var result = await _target!.GetCheckpointAsync(checkpointId).ConfigureAwait(false);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(500, result.SequenceNumber);
        Assert.AreEqual(knownDigest, result.Digest);
    
        // Log checkpoint details
        Console.WriteLine($"Checkpoint {result.SequenceNumber} details:");
        Console.WriteLine($"  Digest: {result.Digest}");
        Console.WriteLine($"  Timestamp: {result.TimestampMs}");
    }
}