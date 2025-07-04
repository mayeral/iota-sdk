﻿using iota_sdk;
using iota_sdk.apis.read;
using iota_sdk.model;
using iota_sdk.model.read.checkpoint;
using iota_sdk.model.read.@object;
using iota_sdk.model.read.transaction;
using iota_sdk_tests.utils;
using Newtonsoft.Json;

namespace iota_sdk_tests.apis;

[TestFixture]
public class ReadApiTests
{
    private IotaClient _client;
    private ReadApi _target;

    private ObjectId _testObjectId;
    private TransactionDigest _testTransactionDigest;
    private IotaAddress _testAddress;
    private ObjectId _testObjectId2;
    private TransactionDigest _testTransactionDigest2;

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

        _testObjectId2 = TestsUtils.InitTestObjectId2();
        _testObjectId = TestsUtils.InitTestObjectId();
        _testAddress = TestsUtils.InitTestAddress();
        _testTransactionDigest = TestsUtils.InitTransactionDigest();
        _testTransactionDigest2 = TestsUtils.InitTransactionDigest2();
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
        Assert.Greater(checkpoint.TimestampMs, 0);
        Assert.IsNotNull(checkpoint.Digest);
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

    [Test]
    public async Task GetObjectWithOptionsAsync_ShouldReturnObjectData()
    {
        // Arrange
        var options = new IotaObjectDataOptions
        {
            ShowType = true,
            ShowOwner = true,
            ShowPreviousTransaction = true,
            ShowContent = true,
            ShowStorageRebate = true,
            ShowBcs = true,
            ShowDisplay = true
        };

        // Act
        var result = await _target.GetObjectAsync(_testObjectId, options);

        // Assert
        Assert.IsNotNull(result, "Response should not be null");
        Assert.IsNotNull(result.Data, "Response data should not be null");

        // Validate object ID
        Assert.AreEqual(_testObjectId.ToString(), result.Data.ObjectId, "Object ID should match");

        // Validate version
        Assert.IsNotNull(result.Data.Version, "Version should not be null");

        // Validate digest
        Assert.IsNotNull(result.Data.Digest, "Digest should not be null");

        // Validate type (if requested)
        if (options.ShowType == true)
        {
            Assert.IsNotNull(result.Data.Type, "Type should not be null when ShowType is true");
        }

        // Validate owner (if requested)
        if (options.ShowOwner == true)
        {
            Assert.IsNotNull(result.Data.Owner, "Owner should not be null when ShowOwner is true");
        }

        // Validate previous transaction (if requested)
        if (options.ShowPreviousTransaction == true)
        {
            Assert.IsNotNull(result.Data.PreviousTransaction, "Previous transaction should not be null when ShowPreviousTransaction is true");
        }

        // Validate storage rebate (if requested)
        if (options.ShowStorageRebate == true)
        {
            Assert.IsNotNull(result.Data.StorageRebate, "Storage rebate should not be null when ShowStorageRebate is true");
        }

        // Validate content (if requested)
        if (options.ShowContent == true)
        {
            Assert.IsNotNull(result.Data.Content, "Content should not be null when ShowContent is true");

            // If content is a MoveObjectContent, validate its properties
            if (result.Data.Content is MoveObjectContent moveContent)
            {
                Assert.AreEqual("moveObject", moveContent.DataType, "Content data type should be 'moveObject'");
                Assert.IsNotNull(moveContent.Type, "Move content type should not be null");
            }
            // If content is a PackageContent, validate its properties
            else if (result.Data.Content is PackageContent packageContent)
            {
                Assert.AreEqual("package", packageContent.DataType, "Content data type should be 'package'");
                Assert.IsNotNull(packageContent.Disassembled, "Package disassembled content should not be null");
            }
        }

        // Log the result
        Console.WriteLine($"Object Type: {result.Data.Type}");
        Console.WriteLine($"Version: {result.Data.Version}");
    }

