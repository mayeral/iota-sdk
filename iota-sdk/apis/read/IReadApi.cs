using iota_sdk.model;
using iota_sdk.model.read;
using System.Numerics;
using Iota.Sdk.Model.Read;
using IotaPastObjectResponse = iota_sdk.model.read.IotaPastObjectResponse;

namespace iota_sdk.apis.read
{
    /// <summary>
    /// Defines methods for retrieving data about objects and transactions.
    /// </summary>
    public interface IReadApi
    {
        /// <summary>
        /// Get the objects owned by the given address.
        /// Results are paginated.
        /// </summary>
        /// <param name="address">The address to query</param>
        /// <param name="query">Optional query parameters</param>
        /// <param name="cursor">Optional cursor for pagination</param>
        /// <param name="limit">Optional limit for results per page</param>
        /// <returns>A page of objects</returns>
        Task<ObjectsPage> GetOwnedObjectsAsync(IotaAddress address, IotaObjectResponseQuery query = null, ObjectId cursor = null, int? limit = null);

        /// <summary>
        /// Get the dynamic fields owned by the given ObjectId.
        /// Results are paginated.
        /// </summary>
        /// <param name="objectId">The object ID to query</param>
        /// <param name="cursor">Optional cursor for pagination</param>
        /// <param name="limit">Optional limit for results per page</param>
        /// <returns>A page of dynamic fields</returns>
        Task<DynamicFieldPage> GetDynamicFieldsAsync(ObjectId objectId, ObjectId cursor = null, int? limit = null);

        /// <summary>
        /// Get information for a specified dynamic field object by its parent object ID and field name.
        /// </summary>
        /// <param name="parentObjectId">The parent object ID</param>
        /// <param name="name">The dynamic field name</param>
        /// <returns>Object response</returns>
        Task<IotaObjectResponse> GetDynamicFieldObjectAsync(ObjectId parentObjectId, DynamicFieldName name);

        /// <summary>
        /// Get information for a specified dynamic field object by its parent object ID and field name with options.
        /// </summary>
        /// <param name="parentObjectId">The parent object ID</param>
        /// <param name="name">The dynamic field name</param>
        /// <param name="options">Object data options</param>
        /// <returns>Object response</returns>
        Task<IotaObjectResponse> GetDynamicFieldObjectV2Async(ObjectId parentObjectId, DynamicFieldName name, IotaObjectDataOptions options = null);

        /// <summary>
        /// Get a parsed past object and version for the provided object ID.
        /// </summary>
        /// <param name="objectId">The object ID</param>
        /// <param name="version">The version number</param>
        /// <param name="options">Object data options</param>
        /// <returns>Past object response</returns>
        Task<IotaPastObjectResponse> TryGetParsedPastObjectAsync(ObjectId objectId, SequenceNumber version, IotaObjectDataOptions options);

        /// <summary>
        /// Get a list of parsed past objects.
        /// </summary>
        /// <param name="pastObjects">List of past object requests</param>
        /// <param name="options">Object data options</param>
        /// <returns>List of past object responses</returns>
        Task<IEnumerable<IotaPastObjectResponse>> TryMultiGetParsedPastObjectAsync(IEnumerable<IotaGetPastObjectRequest> pastObjects, IotaObjectDataOptions options);

        /// <summary>
        /// Get an object by object ID with optional fields enabled by IotaObjectDataOptions.
        /// </summary>
        /// <param name="objectId">The object ID</param>
        /// <param name="options">Object data options</param>
        /// <returns>Object response</returns>
        Task<IotaObjectResponse> GetObjectAsync(ObjectId objectId, IotaObjectDataOptions options);

        /// <summary>
        /// Get a list of objects by their object IDs with optional fields enabled by IotaObjectDataOptions.
        /// </summary>
        /// <param name="objectIds">List of object IDs</param>
        /// <param name="options">Object data options</param>
        /// <returns>List of object responses</returns>
        Task<IEnumerable<IotaObjectResponse>> MultiGetObjectWithOptionsAsync(IEnumerable<ObjectId> objectIds, IotaObjectDataOptions options);

        /// <summary>
        /// Get a BCS serialized object's bytes by object ID.
        /// </summary>
        /// <param name="objectId">The object ID</param>
        /// <returns>Serialized object bytes</returns>
        Task<byte[]> GetMoveObjectBcsAsync(ObjectId objectId);

        /// <summary>
        /// Get the total number of transaction blocks known to server.
        /// </summary>
        /// <returns>Total transaction blocks count</returns>
        Task<ulong> GetTotalTransactionBlocksAsync();

        /// <summary>
        /// Get a transaction and its effects by its digest with optional fields.
        /// </summary>
        /// <param name="digest">Transaction digest</param>
        /// <param name="options">Transaction response options</param>
        /// <returns>Transaction block response</returns>
        Task<IotaTransactionBlockResponse> GetTransactionWithOptionsAsync(TransactionDigest digest, IotaTransactionBlockResponseOptions options);

