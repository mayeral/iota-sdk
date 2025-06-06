using iota_sdk.apis;
using StreamJsonRpc;

namespace iota_sdk;

/// <summary>
/// Implementation of IIotaClient for interacting with the IOTA network
/// </summary>
public class IotaClient : IIotaClient, IDisposable
{
    private readonly JsonRpc _jsonRpc;
    private readonly string _version;
    private readonly List<string> _rpcMethods;
    private readonly List<string> _subscriptions;
    private readonly bool _iotaSystemStateV2Support;
    private readonly ICoinReadApi _coinReadApi;


    internal IotaClient(JsonRpc jsonRpc, IotaClientBuilder.ServerInfo serverInfo)
    {
        _jsonRpc = jsonRpc;
        _version = serverInfo.Version;
        _rpcMethods = serverInfo.RpcMethods;
        _subscriptions = serverInfo.Subscriptions;
        _iotaSystemStateV2Support = serverInfo.IotaSystemStateV2Support;
        _coinReadApi = new CoinReadApi(this);
    }

    public string ApiVersion() => _version;

    public List<string> AvailableRpcMethods() => _rpcMethods;

    public List<string> AvailableSubscriptions() => _subscriptions;

    public async Task CheckApiVersion()
    {
        var clientVersion = GetType().Assembly.GetName().Version?.ToString() ?? "0.0.0";

        if (_version != clientVersion) throw new Exception($"API version mismatch, expected {_version} but got {clientVersion}");
        //return _version == clientVersion;
    }

    public ICoinReadApi CoinReadApi()
    {
        return _coinReadApi;
    }

    public IEventApi EventApi()
    {
        throw new NotImplementedException();
    }

    public IGovernanceApi GovernanceApi()
    {
        throw new NotImplementedException();
    }

    public IQuorumDriverApi QuorumDriverApi()
    {
        throw new NotImplementedException();
    }

    public IReadApi ReadApi()
    {
        throw new NotImplementedException();
    }

    public ITransactionBuilder TransactionBuilder()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        _jsonRpc.Dispose();
    }

    // Internal method to make RPC calls - can be used by API implementations
    internal async Task<T> InvokeRpcMethod<T>(string method, params object[] parameters)
    {
        return await _jsonRpc.InvokeAsync<T>(method, parameters);
    }
}