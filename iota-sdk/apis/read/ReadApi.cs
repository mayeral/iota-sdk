using System.Numerics;
using iota_sdk.model;
using iota_sdk.model.read;

namespace iota_sdk.apis.read;

public class ReadApi : IReadApi
{
    private readonly IotaClient _client;

    /// <summary>
    /// Creates a new instance of <see cref="ReadApi"/>
    /// </summary>
    /// <param name="client">The IOTA client to use for API calls</param>

    public ReadApi(IotaClient client)
    {
        _client = client;
    }

    public async Task<ObjectsPage> GetOwnedObjectsAsync(IotaAddress address, IotaObjectResponseQuery query = null, ObjectId cursor = null, int? limit = null)
    {
        throw new NotImplementedException();
    }

    public async Task<DynamicFieldPage> GetDynamicFieldsAsync(ObjectId objectId, ObjectId cursor = null, int? limit = null)
    {
        throw new NotImplementedException();
    }

    public async Task<IotaObjectResponse> GetDynamicFieldObjectAsync(ObjectId parentObjectId, DynamicFieldName name)
    {
        throw new NotImplementedException();
    }

    public async Task<IotaObjectResponse> GetDynamicFieldObjectV2Async(ObjectId parentObjectId, DynamicFieldName name, IotaObjectDataOptions options = null)
    {
        throw new NotImplementedException();
    }

    public async Task<IotaPastObjectResponse> TryGetParsedPastObjectAsync(ObjectId objectId, SequenceNumber version, IotaObjectDataOptions options)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<IotaPastObjectResponse>> TryMultiGetParsedPastObjectAsync(IEnumerable<IotaGetPastObjectRequest> pastObjects, IotaObjectDataOptions options)
    {
        throw new NotImplementedException();
    }

    public async Task<IotaObjectResponse> GetObjectWithOptionsAsync(ObjectId objectId, IotaObjectDataOptions options)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<IotaObjectResponse>> MultiGetObjectWithOptionsAsync(IEnumerable<ObjectId> objectIds, IotaObjectDataOptions options)
    {
        throw new NotImplementedException();
    }

    public async Task<byte[]> GetMoveObjectBcsAsync(ObjectId objectId)
    {
        throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public async Task<ulong> GetTotalTransactionBlocksAsync()
    {
        var response = await _client.InvokeRpcMethodAsync<ulong>("iota_getTotalTransactionBlocks").ConfigureAwait(false);
    
        return response;
    }

    public async Task<IotaTransactionBlockResponse> GetTransactionWithOptionsAsync(TransactionDigest digest, IotaTransactionBlockResponseOptions options)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<IotaTransactionBlockResponse>> MultiGetTransactionsWithOptionsAsync(IEnumerable<TransactionDigest> digests, IotaTransactionBlockResponseOptions options)
    {
        throw new NotImplementedException();
    }

    public async Task<TransactionBlocksPage> QueryTransactionBlocksAsync(IotaTransactionBlockResponseQuery query, TransactionDigest? cursor = null, int? limit = null, bool descendingOrder = false)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetChainIdentifierAsync()
    {
        var response = await _client.InvokeRpcMethodAsync<string>("iota_getChainIdentifier").ConfigureAwait(false);
        return response;
    }

    public async Task<Checkpoint> GetCheckpointAsync(CheckpointId id)
    {
        // Create a request object with only one property set based on input
        string? requestParam;

        // Use sequence number if provided
        requestParam = id.SequenceNumber.HasValue ? id.SequenceNumber.ToString() :
            // Use digest if provided
            id.Digest;

        var response = await _client.InvokeRpcMethodAsync<Checkpoint>("iota_getCheckpoint", requestParam).ConfigureAwait(false);
        return response;
    }

    /// <summary>
    /// Returns a paginated list of checkpoints
    /// </summary>
    /// <param name="cursor">An optional paging cursor. If provided, the query will start from the next item after the specified cursor.</param>
    /// <param name="limit">Maximum items returned per page. If not specified, defaults to the server's maximum limit.</param>
    /// <param name="descendingOrder">Query result ordering. False (default) for ascending order (oldest first), true for descending order.</param>
    /// <returns>A page of checkpoints</returns>
    public async Task<CheckpointPage> GetCheckpointsAsync(BigInteger? cursor = null, int? limit = null, bool descendingOrder = false)
    {
        // Create parameters array for the RPC call
        var parameters = new List<object>();
    
        // Add cursor if provided
        if (cursor.HasValue)
        {
            parameters.Add(cursor.Value.ToString());
        }
        else
        {
            parameters.Add(null);
        }
    
        // Add limit if provided
        if (limit.HasValue)
        {
            parameters.Add(limit.Value);
        }
        else
        {
            parameters.Add(null);
        }
    
        // Add ordering parameter
        parameters.Add(descendingOrder);
    
        // Make the RPC call
        var response = await _client.InvokeRpcMethodAsync<CheckpointPage>("iota_getCheckpoints", parameters.ToArray()
        ).ConfigureAwait(false);
    
        return response;
    }

    /// <inheritdoc/>
    public async Task<ulong> GetLatestCheckpointSequenceNumberAsync()
    {
        var response = await _client.InvokeRpcMethodAsync<ulong>("iota_getLatestCheckpointSequenceNumber").ConfigureAwait(false);
        return response;
    }

    public IAsyncEnumerable<IotaTransactionBlockResponse> GetTransactionsStreamAsync(IotaTransactionBlockResponseQuery query, TransactionDigest? cursor = null, bool descendingOrder = false)
    {
        throw new NotImplementedException();
    }

    public async Task<IAsyncEnumerable<IotaTransactionBlockEffects>> SubscribeTransactionAsync(TransactionFilter filter)
    {
        throw new NotImplementedException();
    }

    public async Task<IDictionary<string, IotaMoveNormalizedModule>> GetNormalizedMoveModulesByPackageAsync(ObjectId package)
    {
        throw new NotImplementedException();
    }

    public async Task<ulong> GetReferenceGasPriceAsync()
    {
        var response = await _client.InvokeRpcMethodAsync<ulong>("iotax_getReferenceGasPrice").ConfigureAwait(false);
    
        return response;
    }

    public async Task<DryRunTransactionBlockResponse> DryRunTransactionBlockAsync(TransactionData tx)
    {
        throw new NotImplementedException();
    }

    public async Task<DevInspectResults> DevInspectTransactionBlockAsync(IotaAddress senderAddress, TransactionKind tx, BigInteger? gasPrice = null,
        BigInteger? epoch = null, DevInspectArgs additionalArgs = null)
    {
        throw new NotImplementedException();
    }

    public async Task<ProtocolConfigResponse> GetProtocolConfigAsync(BigInteger? version = null)
    {
        // Create parameters array
        object[] parameters = version.HasValue ? new object[] { version.Value.ToString() } : Array.Empty<object>();
    
        var response = await _client.InvokeRpcMethodAsync<ProtocolConfigResponse>("iota_getProtocolConfig", parameters).ConfigureAwait(false);
    
        return response;
    }

    public async Task<IotaPastObjectResponse> TryGetObjectBeforeVersionAsync(ObjectId objectId, SequenceNumber version)
    {
        throw new NotImplementedException();
    }
}