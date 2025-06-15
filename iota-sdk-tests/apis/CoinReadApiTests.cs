using iota_sdk;
using iota_sdk.apis;
using iota_sdk.model.coin;
using iota_sdk_tests.utils;
using System.Reflection;

namespace iota_sdk_tests.apis
{
    [TestFixture]
    public class CoinReadApiTests
    {
        private IotaClient _client;
        private ICoinReadApi target;
        private string _testAddress = "";
        private readonly string _testInvalidAddress = "INVALID_ADDRESS";
        private readonly string _testCoinType = "0x2::iota::IOTA";

        [SetUp]
        public async Task Setup()
        {
            // Initialize the IotaClient using IotaClientBuilder
            var clientBuilder = new IotaClientBuilder()
                .RequestTimeout(TimeSpan.FromSeconds(30))
                .MaxConcurrentRequests(10);

            // initialize test address
            _testAddress = TestsUtils.InitTestAddress();

            // Use the main net endpoint for testing purposes
            _client = (IotaClient)await clientBuilder.BuildMainnet().ConfigureAwait(false);

            // Initialize the CoinReadApi with the client
            target = _client.CoinReadApi();
        }

        [Test]
        public async Task GetAllBalances_ReturnsBalancesList()
        {
            // Act
            var result = await target.GetAllBalances(_testAddress);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<Balance>>(result);
            Assert.GreaterOrEqual(result.Count, 1);

            Assert.IsNotNull(result[0].CoinType);
            Assert.GreaterOrEqual(result[0].CoinObjectCount, 0);
            Assert.IsNotNull(result[0].TotalBalance);
        }

        [Test]
        public async Task GetAllBalances_InvalidAddress_ReturnsBalancesList()
        {
            // Act

            // Capture the exception to check its message
            var exception = Assert.ThrowsAsync<StreamJsonRpc.RemoteMethodNotFoundException>(async () => 
            {
                var result = await target.GetAllBalances(_testInvalidAddress);
            });

            // Check that the exception message contains expected text
            StringAssert.Contains("Invalid params", exception.Message);
        }

        [Test]
        public async Task GetBalance_WithValidAddress_ReturnsBalance()
        {
            // Act
            var result = await target.GetBalance(_testAddress, _testCoinType);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_testCoinType, result.CoinType);
            Assert.GreaterOrEqual(result.CoinObjectCount, 1);
            Assert.IsNotNull(result.TotalBalance);
        }

        [Test]
        public async Task GetBalance_WithoutCoinType_UsesDefaultCoinType()
        {
            // Act
            var result = await target.GetBalance(_testAddress);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.CoinType); // Should default to IOTA
            Assert.GreaterOrEqual(result.CoinObjectCount, 1);
            Assert.IsNotNull(result.TotalBalance);
        }

        [Test]
public async Task GetCoins_WithValidParameters_ReturnsCoinPage()
        {
            // Arrange
            const int limit = 5;

            // Act
            var result = await target.GetCoins(_testAddress, _testCoinType);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.LessOrEqual(result.Data.Count, limit);

            if (result.Data.Count > 0)
            {
                Assert.AreEqual(_testCoinType, result.Data[0].CoinType);
                Assert.IsNotNull(result.Data[0].CoinObjectId);
                Assert.IsNotNull(result.Data[0].Balance);
            }
        }

        [Test]
        public async Task GetAllCoins_WithValidParameters_ReturnsCoinPage()
        {
            // Arrange
            const int limit = 5;

            // Act
            var result = await target.GetAllCoins(_testAddress, null, limit);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.LessOrEqual(result.Data.Count, limit);

            if (result.Data.Count > 0)
            {
                Assert.IsNotNull(result.Data[0].CoinType);
                Assert.IsNotNull(result.Data[0].CoinObjectId);
                Assert.IsNotNull(result.Data[0].Balance);
            }
        }

        [Test]
        public async Task GetCoinMetadata_WithValidCoinType_ReturnsCoinMetadata()
        {
            // Act
            var result = await target.GetCoinMetadata(_testCoinType);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Name);
            Assert.IsNotNull(result.Symbol);
            Assert.IsNotNull(result.Description);
            Assert.GreaterOrEqual(result.Decimals, 0);
        }

        [Test]
        public void GetCoinMetadata_WithNullCoinType_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await target.GetCoinMetadata(null));
        }

        [Test]
        public async Task GetTotalSupply_WithValidCoinType_ReturnsSupply()
        {
            // Act
            var result = await target.GetTotalSupply(_testCoinType);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
        }

        [Test]
        public void GetTotalSupply_WithNullCoinType_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await target.GetTotalSupply(null));
        }

        [Test]
        public async Task GetCirculatingSupply_ReturnsCirculatingSupply()
        {
            // Act
            var result = await target.GetCirculatingSupply();

            // Assert
            Assert.IsNotNull(result);
            Assert.GreaterOrEqual(result.AtCheckpoint, 0);
            Assert.GreaterOrEqual(result.CirculatingSupplyPercentage, 0);
            Assert.LessOrEqual(result.CirculatingSupplyPercentage, 1);
            Assert.GreaterOrEqual(result.Value, 0);
        }

    }
}