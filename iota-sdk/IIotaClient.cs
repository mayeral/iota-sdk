using iota_sdk.apis;
using iota_sdk.apis.@event;
using iota_sdk.apis.governance;
using iota_sdk.apis.read;

namespace iota_sdk
{
    /// <summary>
    /// Interface that provides all the necessary abstractions for interacting with the IOTA network.
    /// </summary>
    public interface IIotaClient : IDisposable
    {
        /// <summary>
        /// Returns a list of RPC methods supported by the node the client is connected to.
        /// </summary>
        /// <returns>List of supported RPC methods</returns>
        List<string> AvailableRpcMethods();

        /// <summary>
        /// Returns a list of streaming/subscription APIs supported by the node the client is connected to.
        /// </summary>
        /// <returns>List of supported subscriptions</returns>
        List<string> AvailableSubscriptions();

        /// <summary>
        /// Returns the API version information as a string.
        /// The format of this string is major.minor.patch, e.g., 1.6.0.
        /// </summary>
        /// <returns>The API version</returns>
        string ApiVersion();

        /// <summary>
        /// Verifies if the API version matches the server version and returns an error if they do not match.
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
        Task CheckApiVersion();

        /// <summary>
        /// Returns the coin read API for accessing coin-related functionality.
        /// </summary>
        /// <returns>Coin read API</returns>
        ICoinReadApi CoinReadApi();

        /// <summary>
        /// Returns the event API for accessing event-related functionality.
        /// </summary>
        /// <returns>Event API</returns>
        IEventApi EventApi();

        /// <summary>
        /// Returns the governance API for accessing governance-related functionality.
        /// </summary>
        /// <returns>Governance API</returns>
        IGovernanceApi GovernanceApi();

        /// <summary>
        /// Returns the read API for retrieving data about different objects and transactions.
        /// </summary>
        /// <returns>Read API</returns>
        IReadApi ReadApi();

        // TODO
        ///// <summary>
        ///// Returns the transaction builder API for building transactions.
        ///// </summary>
        ///// <returns>Transaction builder API</returns>
        //ITransactionBuilder TransactionBuilder();

        ///// <summary>
        ///// Returns the quorum driver API for accessing transaction execution functionality.
        ///// </summary>
        ///// <returns>Quorum driver API</returns>
        //IQuorumDriverApi QuorumDriverApi();
    }
}