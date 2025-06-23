using iota_sdk.model.governance;
using iota_sdk.model.read;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iota_sdk.apis.governance
{
    /// <summary>
    /// Implementation of the IGovernanceApi interface
    /// </summary>
    public class GovernanceApi : IGovernanceApi
    {
        private readonly IotaClient _client;
        private readonly bool _iotaSystemStateV2Support;

        /// <summary>
        /// Creates a new instance of <see cref="GovernanceApi"/>
        /// </summary>
        /// <param name="client">The IOTA client to use for API calls</param>
        public GovernanceApi(IotaClient client)
        {
            _client = client;
            _iotaSystemStateV2Support = client.ServerInfo.IotaSystemStateV2Support;
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
            return _client.InvokeRpcMethodAsync<IEnumerable<DelegatedStake>>("iotax_getStakes", address);
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
            return _client.InvokeRpcMethodAsync<IEnumerable<DelegatedTimelockedStake>>("iotax_getTimelockedStakes", address);
        }

        /// <inheritdoc />
        public Task<IEnumerable<DelegatedStake>> GetStakesByIdsAsync(string[] stakedIotaIds)
        {
            // Validate input
            if (stakedIotaIds == null || stakedIotaIds.Length == 0)
            {
                throw new ArgumentNullException(nameof(stakedIotaIds), "Staked IOTA IDs must be specified");
            }

            // Convert ObjectIds to strings
            string[] stakedIotaIdStrings = stakedIotaIds.Select(id => id.ToString()).ToArray();

            // Invoke the RPC method with the array of ID strings
            return _client.InvokeRpcMethodAsync<IEnumerable<DelegatedStake>>("iotax_getStakesByIds", new object[] { stakedIotaIdStrings });
        }

        /// <inheritdoc />
        public Task<IEnumerable<DelegatedTimelockedStake>> GetTimelockedStakesByIdsAsync(string[] timelockedStakedIotaIds)
        {
            // Validate input
            if (timelockedStakedIotaIds == null || timelockedStakedIotaIds.Length == 0)
            {
                throw new ArgumentNullException(nameof(timelockedStakedIotaIds), "Timelocked staked IOTA IDs must be specified");
            }

            // Convert ObjectIds to strings
            string[] timelockedStakedIotaIdStrings = timelockedStakedIotaIds.Select(id => id.ToString()).ToArray();

            // Invoke the RPC method with the array of ID strings
            return _client.InvokeRpcMethodAsync<IEnumerable<DelegatedTimelockedStake>>("iotax_getTimelockedStakesByIds", new object[] { timelockedStakedIotaIdStrings });
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
            return _client.InvokeRpcMethodAsync<IotaCommittee>("iotax_getCommitteeInfo", parameters.Count > 0 ? parameters.ToArray() : null);
        }

        /// <inheritdoc />
        public async Task<IotaSystemStateSummary?> GetLatestIotaSystemStateAsync()
        {
            if (_iotaSystemStateV2Support)
            {
                return await GetlatestIotaSystemStateV2Async().ConfigureAwait(false);
            }
            else
            {
                return await GetlatestIotaSystemStateV1Async().ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task<IotaSystemStateSummary?> GetlatestIotaSystemStateV1Async()
        {
                var response = await _client.InvokeRpcMethodAsync<JObject>("iotax_getLatestIotaSystemState").ConfigureAwait(false);

                return JsonConvert.DeserializeObject<IotaSystemStateSummary>(response.ToString());
        }

        public async Task<IotaSystemStateSummary?> GetlatestIotaSystemStateV2Async()
        {
            if(_iotaSystemStateV2Support)
            {
                var response = await _client.InvokeRpcMethodAsync<JObject>("iotax_getLatestIotaSystemStateV2").ConfigureAwait(false);

                return JsonConvert.DeserializeObject<IotaSystemStateSummary>(response.ToString());

            }
            else
            {
                throw new InvalidOperationException("The protocol version does not support IOTA System State V2");
            }
        }

        /// <inheritdoc />
        public Task<ulong> GetReferenceGasPriceAsync()
        {
            // This method doesn't require any parameters
            return _client.InvokeRpcMethodAsync<ulong>("iotax_getReferenceGasPrice");
        }

        public Task<ValidatorApys> GetValidatorsApyAsync()
        {
            // This method doesn't require any parameters
            return _client.InvokeRpcMethodAsync<ValidatorApys>("iotax_getValidatorsApy");
        }
    }
}