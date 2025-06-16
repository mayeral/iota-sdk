using System.Numerics;
using Iota.Model.Read;
using iota_sdk.apis.read.m;
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

    public async Task<ObjectsPage> GetOwnedObjectsAsync(IotaAddress address, IotaObjectResponseQuery query = null, ObjectID cursor = null, int? limit = null)
    {
        throw new NotImplementedException();
    }

    public async Task<DynamicFieldPage> GetDynamicFieldsAsync(ObjectID objectId, ObjectID cursor = null, int? limit = null)
    {
        throw new NotImplementedException();
    }

    public async Task<IotaObjectResponse> GetDynamicFieldObjectAsync(ObjectID parentObjectId, DynamicFieldName name)
    {
        throw new NotImplementedException();
    }

    public async Task<IotaObjectResponse> GetDynamicFieldObjectV2Async(ObjectID parentObjectId, DynamicFieldName name, IotaObjectDataOptions options = null)
    {
        throw new NotImplementedException();
    }

    public async Task<IotaPastObjectResponse> TryGetParsedPastObjectAsync(ObjectID objectId, SequenceNumber version, IotaObjectDataOptions options)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<IotaPastObjectResponse>> TryMultiGetParsedPastObjectAsync(IEnumerable<IotaGetPastObjectRequest> pastObjects, IotaObjectDataOptions options)
    {
        throw new NotImplementedException();
    }

    public async Task<IotaObjectResponse> GetObjectWithOptionsAsync(ObjectID objectId, IotaObjectDataOptions options)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<IotaObjectResponse>> MultiGetObjectWithOptionsAsync(IEnumerable<ObjectID> objectIds, IotaObjectDataOptions options)
    {
        throw new NotImplementedException();
    }

    public async Task<byte[]> GetMoveObjectBcsAsync(ObjectID objectId)
    {
        throw new NotImplementedException();
    }

    public async Task<ulong> GetTotalTransactionBlocksAsync()
    {
        throw new NotImplementedException();
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

    public async Task<CheckpointPage> GetCheckpointsAsync(BigInteger? cursor = null, int? limit = null, bool descendingOrder = false)
    {
        throw new NotImplementedException();
    }

    public async Task<CheckpointSequenceNumber> GetLatestCheckpointSequenceNumberAsync()
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<IotaTransactionBlockResponse> GetTransactionsStreamAsync(IotaTransactionBlockResponseQuery query, TransactionDigest? cursor = null, bool descendingOrder = false)
    {
        throw new NotImplementedException();
    }

    public async Task<IAsyncEnumerable<IotaTransactionBlockEffects>> SubscribeTransactionAsync(TransactionFilter filter)
    {
        throw new NotImplementedException();
    }

    public async Task<IDictionary<string, IotaMoveNormalizedModule>> GetNormalizedMoveModulesByPackageAsync(ObjectID package)
    {
        throw new NotImplementedException();
    }

    public async Task<ulong> GetReferenceGasPriceAsync()
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public async Task<IotaPastObjectResponse> TryGetObjectBeforeVersionAsync(ObjectID objectId, SequenceNumber version)
    {
        throw new NotImplementedException();
    }
}