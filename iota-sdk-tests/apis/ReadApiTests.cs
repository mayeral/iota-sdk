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
}