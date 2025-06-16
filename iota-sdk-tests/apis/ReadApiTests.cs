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
        _client = (IotaClient)await clientBuilder.BuildMainnet().ConfigureAwait(false);

        // Initialize the ReadApi with the client
        _target = (ReadApi)_client.ReadApi();
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

    [Test]
    public async Task GetCheckpointsAsync_ReturnsValidCheckpointPage()
    {
        // Arrange
        int limit = 5;
        bool descendingOrder = true;

        // Act
        var result = await _target!.GetCheckpointsAsync(null, limit, descendingOrder).ConfigureAwait(false);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Data);
        Assert.LessOrEqual(result.Data.Count, limit);

        if (result.Data.Count > 0)
        {
            // Verify checkpoint properties
            foreach (var checkpoint in result.Data)
            {
                Assert.IsNotNull(checkpoint);
                Assert.IsNotNull(checkpoint.Digest);
                Assert.IsNotEmpty(checkpoint.Digest);
                Assert.Greater(checkpoint.SequenceNumber, 0);
                Assert.Greater(checkpoint.TimestampMs, 0);
            }

            // Verify descending order (newer checkpoints first)
            for (int i = 0; i < result.Data.Count - 1; i++)
            {
                Assert.GreaterOrEqual(result.Data[i].SequenceNumber, result.Data[i + 1].SequenceNumber);
            }
        }

        // Log basic information about the result
        Console.WriteLine($"Checkpoint page details:");
        Console.WriteLine($"  Total checkpoints in page: {result.Data.Count}");
        Console.WriteLine($"  Has next page: {result.HasNextPage}");
        Console.WriteLine($"  Next cursor: {result.NextCursor}");

        if (result.Data.Count > 0)
        {
            Console.WriteLine($"First checkpoint in page: #{result.Data[0].SequenceNumber} (Digest: {result.Data[0].Digest})");
            Console.WriteLine($"Last checkpoint in page: #{result.Data[^1].SequenceNumber} (Digest: {result.Data[^1].Digest})");
        }
    }

    [Test]
    public async Task GetCheckpointsAsync_WithCursor_ReturnsNextPage()
    {
        // Arrange
        int limit = 3;

        // Get first page
        var firstPage = await _target!.GetCheckpointsAsync(null, limit).ConfigureAwait(false);

        // Skip test if doesn't have next page
        if (!firstPage.HasNextPage || !firstPage.NextCursor.HasValue)
        {
            Assert.Ignore("Test skipped - no next page available");
            return;
        }

        // Act - Get second page using cursor from first page
        var secondPage = await _target!.GetCheckpointsAsync(firstPage.NextCursor, limit).ConfigureAwait(false);

        // Assert
        Assert.IsNotNull(secondPage);
        Assert.IsNotNull(secondPage.Data);
        Assert.LessOrEqual(secondPage.Data.Count, limit);

        if (firstPage.Data.Count > 0 && secondPage.Data.Count > 0)
        {
            // Verify that second page starts after first page ends
            var lastCheckpointInFirstPage = firstPage.Data[^1];
            var firstCheckpointInSecondPage = secondPage.Data[0];

            Assert.Greater(firstCheckpointInSecondPage.SequenceNumber, lastCheckpointInFirstPage.SequenceNumber);
        }

        // Log basic information
        Console.WriteLine($"First page - checkpoint range: {firstPage.Data[0].SequenceNumber} to {firstPage.Data[^1].SequenceNumber}");
        Console.WriteLine($"Second page - checkpoint range: {secondPage.Data[0].SequenceNumber} to {secondPage.Data[^1].SequenceNumber}");
    }

    [Test]
    public async Task GetCheckpointsAsync_WithAscendingOrder_ReturnsOrderedCheckpoints()
    {
        // Arrange
        int limit = 5;
        bool ascendingOrder = false; // Default is ascending

        // Act
        var result = await _target!.GetCheckpointsAsync(null, limit, ascendingOrder).ConfigureAwait(false);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Data);

        if (result.Data.Count > 1)
        {
            // Verify ascending order (older checkpoints first)
            for (int i = 0; i < result.Data.Count - 1; i++)
            {
                Assert.LessOrEqual(result.Data[i].SequenceNumber, result.Data[i + 1].SequenceNumber);
            }
        }

        // Log basic checkpoint sequence info
        if (result.Data.Count > 0)
        {
            Console.WriteLine($"Ascending order - First checkpoint: #{result.Data[0].SequenceNumber}, Last checkpoint: #{result.Data[^1].SequenceNumber}");
        }
    }

    [Test]
    public async Task GetLatestCheckpointSequenceNumberAsync_ReturnsValidSequenceNumber()
    {
        // Act
        var result = await _target!.GetLatestCheckpointSequenceNumberAsync().ConfigureAwait(false);

        // Assert
        Assert.Greater(result, 0);

        // Log the result
        Console.WriteLine($"Latest checkpoint sequence number: {result}");

        // Optional: Verify we can fetch this checkpoint
        var checkpointId = new CheckpointId { SequenceNumber = result };
        var checkpoint = await _target!.GetCheckpointAsync(checkpointId).ConfigureAwait(false);

        Assert.IsNotNull(checkpoint);
        Assert.AreEqual(result, checkpoint.SequenceNumber);
        Console.WriteLine($"Latest checkpoint digest: {checkpoint.Digest}");
        Console.WriteLine($"Latest checkpoint timestamp: {checkpoint.TimestampMs}");
    }

    [Test]
    public async Task GetReferenceGasPriceAsync_ReturnsValidGasPrice()
    {
        // Act
        var result = await _target!.GetReferenceGasPriceAsync().ConfigureAwait(false);

        // Assert
        Assert.Greater(result, 0);

        // Log the result
        Console.WriteLine($"Reference gas price: {result}");
    }

    [Test]
    public async Task GetTotalTransactionBlocksAsync_ReturnsValidCount()
    {
        // Act
        var result = await _target!.GetTotalTransactionBlocksAsync().ConfigureAwait(false);

        // Assert
        Assert.Greater(result, 0);

        // Log the result
        Console.WriteLine($"Total transaction blocks: {result}");
    }

    [Test]
    public async Task GetProtocolConfigAsync_ReturnsValidConfig()
    {
        // Act
        var result = await _target!.GetProtocolConfigAsync().ConfigureAwait(false);

        // Assert
        Assert.IsNotNull(result);
        Assert.Greater(result.ProtocolVersion, 0);
        Assert.GreaterOrEqual(result.MaxSupportedProtocolVersion, result.ProtocolVersion);
        Assert.LessOrEqual(result.MinSupportedProtocolVersion, result.ProtocolVersion);

        // Check feature flags and attributes
        Assert.IsNotNull(result.FeatureFlags);
        Assert.IsNotNull(result.Attributes);

        // Log some information about the protocol config
        Console.WriteLine($"Protocol version: {result.ProtocolVersion}");
        Console.WriteLine($"Min supported version: {result.MinSupportedProtocolVersion}");
        Console.WriteLine($"Max supported version: {result.MaxSupportedProtocolVersion}");
        Console.WriteLine($"Number of feature flags: {result.FeatureFlags.Count}");
        Console.WriteLine($"Number of attributes: {result.Attributes.Count}");

        // Log some feature flags if available
        if (result.FeatureFlags.Count > 0)
        {
            Console.WriteLine("Sample feature flags:");
            foreach (var flag in result.FeatureFlags.Take(5))
            {
                Console.WriteLine($"  {flag.Key}: {flag.Value}");
            }
        }

        // Log some attributes if available
        if (result.Attributes.Count > 0)
        {
            Console.WriteLine("Sample attributes:");
            foreach (var attr in result.Attributes.Take(5))
            {
                Console.WriteLine($"  {attr.Key}: {attr.Value?.Value}");
            }
        }
    }

    [Test]
    public async Task GetProtocolConfigAsync_WithVersion_ReturnsSpecificConfig()
    {
        // Skip if we don't know what version to use
        var currentConfig = await _target!.GetProtocolConfigAsync().ConfigureAwait(false);
        if (currentConfig.ProtocolVersion <= 1)
        {
            Assert.Ignore("Test skipped - need a previous protocol version to test with");
            return;
        }

        // Use previous version
        var previousVersion = currentConfig.ProtocolVersion - 1;

        // Act
        var result = await _target!.GetProtocolConfigAsync(previousVersion).ConfigureAwait(false);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(previousVersion, result.ProtocolVersion);

        Console.WriteLine($"Retrieved protocol config for version: {previousVersion}");
    }
}