using iota_sdk;

namespace iota_sdk_tests
{
    [TestFixture]
    public class IotaClientBuilderTests
    {
        private IotaClientBuilder _builder;

        [SetUp]
        public void Setup()
        {
            _builder = new IotaClientBuilder();
        }

        [Test]
        public void RequestTimeout_SetsTimeoutValue_ReturnsBuilder()
        {
            // Act
            var result = _builder.RequestTimeout(TimeSpan.FromSeconds(45));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IotaClientBuilder>(result);
        }

        [Test]
        public void MaxConcurrentRequests_SetsMaxRequests_ReturnsBuilder()
        {
            // Act
            var result = _builder.MaxConcurrentRequests(20);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IotaClientBuilder>(result);
        }

        [Test]
        public void WsUrl_SetsWebSocketUrl_ReturnsBuilder()
        {
            // Act
            var result = _builder.WsUrl("wss://test.example.com");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IotaClientBuilder>(result);
        }

        [Test]
        public void BasicAuth_SetsCredentials_ReturnsBuilder()
        {
            // Act
            var result = _builder.BasicAuth("username", "password");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IotaClientBuilder>(result);
        }

        [Test]
        public async Task BuildMainnet_CreatesClientWithMainnetEndpoint()
        {
            // Act
            var client = await _builder.BuildMainnet();

            // Assert
            Assert.IsNotNull(client);
            Assert.IsInstanceOf<IIotaClient>(client);

            // Cleanup
            client.Dispose();
        }

        [Test]
        public async Task BuildDevnet_CreatesClientWithDevnetEndpoint()
        {
            // Act
            var client = await _builder.BuildDevnet();

            // Assert
            Assert.IsNotNull(client);
            Assert.IsInstanceOf<IIotaClient>(client);

            // Cleanup
            client.Dispose();
        }

        [Test]
        public async Task BuildTestnet_CreatesClientWithTestnetEndpoint()
        {
            // Act
            var client = await _builder.BuildTestnet();

            // Assert
            Assert.IsNotNull(client);
            Assert.IsInstanceOf<IIotaClient>(client);

            // Cleanup
            client.Dispose();
        }

        //[Test] no local net setup.
        //public async Task BuildLocalnet_CreatesClientWithLocalnetEndpoint()
        //{
        //    // Act
        //    var client = await _builder.BuildLocalnet();

        //    // Assert
        //    Assert.IsNotNull(client);
        //    Assert.IsInstanceOf<IIotaClient>(client);

        //    // Cleanup
        //    client.Dispose();
        //}


        [Test]
        public void WsPingInterval_SetsInterval_ReturnsBuilder()
        {
            // Act
            var result = _builder.WsPingInterval(TimeSpan.FromSeconds(30));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IotaClientBuilder>(result);
        }

        [Test]
        public async Task ChainedConfiguration_CreatesConfiguredClient()
        {
            // Act
            var client = await _builder
                .RequestTimeout(TimeSpan.FromSeconds(45))
                .MaxConcurrentRequests(20)
                .BasicAuth("testuser", "testpass")
                .BuildMainnet();

            // Assert
            Assert.IsNotNull(client);
            Assert.IsInstanceOf<IIotaClient>(client);

            // Cleanup
            client.Dispose();
        }
    }
}