    [Test]
    public async Task GetObjectWithMinimalOptionsAsync_ShouldReturnBasicObjectData()
    {
        // Arrange
        var options = new IotaObjectDataOptions
        {
            ShowType = false,
            ShowOwner = false,
            ShowPreviousTransaction = false,
            ShowContent = false,
            ShowStorageRebate = false,
            ShowBcs = false,
            ShowDisplay = false
        };

        // Act
        var result = await _target.GetObjectAsync(_testObjectId, options);

        // Assert
        Assert.IsNotNull(result, "Response should not be null");
        Assert.IsNotNull(result.Data, "Response data should not be null");

        // Validate object ID
        Assert.AreEqual(_testObjectId.ToString(), result.Data.ObjectId, "Object ID should match");

        // Validate basic required fields are present
        Assert.IsNotNull(result.Data.Version, "Version should not be null");
        Assert.IsNotNull(result.Data.Digest, "Digest should not be null");

        // Validate optional fields are not present when not requested
        Assert.IsNull(result.Data.Type, "Type should be null when ShowType is false");
        Assert.IsNull(result.Data.Owner, "Owner should be null when ShowOwner is false");
        Assert.IsNull(result.Data.PreviousTransaction, "Previous transaction should be null when ShowPreviousTransaction is false");
        Assert.IsNull(result.Data.Content, "Content should be null when ShowContent is false");
        Assert.IsNull(result.Data.StorageRebate, "Storage rebate should be null when ShowStorageRebate is false");
        Assert.IsNull(result.Data.Bcs, "BCS data should be null when ShowBcs is false");
        Assert.IsNull(result.Data.Display, "Display data should be null when ShowDisplay is false");

        // Log basic information about the object
        Console.WriteLine($"Object version: {result.Data.Version}");
    }

    [Test]
    public async Task GetTransactionAsync_ReturnsValidTransaction()
    {
        // Act
        var result = await _target!.GetTransactionAsync(_testTransactionDigest).ConfigureAwait(false);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(_testTransactionDigest.ToString(), result.Digest);

        // Log some details about the transaction
        Console.WriteLine($"Transaction Timestamp: {result.TimestampMs}");
        Assert.IsNotNull(result.TimestampMs);
        Assert.IsNotNull(result.Checkpoint);
    }

    [Test]
    public async Task GetTransactionWithOptionsAsync_AllOptionsTrue_ReturnsDetailedTransaction()
    {
        // Arrange
        var options = new IotaTransactionBlockResponseOptions
        {
            ShowInput = true,
            ShowEffects = true,
            ShowEvents = true,
            ShowObjectChanges = true,
            ShowBalanceChanges = true,
            ShowRawEffects = true,
            ShowRawInput = true,
        };

        // Act
        var result = await _target!.GetTransactionAsync(_testTransactionDigest, options).ConfigureAwait(false);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(_testTransactionDigest.ToString(), result.Digest);
        Assert.IsNotNull(result.Checkpoint);
        Assert.IsNotNull(result.TimestampMs);

        // Additional assertions based on the options
        if ((bool)options.ShowInput)
        {
            Assert.NotNull(result.Transaction, "Transaction should be included with ShowInput=true");
        }

        if ((bool)options.ShowEffects)
        {
            Assert.NotNull(result.Effects, "Effects should be included with ShowEffects=true");
        }

        if ((bool)options.ShowEvents)
        {
            Assert.NotNull(result.Events, "Events should be included with ShowEvents=true");
        }

        if ((bool)options.ShowObjectChanges)
        {
            Assert.NotNull(result.ObjectChanges, "ObjectChanges should be included with ShowObjectChanges=true");
        }

        if ((bool)options.ShowBalanceChanges)
        {
            Assert.NotNull(result.BalanceChanges, "BalanceChanges should be included with ShowBalanceChanges=true");
        }

        if ((bool)options.ShowRawInput)
        {
            Assert.NotNull(result.RawTransaction, "RawInput should be included with ShowRawInput=true");
        }

        if ((bool)options.ShowRawEffects)
        {
            Assert.NotNull(result.RawEffects, "RawEffects should be included with ShowRawEffects=true");
        }
    }

