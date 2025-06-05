namespace iota_sdk
{
    /// <summary>
    /// Interface for building an IotaClient for connecting to the IOTA network.
    /// </summary>
    public interface IIotaClientBuilder
    {
        /// <summary>
        /// Set the request timeout to the specified duration.
        /// </summary>
        /// <param name="requestTimeout">The timeout duration</param>
        /// <returns>The builder instance</returns>
        IIotaClientBuilder RequestTimeout(TimeSpan requestTimeout);

        /// <summary>
        /// Set the max concurrent requests allowed.
        /// </summary>
        /// <param name="maxConcurrentRequests">Maximum number of concurrent requests</param>
        /// <returns>The builder instance</returns>
        IIotaClientBuilder MaxConcurrentRequests(int maxConcurrentRequests);

        /// <summary>
        /// Set the WebSocket URL for the IOTA network.
        /// </summary>
        /// <param name="url">The WebSocket URL</param>
        /// <returns>The builder instance</returns>
        IIotaClientBuilder WsUrl(string url);

        /// <summary>
        /// Set the WebSocket ping interval.
        /// </summary>
        /// <param name="duration">The ping interval duration</param>
        /// <returns>The builder instance</returns>
        IIotaClientBuilder WsPingInterval(TimeSpan duration);

        /// <summary>
        /// Set the basic auth credentials for the HTTP client.
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <returns>The builder instance</returns>
        IIotaClientBuilder BasicAuth(string username, string password);

        /// <summary>
        /// Build an IotaClient connected to the specified URL.
        /// </summary>
        /// <param name="httpUrl">The HTTP URL of the IOTA network</param>
        /// <returns>An IotaClient instance</returns>
        Task<IIotaClient> Build(string httpUrl);

        /// <summary>
        /// Build an IotaClient connected to the local development network.
        /// </summary>
        /// <returns>An IotaClient instance</returns>
        Task<IIotaClient> BuildLocalnet();

        /// <summary>
        /// Build an IotaClient connected to the IOTA devnet.
        /// </summary>
        /// <returns>An IotaClient instance</returns>
        Task<IIotaClient> BuildDevnet();

        /// <summary>
        /// Build an IotaClient connected to the IOTA testnet.
        /// </summary>
        /// <returns>An IotaClient instance</returns>
        Task<IIotaClient> BuildTestnet();

        /// <summary>
        /// Build an IotaClient connected to the IOTA mainnet.
        /// </summary>
        /// <returns>An IotaClient instance</returns>
        Task<IIotaClient> BuildMainnet();
    }
}