using iota_sdk;
using iota_sdk.apis;
using iota_sdk.model.coin;

namespace iota_sdk_tests
{
    [TestFixture]
    public class CoinReadApiTests
    {
        private IotaClient _client;
        private ICoinReadApi target;
        private readonly string _testAddress = "0x7b4a34f6a011794f0ecbe5e5beb96102d3eef6122eb929b9f50a8d757bfbdd67";
        private readonly string _testInvalidAddress = "INVALID_ADDRESS";
        private readonly string _testCoinType = "0x2::iota::IOTA";

        [SetUp]
        public async Task Setup()
        {
            // Initialize the IotaClient using IotaClientBuilder
            var clientBuilder = new IotaClientBuilder()
                .RequestTimeout(TimeSpan.FromSeconds(30))
                .MaxConcurrentRequests(10);

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
            var result = await target.GetAllBalances(_testInvalidAddress);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<Balance>>(result);
            Assert.GreaterOrEqual(result.Count, 1);

            Assert.IsNotNull(result[0].CoinType);
            Assert.GreaterOrEqual(result[0].CoinObjectCount, 0);
            Assert.IsNotNull(result[0].TotalBalance);
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
            var result = await target.GetCoins(_testAddress, _testCoinType, null, limit);

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
        public async Task GetCoins_WithPagination_ReturnsNextPage()
        {
            // This test checks if pagination works correctly
            // First, get the first page
            var firstPage = await target.GetCoins(_testAddress, _testCoinType, null, 2);

            // If there's a next page
            if (firstPage.HasNextPage && firstPage.NextCursor != null)
            {
                // Get the second page
                var secondPage = await target.GetCoins(_testAddress, _testCoinType, firstPage.NextCursor, 2);

                // Assert
                Assert.IsNotNull(secondPage);
                Assert.IsNotNull(secondPage.Data);

                // Verify the items are different
                if (firstPage.Data.Count > 0 && secondPage.Data.Count > 0)
                {
                    Assert.AreNotEqual(firstPage.Data[0].CoinObjectId, secondPage.Data[0].CoinObjectId);
                }
            }
            else
            {
                // If there's no next page, the test is still valid
                Assert.Pass("No next page available, pagination test skipped");
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