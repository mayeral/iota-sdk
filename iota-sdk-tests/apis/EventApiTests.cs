using Iota.Model.Read;
using iota_sdk;
using iota_sdk.apis.@event;
using iota_sdk.model;
using iota_sdk_tests.utils;

namespace iota_sdk_tests.apis
{
    [TestFixture]
    public class EventApiTests
    {
        private IIotaClient _client;
        private IEventApi _target;
        private readonly string _testDigest = "EnktLu7xvWg4qpdrbPpTVeRTQfm1a79xJEL98mX6UbQC";
        private IotaAddress? _testAddress;

        [SetUp]
        public async Task Setup()
        {
            // Initialize the IotaClient using IotaClientBuilder
            var clientBuilder = new IotaClientBuilder()
                .RequestTimeout(TimeSpan.FromSeconds(30))
                .MaxConcurrentRequests(10);

            // Initialize test address
            _testAddress = TestsUtils.InitTestAddress();

            // Use the main net endpoint for testing purposes
            _client = await clientBuilder.BuildMainnet().ConfigureAwait(false);

            // Initialize the EventApi with the client
            _target = _client.EventApi();
        }

        [Test]
        public async Task GetEventsAsync_WithParsedDigest_ReturnsEvents()
        {
            // arrange
            // Using the Parse method from TransactionDigest class
            var transactionDigest = TransactionDigest.Parse(_testDigest);

            // act
            var result = await _target!.GetEventsAsync(transactionDigest).ConfigureAwait(false);

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.IsNotEmpty(result.Data);
        }


        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
        }
    }
}