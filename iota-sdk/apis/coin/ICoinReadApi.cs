using iota_sdk.model.coin;

namespace iota_sdk.apis.coin;

/// <summary>
/// Defines methods that retrieve information from the IOTA network regarding
/// the coins owned by an address.
/// </summary>
public interface ICoinReadApi
{
    /// <summary>
    /// Get a list of balances grouped by coin type and owned by the given address.
    /// </summary>
    /// <param name="address">The IOTA address</param>
    /// <returns>List of balances for different coin types</returns>
    Task<List<Balance>> GetAllBalancesAsync(string address);

    /// <summary>
    /// Get all the coins for the given address regardless of coin type.
    /// Results are paginated.
    /// </summary>
    /// <param name="address">The  IOTA address</param>
    /// <param name="cursor">Optional pagination cursor</param>
    /// <param name="limit">Optional page size limit</param>
    /// <returns>Paginated coin results</returns>
    Task<CoinPage> GetAllCoinsAsync(string address, string? cursor = null, int? limit = null);

    /// <summary>
    /// Get the balance for the given address filtered by coin type.
    /// </summary>
    /// <param name="address">The IOTA address</param>
    /// <param name="coinType">Optional coin type (defaults to 0x2::iota::IOTA)</param>
    /// <returns>Balance information</returns>
    Task<Balance> GetBalanceAsync(string address, string? coinType = null);

    /// <summary>
    /// Get the IOTA circulating supply summary.
    /// </summary>
    /// <returns>IOTA circulating supply information</returns>
    Task<IotaCirculatingSupply> GetCirculatingSupplyAsync();

    /// <summary>
    /// Get coins for the given address filtered by coin type.
    /// Results are paginated.
    /// </summary>
    /// <param name="address">The IOTA address</param>
    /// <param name="coinType">Optional coin type (defaults to 0x2::iota::IOTA)</param>
    /// <param name="cursor">Optional pagination cursor</param>
    /// <param name="limit">Optional page size limit</param>
    /// <returns>Paginated coin results</returns>
    Task<CoinPage> GetCoinsAsync(string address, string? coinType = null, string? cursor = null, int? limit = null);

    /// <summary>
    /// Get the coin metadata (name, symbol, description, decimals, etc.) for a given coin type.
    /// </summary>
    /// <param name="coinType">The coin type</param>
    /// <returns>Coin metadata if available</returns>
    Task<CoinMetadata> GetCoinMetadataAsync(string coinType);

    /// <summary>
    /// Get the total supply for a given coin type.
    /// </summary>
    /// <param name="coinType">The coin type</param>
    /// <returns>CoinSupply information</returns>
    Task<CoinSupply> GetTotalSupplyAsync(string coinType);
}