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
    public Task<List<Balance>> GetAllBalancesAsync(string address)
    {
        return _client.InvokeRpcMethodAsync<List<Balance>>("iotax_getAllBalances", address);
    }

    /// <inheritdoc />
    public Task<CoinPage> GetAllCoinsAsync(string address, string? cursor = null, int? limit = null)
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
        return _client.InvokeRpcMethodAsync<CoinPage>("iotax_getAllCoins", parameters.ToArray());
    }

    /// <inheritdoc />
    public Task<Balance> GetBalanceAsync(string address, string? coinType = null)
    {
        // Create the parameters array for the RPC call
        var parameters = new List<object> { address };

        // Add optional coinType parameter if it is provided
        if (!string.IsNullOrEmpty(coinType))
        {
            parameters.Add(coinType);
        }

        // Invoke the RPC method with the parameters
        return _client.InvokeRpcMethodAsync<Balance>("iotax_getBalance", parameters.ToArray());
    }

    /// <inheritdoc />
    public Task<IotaCirculatingSupply> GetCirculatingSupplyAsync()
    {
        // This method doesn't require any parameters
        return _client.InvokeRpcMethodAsync<IotaCirculatingSupply>("iotax_getCirculatingSupply");
    }

    /// <inheritdoc />
    public Task<CoinPage> GetCoinsAsync(string address, string? coinType = null, string? cursor = null, int? limit = null)
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
        return _client.InvokeRpcMethodAsync<CoinPage>("iotax_getCoins", parameters.ToArray());

    }

    /// <inheritdoc />
    public Task<CoinMetadata> GetCoinMetadataAsync(string coinType)
    {
        // The coinType parameter is required
        if (string.IsNullOrEmpty(coinType))
        {
            throw new ArgumentNullException(nameof(coinType), "Coin type must be specified");
        }

        // Invoke the RPC method with the coinType parameter
        return _client.InvokeRpcMethodAsync<CoinMetadata>("iotax_getCoinMetadata", coinType);
    }

    /// <inheritdoc />
    public Task<CoinSupply> GetTotalSupplyAsync(string coinType)
    {
        // The coinType parameter is required
        if (string.IsNullOrEmpty(coinType))
        {
            throw new ArgumentNullException(nameof(coinType), "Coin type must be specified");
        }

        // Invoke the RPC method with the coinType parameter
        return _client.InvokeRpcMethodAsync<CoinSupply>("iotax_getTotalSupply", coinType);
    }
}