using iota_sdk.model.governance;

namespace iota_sdk.apis.governance
{
    /// <summary>
    /// Implementation of the IGovernanceApi interface
    /// </summary>
    public class GovernanceApi : IGovernanceApi
    {
        private readonly IotaClient _client;

        /// <summary>
        /// Creates a new instance of <see cref="GovernanceApi"/>
        /// </summary>
        /// <param name="client">The IOTA client to use for API calls</param>
        public GovernanceApi(IotaClient client)
        {
            _client = client;
        }

        /// <inheritdoc />
        public Task<IEnumerable<DelegatedStake>> GetStakesAsync(string address)
        {
            // The address parameter is required
            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException(nameof(address), "Address must be specified");
            }

            // Invoke the RPC method with the address parameter
            return _client.InvokeRpcMethod<IEnumerable<DelegatedStake>>("iotax_getStakes", address);
        }

        /// <inheritdoc />
        public Task<IEnumerable<DelegatedTimelockedStake>> GetTimelockedStakesAsync(string address)
        {
            // The address parameter is required
            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException(nameof(address), "Address must be specified");
            }

            // Invoke the RPC method with the address parameter
            return _client.InvokeRpcMethod<IEnumerable<DelegatedTimelockedStake>>("iotax_getTimelockedStakes", address);
        }

        /// <inheritdoc />
        public Task<IotaCommittee> GetCommitteeInfoAsync(System.Numerics.BigInteger? epoch = null)
        {
            // Create parameters list
            var parameters = new List<object>();

            // Add optional epoch parameter if provided
            if (epoch.HasValue)
            {
                parameters.Add(epoch.Value.ToString());
            }

            // Invoke the RPC method with optional epoch parameter
            return _client.InvokeRpcMethod<IotaCommittee>("iotax_getCommitteeInfo", parameters.Count > 0 ? parameters.ToArray() : null);
        }

        /// <inheritdoc />
        public Task<IotaSystemStateSummary> GetLatestIotaSystemStateAsync()
        {
            // This method doesn't require any parameters
            return _client.InvokeRpcMethod<IotaSystemStateSummary>("iotax_getLatestIotaSystemState");
        }

        /// <inheritdoc />
        public Task<ulong> GetReferenceGasPriceAsync()
        {
            // This method doesn't require any parameters
            return _client.InvokeRpcMethod<ulong>("iotax_getReferenceGasPrice");
        }

        public async Task<ValidatorApys> GetValidatorsApyAsync()
        {
            throw new NotImplementedException();
        }
    }
}