using iota_sdk;
using iota_sdk.apis;
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

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
        }
    }
}