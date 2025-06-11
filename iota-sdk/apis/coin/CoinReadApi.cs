using iota_sdk.model.coin;

namespace iota_sdk.apis.coin;

public class CoinReadApi : ICoinReadApi
{
    private readonly IotaClient _client;

    /// <summary>
    /// Creates a new instance of <see cref="CoinReadApi"/>
    /// </summary>
    /// <param name="client"></param>
    public CoinReadApi(IotaClient client)
    {
        _client = client;
    }

    /// <inheritdoc />
    public Task<List<Balance>> GetAllBalances(string address)
    {
        return _client.InvokeRpcMethod<List<Balance>>("iotax_getAllBalances", address);
    }

    /// <inheritdoc />
    public Task<CoinPage> GetAllCoins(string address, string? cursor = null, int? limit = null)
    {
        // Create the parameters array for the RPC call
        var parameters = new List<object> { address };

        // Add optional parameters if they are provided
        if (cursor != null)
        {
            parameters.Add(cursor);

            if (limit.HasValue)
            {
                parameters.Add(limit.Value);
            }
        }

        // Invoke the RPC method with the parameters
        return _client.InvokeRpcMethod<CoinPage>("iotax_getAllCoins", parameters.ToArray());
    }


    public Task<Balance> GetBalance(string address, string? coinType = null)
    {
        // Create the parameters array for the RPC call
        var parameters = new List<object> { address };

        // Add optional coinType parameter if it is provided
        if (!string.IsNullOrEmpty(coinType))
        {
            parameters.Add(coinType);
        }

        // Invoke the RPC method with the parameters
        return _client.InvokeRpcMethod<Balance>("iotax_getBalance", parameters.ToArray());
    }

    /// <inheritdoc />
    public Task<IotaCirculatingSupply> GetCirculatingSupply()
    {
        // This method doesn't require any parameters
        return _client.InvokeRpcMethod<IotaCirculatingSupply>("iotax_getCirculatingSupply");
    }

    /// <inheritdoc />
    public Task<CoinPage> GetCoins(string address, string? coinType = null, string? cursor = null, int? limit = null)
    {
        // Create the parameters array for the RPC call
        var parameters = new List<object> { address };

        // Add optional coinType parameter if it is provided
        if (!string.IsNullOrEmpty(coinType))
        {
            parameters.Add(coinType);
        }

        // Add optional cursor parameter if it is provided
        if (cursor != null)
        {
            parameters.Add(cursor);
        }

        // Add optional limit parameter if it is provided
        if (limit.HasValue)
        {
            parameters.Add(limit.Value);
        }

        // Invoke the RPC method with the parameters
        return _client.InvokeRpcMethod<CoinPage>("iotax_getCoins", parameters.ToArray());

    }

    /// <inheritdoc />
    public Task<CoinMetadata> GetCoinMetadata(string coinType)
    {
        // The coinType parameter is required
        if (string.IsNullOrEmpty(coinType))
        {
            throw new ArgumentNullException(nameof(coinType), "Coin type must be specified");
        }

        // Invoke the RPC method with the coinType parameter
        return _client.InvokeRpcMethod<CoinMetadata>("iotax_getCoinMetadata", coinType);
    }

    /// <inheritdoc />
    public Task<Supply> GetTotalSupply(string coinType)
    {
        // The coinType parameter is required
        if (string.IsNullOrEmpty(coinType))
        {
            throw new ArgumentNullException(nameof(coinType), "Coin type must be specified");
        }

        // Invoke the RPC method with the coinType parameter
        return _client.InvokeRpcMethod<Supply>("iotax_getTotalSupply", coinType);
    }
}