    [Test]
    public async Task GetTransactionWithOptionsAsync_AllOptionsFalse_ReturnsMinimalTransaction()
    {
        // Arrange
        var options = new IotaTransactionBlockResponseOptions
        {
            ShowInput = false,
            ShowEffects = false,
            ShowEvents = false,
            ShowObjectChanges = false,
            ShowBalanceChanges = false
        };

        // Act
        var result = await _target!.GetTransactionAsync(_testTransactionDigest, options).ConfigureAwait(false);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(_testTransactionDigest.ToString(), result.Digest);
        Assert.IsNotNull(result.Checkpoint);
        Assert.IsNotNull(result.TimestampMs);


        // Note: With all options set to false, we expect minimal data
        // These assertions might fail if the API returns data regardless of options
        // In that case, you might want to comment them out
        Assert.Null(result.Transaction, "Transaction should not be included with ShowInput=false");
        Assert.Null(result.Effects, "Effects should not be included with ShowEffects=false");
        Assert.Null(result.Events, "Events should not be included with ShowEvents=false");
        Assert.Null(result.ObjectChanges, "Object changes should not be included with ShowObjectChanges=false");
        Assert.Null(result.BalanceChanges, "Balance changes should not be included with ShowBalanceChanges=false");
    }

    [Test]
    public async Task MultiGetObjectsWithOptionsAsync_ReturnsValidObjects()
    {
        // Arrange
        var objectIds = new List<ObjectId>
        {
    _testObjectId,
    _testObjectId2
        };

        var options = new IotaObjectDataOptions
        {
            ShowType = true,
            ShowOwner = true,
            ShowContent = true,
            ShowDisplay = true,
            ShowBcs = true,
            ShowStorageRebate = true,
            ShowPreviousTransaction = true
        };

        // Act
        var result = await _target!.MultiGetObjectsAsync(objectIds, options).ConfigureAwait(false);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(2, result?.ToList().Count);

        // Verify that the returned objects match our requested IDs
        var returnedIds = result?.Select(obj => obj?.Data?.ObjectId).ToList();
        Assert.Contains(objectIds[0].ToString(), returnedIds);
        Assert.Contains(objectIds[1].ToString(), returnedIds);

        // Log some details
        foreach (var obj in result)
        {
            Console.WriteLine($"Object Type: {obj?.Data?.Type}");
            Console.WriteLine($"Object Owner: {obj?.Data?.Owner}");
            Console.WriteLine("---");
        }
    }

    [Test]
    public async Task MultiGetTransactionsWithOptionsAsync_ReturnsValidTransactions()
    {
        // Arrange
        var digests = new List<TransactionDigest>
        {
    _testTransactionDigest,
    _testTransactionDigest2
        };

        var options = new IotaTransactionBlockResponseOptions
        {
            ShowInput = true,
            ShowRawInput = true,
            ShowEffects = true,
            ShowEvents = true,
            ShowObjectChanges = true,
            ShowBalanceChanges = true,
            ShowRawEffects = true
        };

        // Act
        var result = await _target!.MultiGetTransactionsAsync(digests, options).ConfigureAwait(false);

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.AreEqual(2, resultList.Count);

        // Verify that the returned transactions match our requested digests
        var returnedDigests = resultList.Select(tx => tx.Digest).ToList();
        Assert.Contains(digests[0].ToString(), returnedDigests);
        Assert.Contains(digests[1].ToString(), returnedDigests);

        // Log some details with more information based on the options
        foreach (var tx in resultList)
        {
            Console.WriteLine($"Transaction Timestamp: {tx.TimestampMs}");

            if (tx.Transaction != null)
            {
                Console.WriteLine($"Transaction Sender: {tx.Transaction.Data.Sender}");
            }

            if (tx.Events != null && tx.Events.Any())
            {
                Console.WriteLine($"Number of Events: {tx.Events.Count}");
            }

            Console.WriteLine("---");
        }
    }

