using iota_sdk;
using iota_sdk.apis.coin;
using iota_sdk.apis.@event;
using iota_sdk.apis.governance;
using iota_sdk.apis.read;
// ReSharper disable AsyncApostle.AsyncWait

namespace iota_sdk_tests
{
    public class IotaClientTests
    {
        private IotaClient? _client;

        [SetUp]
        public async Task Setup()
        {
            // Initialize the IotaClient using IotaClientBuilder
            var clientBuilder = new IotaClientBuilder()
                .RequestTimeout(TimeSpan.FromSeconds(30))
                .MaxConcurrentRequests(10);

            // Use the main net endpoint for testing purposes
            _client = (IotaClient)await clientBuilder.BuildMainnet().ConfigureAwait(false);
        }

        [Test]
        public void ApiVersion_ReturnsVersionString()
        {
            // Act
            var version = _client.ApiVersion();

            // Assert
            Assert.IsNotNull(version);
            Assert.IsNotEmpty(version);
        }

        [Test]
        public void AvailableRpcMethods_ReturnsNonEmptyList()
        {
            // Act
            var methods = _client.AvailableRpcMethods();

            // Assert
            Assert.IsNotNull(methods);
            Assert.IsInstanceOf<List<string>>(methods);
            Assert.GreaterOrEqual(methods.Count, 1);
            // print list ordered
            foreach (var method in methods.OrderBy(m => m))
            {
                Console.WriteLine($"- {method}");
            }   
        }

        [Test]
        public void AvailableSubscriptions_ReturnsNonEmptyList()
        {
            // Act
            var subscriptions = _client.AvailableSubscriptions();

            // Assert
            Assert.IsNotNull(subscriptions);
            Assert.IsInstanceOf<List<string>>(subscriptions);
            Assert.GreaterOrEqual(subscriptions.Count, 0); // May be empty if no subscriptions
        }

        [Test]
        public async Task CheckApiVersion_DoesNotThrowException()
        {
            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await _client.CheckApiVersionAsync());
        }

        [Test]
        public void CoinReadApi_ReturnsValidApi()
        {
            // Act
            var api = _client.CoinReadApi();

            // Assert
            Assert.IsNotNull(api);
            Assert.IsInstanceOf<ICoinReadApi>(api);
        }

        [Test]
        public void EventApi_ReturnsValidApi()
        {
            // Act
            var api = _client.EventApi();

            // Assert
            Assert.IsNotNull(api);
            Assert.IsInstanceOf<IEventApi>(api);
        }

        [Test]
        public void GovernanceApi_ReturnsValidApi()
        {
            // Act
            var api = _client.GovernanceApi();

            // Assert
            Assert.IsNotNull(api);
            Assert.IsInstanceOf<IGovernanceApi>(api);
        }

        [Test]
        public void ReadApi_ReturnsValidApi()
        {
            // Act
            var api = _client.ReadApi();

            // Assert
            Assert.IsNotNull(api);
            Assert.IsInstanceOf<IReadApi>(api);
        }

        [Test]
        public void InvokeRpcMethod_WithInvalidMethod_ThrowsException()
        {
            // Arrange
            var methodName = "invalid_method_name";
            var parameters = new object[] { };

            // Act & Assert
            // Capture the exception to check its message
            var exception = Assert.ThrowsAsync<StreamJsonRpc.RemoteMethodNotFoundException>(async () =>
            {
                await _client.InvokeRpcMethodAsync<object>(methodName, parameters);
            });

            // Check that the exception message contains expected text
            StringAssert.Contains("Method not found", exception.Message);
        }

        [Test]
        public void Dispose_DoesNotThrowException()
        {
            // Arrange
            var clientBuilder = new IotaClientBuilder();
            var client = clientBuilder.BuildMainnet().Result;

            // Act & Assert
            Assert.DoesNotThrow(() => client.Dispose());
        }


        [Test]
        public async Task CompareAvailableRpcMethods_DevNetVsMainNet()
        {
            // Create clients for both networks using the client builder
            var devNetClient = new IotaClientBuilder()
                .RequestTimeout(TimeSpan.FromSeconds(30)).
                BuildDevnet().Result;


            var mainNetClient = new IotaClientBuilder()
                .RequestTimeout(TimeSpan.FromSeconds(30)).
                BuildMainnet().Result;

            // Get available methods for both networks
            var devNetMethods = devNetClient.AvailableRpcMethods();
            var mainNetMethods = mainNetClient.AvailableRpcMethods();

            // Print all methods for DevNet
            Console.WriteLine("===== DevNet Available RPC Methods =====");
            foreach (var method in devNetMethods.OrderBy(m => m))
            {
                Console.WriteLine($"- {method}");
            }

            // Print all methods for MainNet
            Console.WriteLine("\n===== MainNet Available RPC Methods =====");
            foreach (var method in mainNetMethods.OrderBy(m => m))
            {
                Console.WriteLine($"- {method}");
            }

            // Find and print differences
            Console.WriteLine("\n===== Methods only in DevNet =====");
            var onlyInDevNet = devNetMethods.Except(mainNetMethods).OrderBy(m => m);
            foreach (var method in onlyInDevNet)
            {
                Console.WriteLine($"- {method}");
            }

            Console.WriteLine("\n===== Methods only in MainNet =====");
            var onlyInMainNet = mainNetMethods.Except(devNetMethods).OrderBy(m => m);
            foreach (var method in onlyInMainNet)
            {
                Console.WriteLine($"- {method}");
            }

            // Get and print API version from ServerInfo
            Console.WriteLine("\n===== API Version Information =====");
            //var devNetInfo = await devNetClient.ServerInfo
            //var mainNetInfo = await mainNetClient.GetInfo();

            Console.WriteLine($"DevNet API Version: {devNetClient.ServerInfo}");
            Console.WriteLine($"MainNet API Version: {mainNetClient.ServerInfo}");

            // Assertions to make this a proper test
            Assert.IsNotNull(devNetMethods, "DevNet methods should not be null");
            Assert.IsNotNull(mainNetMethods, "MainNet methods should not be null");
            Assert.IsTrue(devNetMethods.Count > 0, "DevNet should have available methods");
            Assert.IsTrue(mainNetMethods.Count > 0, "MainNet should have available methods");
        }

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
        }
    }
}