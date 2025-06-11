using System.Numerics;
using iota_sdk;
using iota_sdk.apis.governance;
// ReSharper disable AsyncApostle.AsyncWait

namespace iota_sdk_tests;

[TestFixture]
public class GovernanceApiTests
{
    private IotaClient _client;
    private IGovernanceApi target;
    private readonly string _testAddress = "TODO_ADDRESS";


    [SetUp]
    public async Task Setup()
    {
        // Initialize the IotaClient using IotaClientBuilder
        var clientBuilder = new IotaClientBuilder()
            .RequestTimeout(TimeSpan.FromSeconds(30))
            .MaxConcurrentRequests(10);

        // Use the main net endpoint for testing purposes
        _client = (IotaClient) await clientBuilder.BuildMainnet().ConfigureAwait(false);

        // Initialize the GovernanceApi with the client   
        target = _client.GovernanceApi();
    }


    [Test]
    public Task GetStakesAsync_ReturnsStakes()
    {
        // act
        var result = target.GetStakesAsync(_testAddress).Result;

        // assert
        Assert.IsNotNull(result);
        // TODO

        return Task.CompletedTask;
    }

    [Test]
    public Task GetTimelockedStakesAsync_ReturnsTimelockedStakes()
    {
        // act
        var result = target.GetTimelockedStakesAsync(_testAddress).Result;

        // assert
        Assert.IsNotNull(result);

        // TODO
        Assert.Fail("Test not implemented");

        return Task.CompletedTask;
    }

    [Test]
    public Task GetCommitteeInfoAsync_ReturnsCommitteeInfo()
    {
        // act
        var result = target.GetCommitteeInfoAsync().Result;

        // assert
        // TODO
        Assert.Fail("Test not implemented");

        return Task.CompletedTask;
    }

    [Test]
    public Task GetCommitteeInfoAsync_WithEpoch_ReturnsCommitteeInfo()
    {
        // arrange
        BigInteger epoch = 1;

        // act
        var result = target.GetCommitteeInfoAsync(epoch).Result;

        // assert
        // TODO
        Assert.Fail("Test not implemented");

        return Task.CompletedTask;
    }

    [Test]
    public Task GetLatestIotaSystemStateAsync_ReturnsSystemState()
    {
        // act
        var result = target.GetLatestIotaSystemStateAsync().Result;

        // assert
        // TODO
        Assert.Fail("Test not implemented");

        return Task.CompletedTask;
    }

    [Test]
    public Task GetReferenceGasPriceAsync_ReturnsGasPrice()
    {
        // act
        var result = target.GetReferenceGasPriceAsync().Result;

        // assert
        // TODO
        Assert.Fail("Test not implemented");

        return Task.CompletedTask;
    }

    [Test]
    public Task GetValidatorsApyAsync_ReturnsValidatorApys()
    {
        // act
        var result = target.GetValidatorsApyAsync().Result;

        // assert
        // TODO
        Assert.Fail("Test not implemented");

        return Task.CompletedTask;
    }
}