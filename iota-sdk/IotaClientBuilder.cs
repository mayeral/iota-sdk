using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using StreamJsonRpc;
using System.Net.Http.Headers;
using System.Text;

namespace iota_sdk
{
    public class IotaClientBuilder : IIotaClientBuilder
    {
        private TimeSpan _requestTimeout = TimeSpan.FromSeconds(60);
        private int? _maxConcurrentRequests = null;
        private string? _wsUrl = null;
        private TimeSpan? _wsPingInterval = null;
        private (string username, string password)? _basicAuth = null;

        // Constants for network URLs
        private const string IOTA_LOCAL_NETWORK_URL = "http://127.0.0.1:9000";
        private const string IOTA_DEVNET_URL = "https://api.devnet.iota.cafe";
        private const string IOTA_TESTNET_URL = "https://api.testnet.iota.cafe";
        private const string IOTA_MAINNET_URL = "https://api.mainnet.iota.cafe";

        public IIotaClientBuilder RequestTimeout(TimeSpan requestTimeout)
        {
            _requestTimeout = requestTimeout;
            return this;
        }

        public IIotaClientBuilder MaxConcurrentRequests(int maxConcurrentRequests)
        {
            _maxConcurrentRequests = maxConcurrentRequests;
            return this;
        }

        public IIotaClientBuilder WsUrl(string url)
        {
            _wsUrl = url;
            return this;
        }

        public IIotaClientBuilder WsPingInterval(TimeSpan duration)
        {
            _wsPingInterval = duration;
            return this;
        }

        public IIotaClientBuilder BasicAuth(string username, string password)
        {
            _basicAuth = (username, password);
            return this;
        }

        public async Task<IIotaClient> Build(string httpUrl)
        {
            // Configure HTTP client with headers and authentication
            var httpClient = CreateConfiguredHttpClient();

            // Create JSON-RPC connection
            var endpoint = new Uri(httpUrl);
            var messageHandler = new HttpClientMessageHandler(httpClient, endpoint);
            var formatter = new JsonMessageFormatter();
            var jsonRpc = new JsonRpc(messageHandler, formatter);
            jsonRpc.StartListening();

            // Get server information
            var serverInfo = await GetServerInfo(jsonRpc);

            return new IotaClient(jsonRpc, serverInfo);
        }

        private HttpClient CreateConfiguredHttpClient()
        {
            var handler = new HttpClientHandler();
            var httpClient = new HttpClient(handler);

            // Set client version and type headers
            var clientVersion = GetType().Assembly.GetName().Version?.ToString() ?? "0.0.0";
            httpClient.DefaultRequestHeaders.Add("Client-Target-Api-Version", clientVersion);
            httpClient.DefaultRequestHeaders.Add("Client-Sdk-Version", clientVersion);
            httpClient.DefaultRequestHeaders.Add("Client-Sdk-Type", "csharp");

            // Add Accept header for JSON
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Configure timeout
            httpClient.Timeout = _requestTimeout;

            // Add basic authentication if provided
            if (_basicAuth.HasValue)
            {
                var (username, password) = _basicAuth.Value;
                var authBytes = Encoding.ASCII.GetBytes($"{username}:{password}");
                var authHeader = Convert.ToBase64String(authBytes);
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", authHeader);
            }

            return httpClient;
        }

        public async Task<IIotaClient> BuildLocalnet()
        {
            return await Build(IOTA_LOCAL_NETWORK_URL);
        }

        public async Task<IIotaClient> BuildDevnet()
        {
            return await Build(IOTA_DEVNET_URL);
        }

        public async Task<IIotaClient> BuildTestnet()
        {
            return await Build(IOTA_TESTNET_URL);
        }

        public async Task<IIotaClient> BuildMainnet()
        {
            return await Build(IOTA_MAINNET_URL);
        }

        private async Task<ServerInfo> GetServerInfo(JsonRpc rpcClient)
        {
            // Use the JSON-RPC client to make the rpc.discover call
            var response = await rpcClient.InvokeAsync<dynamic>("rpc.discover").ConfigureAwait(false);

            // Parse the response to extract server info
            var serverInfo = new ServerInfo
            {
                Version = response.info.version,
                RpcMethods = new List<string>(),
                Subscriptions = new List<string>()
            };

            // Parse methods from response
            foreach (var method in response.methods)
            {
                serverInfo.RpcMethods.Add(method.name.ToString());
            }

            // Check for specific API support
            serverInfo.IotaSystemStateV2Support =
                serverInfo.RpcMethods.Contains("iotax_getLatestIotaSystemStateV2");

            return serverInfo;
        }

        public class ServerInfo
        {
            public string Version { get; set; } = string.Empty;
            public List<string> RpcMethods { get; set; } = new List<string>();
            public List<string> Subscriptions { get; set; } = new List<string>();
            public bool IotaSystemStateV2Support { get; set; }
        }
    }
}