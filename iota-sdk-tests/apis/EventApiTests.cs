using iota_sdk;
using iota_sdk.apis.@event;
using iota_sdk.model;
using iota_sdk.model.@event;
using iota_sdk.model.read.transaction;
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
            _target = (IEventApi)_client.EventApi();
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

            // verify data fields of first event
            var firstEvent = result.Data[0];
            Assert.IsNotNull(firstEvent.Bcs);
            Assert.IsNotNull(firstEvent.BcsEncoding);
            Assert.IsNotNull(firstEvent.Id.EventSeq);
            Assert.IsNotNull(firstEvent.Id.TxDigest);
            Assert.IsNotNull(firstEvent.PackageId);
            Assert.IsNotNull(firstEvent.Type);
            Assert.IsNotNull(firstEvent.Sender);
            Assert.IsNotNull(firstEvent.TransactionModule);
            Assert.IsNotNull(firstEvent.ParsedJson);
        }

        [Test]
        public async Task QueryEventsAsync_WithSenderFilter_ReturnsEvents()
        {
            // Arrange
            var filter = EventFilter.BySender(_testAddress);

            // Act
            var result = await _target.QueryEventsAsync(filter).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.IsNotEmpty(result.Data);

            // Verify the results contain events from the expected sender
            foreach (var eventItem in result.Data)
            {
                Assert.IsTrue(string.Equals(_testAddress, eventItem.Sender, StringComparison.OrdinalIgnoreCase));
                //timestampMs string != null
                Assert.IsNotNull(eventItem.TimestampMs);
            }

            // Verify some specific event data from the first event (based on the provided example)
            var firstEvent = result.Data[0];

            Assert.IsNotNull(firstEvent);
            Assert.AreEqual("0x0000000000000000000000000000000000000000000000000000000000000003", firstEvent.PackageId);
            Assert.AreEqual("iota_system", firstEvent.TransactionModule);
            Assert.AreEqual("0x3::validator::StakingRequestEvent", firstEvent.Type);



            // Verify pagination info
            Assert.IsFalse(result.HasNextPage);
            Assert.IsNotNull(result.NextCursor);
            Assert.AreEqual("0", result.NextCursor.EventSeq);
        }


        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
        }
    }
}