    [Test]
    public async Task GetOwnedObjectsAsync_ReturnsValidObjects()
    {
        // Arrange
        var address = new IotaAddress(_testAddress);
        var query = new IotaObjectResponseQuery
        {
            Options = new IotaObjectDataOptions
            {
                ShowType = true,
                ShowOwner = true,
                ShowContent = true,
                ShowDisplay = false,
                ShowBcs = false,
                ShowStorageRebate = true,
                ShowPreviousTransaction = true
            }
        };
        int limit = 10;

        // Act
        var result = await _target!.GetOwnedObjectsAsync(address, query, null, limit).ConfigureAwait(false);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Data);

        Console.WriteLine($"Objects found: {result.Data.Count}");
        Console.WriteLine($"Has next page: {result.HasNextPage}");
        Console.WriteLine($"Next cursor: {result.NextCursor}");

        // Log details of returned objects
        foreach (var obj in result.Data)
        {
            Console.WriteLine($"Object Type: {obj?.Data?.Type}");
            Console.WriteLine("---");
        }
    }

    [Test]
    public async Task GetOwnedObjectsAsync_WithPagination_ReturnsValidObjects()
    {
        // Arrange
        int limit = 2; // Small limit to test pagination
        ObjectId? cursor = null;

        // Act & Assert - First page
        ObjectsPage firstPage = await _target!.GetOwnedObjectsAsync(_testAddress, null, cursor, limit).ConfigureAwait(false);

        Assert.NotNull(firstPage);
        Assert.NotNull(firstPage.Data);
        Assert.LessOrEqual(firstPage.Data.Count, limit);

        Console.WriteLine("First Page:");
        Console.WriteLine($"Objects found: {firstPage.Data.Count}");
        Console.WriteLine($"Has next page: {firstPage.HasNextPage}");

        if (firstPage.HasNextPage && !string.IsNullOrEmpty(firstPage.NextCursor))
        {
            // Act & Assert - Second page
            cursor = new ObjectId(firstPage.NextCursor);
            var secondPage = await _target!.GetOwnedObjectsAsync(_testAddress, null, cursor, limit).ConfigureAwait(false);

            Assert.NotNull(secondPage);
            Assert.NotNull(secondPage.Data);
            Assert.LessOrEqual(secondPage.Data.Count, limit);

            Console.WriteLine("\nSecond Page:");
            Console.WriteLine($"Objects found: {secondPage.Data.Count}");
            Console.WriteLine($"Has next page: {secondPage.HasNextPage}");

            // Verify the objects from the first and second pages are different
            var firstPageIds = firstPage.Data.Select(o => o?.Data?.ObjectId).ToList();
            var secondPageIds = secondPage.Data.Select(o => o?.Data?.ObjectId).ToList();

            foreach (var id in secondPageIds)
            {
                Assert.IsFalse(firstPageIds.Contains(id), "Second page contains duplicate objects from first page");
            }
        }
    }

    [Test]
    public async Task GetObjectsWithFilter_ReturnsObjects()
    {

        // Arrange
        var address = _testAddress;
        var filter = new MatchAllFilter
        {
            MatchAll = new IotaObjectDataFilter[]
            {
            new StructTypeFilter { StructType = "0x107a::nft::Nft" },
            new AddressOwnerFilter { AddressOwner = address }
            }
        };

        var query = new IotaObjectResponseQuery
        {
            Filter = filter,
            Options = new IotaObjectDataOptions
            {
                ShowContent = true,
                ShowType = true,
                ShowOwner = true,
                ShowDisplay = true
            }
        };

        // Act
        var result = await _target!.GetOwnedObjectsAsync(_testAddress, query).ConfigureAwait(false);

        // Assert
        Assert.IsNotNull(result);

        // Verify each returned object matches our filter criteria
        foreach (var item in result.Data)
        {
            Assert.IsNotNull(item.Data?.Type);
            Assert.IsTrue(item.Data?.Type?.Contains("0x107a::nft::Nft"), $"Type {item.Data?.Type} doesn't match expected 0x107a::nft::Nft");

            Assert.IsNotNull(item.Data?.Owner);
            string ownerJson = JsonConvert.SerializeObject(item.Data?.Owner);
            Assert.IsTrue(ownerJson.Contains(address), $"Owner doesn't contain expected address");
        }
    }
}