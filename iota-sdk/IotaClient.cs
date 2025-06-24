using iota_sdk.apis.coin;
using iota_sdk.apis.@event;
using iota_sdk.apis.governance;
using iota_sdk.apis.read;
using StreamJsonRpc;

namespace iota_sdk;

/// <summary>
/// Implementation of IIotaClient for interacting with the IOTA network
/// </summary>
public class IotaClient : IIotaClient
{
    private readonly JsonRpc _jsonRpc;
    private readonly string _version;
    private readonly List<string> _rpcMethods;
    private readonly List<string> _subscriptions;

    // APIs
    private readonly ICoinReadApi _coinReadApi;
    private readonly IEventApi _eventApi;
    private readonly IGovernanceApi _governanceApi;
    private readonly IReadApi _readApi;


    internal IotaClient(JsonRpc jsonRpc, IotaClientBuilder.ServerInfo serverInfo)
    {
        _jsonRpc = jsonRpc;
        ServerInfo = serverInfo;
        _version = serverInfo.Version;
        _rpcMethods = serverInfo.RpcMethods;
        _subscriptions = serverInfo.Subscriptions;
        _coinReadApi = new CoinReadApi(this);
        _eventApi = new EventApi(this);
        _governanceApi = new GovernanceApi(this);
        _readApi = new ReadApi(this);
    }

    public IotaClientBuilder.ServerInfo ServerInfo { get; set; }

    public string ApiVersion() => _version;

    public List<string> AvailableRpcMethods() => _rpcMethods;

    public List<string> AvailableSubscriptions() => _subscriptions;

    /// <summary>
    /// Checks the API version against the client version
    /// </summary>
    /// <exception cref="Exception"></exception>
    public Task CheckApiVersionAsync()
    {
        var clientVersion = GetType().Assembly.GetName().Version?.ToString(3) ?? "0.0.0";

        if (_version != clientVersion) throw new Exception($"API version mismatch, current version is {_version} but got nuget package version {clientVersion}");
        return Task.CompletedTask;
    }

    public ICoinReadApi CoinReadApi()
    {
        return _coinReadApi;
    }

    public IEventApi EventApi()
    {
        return _eventApi;
    }

    public IGovernanceApi GovernanceApi()
    {
        return _governanceApi;
    }


    public IReadApi ReadApi()
    {
        return _readApi;
    }

    public void Dispose()
    {
        _jsonRpc.Dispose();
    }

    /// <summary>
    /// Internal method to make RPC calls - can be used by API implementations
    /// </summary>
    /// <param name="method"></param>
    /// <param name="parameters"></param>
    /// <typeparam name="T"></typeparam>
    public Task<T> InvokeRpcMethodAsync<T>(string method, params object[]? parameters)
    {
        return _jsonRpc.InvokeAsync<T>(method, parameters);
    }
}