        /// <summary>
        /// Get a list of transactions and their effects by their digests with optional fields.
        /// </summary>
        /// <param name="digests">List of transaction digests</param>
        /// <param name="options">Transaction response options</param>
        /// <returns>List of transaction block responses</returns>
        Task<IEnumerable<IotaTransactionBlockResponse>> MultiGetTransactionsWithOptionsAsync(IEnumerable<TransactionDigest> digests, IotaTransactionBlockResponseOptions options);

        /// <summary>
        /// Get filtered transaction blocks information. Results are paginated.
        /// </summary>
        /// <param name="query">Query parameters</param>
        /// <param name="cursor">Optional cursor for pagination</param>
        /// <param name="limit">Optional limit for results per page</param>
        /// <param name="descendingOrder">Whether to order results in descending order</param>
        /// <returns>Page of transaction blocks</returns>
        Task<TransactionBlocksPage> QueryTransactionBlocksAsync(IotaTransactionBlockResponseQuery query, TransactionDigest? cursor = null, int? limit = null, bool descendingOrder = false);

        /// <summary>
        /// Get the first four bytes of the chain's genesis checkpoint digest in hex format.
        /// </summary>
        /// <returns>Chain identifier</returns>
        Task<string> GetChainIdentifierAsync();

        /// <summary>
        /// Get a checkpoint by its ID.
        /// </summary>
        /// <param name="id">Checkpoint ID</param>
        /// <returns>Checkpoint</returns>
        Task<Checkpoint> GetCheckpointAsync(CheckpointId id);

        /// <summary>
        /// Return a list of checkpoints. Results are paginated.
        /// </summary>
        /// <param name="cursor">Optional cursor for pagination</param>
        /// <param name="limit">Optional limit for results per page</param>
        /// <param name="descendingOrder">Whether to order results in descending order</param>
        /// <returns>Page of checkpoints</returns>
        Task<CheckpointPage> GetCheckpointsAsync(BigInteger? cursor = null, int? limit = null, bool descendingOrder = false);

        /// <summary>
        /// Get the sequence number of the latest checkpoint that has been executed.
        /// </summary>
        /// <returns>Latest checkpoint sequence number</returns>
        Task<ulong> GetLatestCheckpointSequenceNumberAsync();

        /// <summary>
        /// Get a stream of transactions.
        /// </summary>
        /// <param name="query">Query parameters</param>
        /// <param name="cursor">Optional cursor for pagination</param>
        /// <param name="descendingOrder">Whether to order results in descending order</param>
        /// <returns>Stream of transaction block responses</returns>
        IAsyncEnumerable<IotaTransactionBlockResponse> GetTransactionsStreamAsync(IotaTransactionBlockResponseQuery query, TransactionDigest? cursor = null, bool descendingOrder = false);

        /// <summary>
        /// Subscribe to a stream of transactions.
        /// This is only available through WebSockets.
        /// </summary>
        /// <param name="filter">Transaction filter</param>
        /// <returns>Stream of transaction block effects</returns>
        Task<IAsyncEnumerable<IotaTransactionBlockEffects>> SubscribeTransactionAsync(TransactionFilter filter);

        /// <summary>
        /// Get move modules by package ID, keyed by name.
        /// </summary>
        /// <param name="package">The package object ID</param>
        /// <returns>Dictionary of module name to normalized module</returns>
        Task<IDictionary<string, IotaMoveNormalizedModule>> GetNormalizedMoveModulesByPackageAsync(ObjectId package);

        /// <summary>
        /// Get the reference gas price.
        /// </summary>
        /// <returns>Reference gas price</returns>
        Task<ulong> GetReferenceGasPriceAsync();

        /// <summary>
        /// Dry run a transaction block given the provided transaction data.
        /// </summary>
        /// <param name="tx">Transaction data</param>
        /// <returns>Dry run transaction block response</returns>
        Task<DryRunTransactionBlockResponse> DryRunTransactionBlockAsync(TransactionData tx);

        /// <summary>
        /// Use this function to inspect the current state of the network by running
        /// a programmable transaction block without committing its effects on chain.
        /// </summary>
        /// <param name="senderAddress">Sender address</param>
        /// <param name="tx">Transaction kind</param>
        /// <param name="gasPrice">Optional gas price</param>
        /// <param name="epoch">Optional epoch</param>
        /// <param name="additionalArgs">Optional additional arguments</param>
        /// <returns>Dev inspect results</returns>
        Task<DevInspectResults> DevInspectTransactionBlockAsync(IotaAddress senderAddress, TransactionKind tx, BigInteger? gasPrice = null, BigInteger? epoch = null, DevInspectArgs additionalArgs = null);

        /// <summary>
        /// Get the protocol config by version.
        /// The version defaults to the current version.
        /// </summary>
        /// <param name="version">Optional protocol version</param>
        /// <returns>Protocol config response</returns>
        Task<ProtocolConfigResponse> GetProtocolConfigAsync(BigInteger? version = null);

        /// <summary>
        /// Get an object by ID before the given version.
        /// </summary>
        /// <param name="objectId">The object ID</param>
        /// <param name="version">The version number</param>
        /// <returns>Past object response</returns>
        Task<IotaPastObjectResponse> TryGetObjectBeforeVersionAsync(ObjectId objectId, SequenceNumber version);
    }
}