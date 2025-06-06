using iota_sdk.model;

namespace iota_sdk.apis;

public interface ICoinReadApi
{
    Task<List<Balance>> GetAllBalances(string address);
}

// Implementation of the Coin Read API
public class CoinReadApi : ICoinReadApi
{
    private readonly IotaClient _client;

    public CoinReadApi(IotaClient client)
    {
        _client = client;
    }

    /// <summary>
    /// Return the total coin balance for all coin types owned by the address owner.
    /// </summary>
    /// <param name="address">The owner's IOTA address</param>
    /// <returns>List of balances for different coin types</returns>
    public async Task<List<Balance>> GetAllBalances(string address)
    {
        return await _client.InvokeRpcMethod<List<Balance>>("iotax_getAllBalances", address);
    }
}

// Balance model that matches the JSON-RPC response