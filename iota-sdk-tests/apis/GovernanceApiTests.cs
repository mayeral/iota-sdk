using iota_sdk;
using iota_sdk.apis.governance;
using iota_sdk_tests.utils;
using Microsoft.Extensions.Configuration;
using System.Numerics;
// ReSharper disable AsyncApostle.AsyncWait

namespace iota_sdk_tests.apis;

[TestFixture]
public class GovernanceApiTests
{
    private IIotaClient? _client;
    private IGovernanceApi? _target;
    private string _testAddress = "";

    [SetUp]
    public void Setup()
    {
        // Initialize the IotaClient using IotaClientBuilder
        var clientBuilder = new IotaClientBuilder()
            .RequestTimeout(TimeSpan.FromSeconds(30))
            .MaxConcurrentRequests(10);

        // initialize test address
        _testAddress = TestsUtils.InitTestAddress();

        // Use the main net endpoint for testing purposes
        _client = clientBuilder.BuildMainnet().Result;

        // Initialize the GovernanceApi with the client   
        _target = _client.GovernanceApi();
    }


    [Test]
    public Task GetStakesAsync_ReturnsStakes()
    {
        // act
        var result = _target!.GetStakesAsync(_testAddress).Result;
        // assert
        Assert.IsNotNull(result);

        return Task.CompletedTask;
    }

    [Test]
    public Task GetTimelockedStakesAsync_ReturnsTimelockedStakes()
    {
        // act
        var result = _target!.GetTimelockedStakesAsync(_testAddress).Result;
        // assert
        Assert.IsNotNull(result);

        return Task.CompletedTask;
    }

    [Test]
    public Task GetCommitteeInfoAsync_ReturnsCommitteeInfo()
    {
        // act
        var result = _target!.GetCommitteeInfoAsync().Result;

        // assert epoch is bigger than 1
        Assert.That(result.Epoch, Is.GreaterThan(new BigInteger(1)));
        Assert.That(result.Validators[0].AuthorityName, Is.Not.Null);
        Assert.That(result.Validators[0].StakeUnit, Is.GreaterThanOrEqualTo(BigInteger.Zero));

        return Task.CompletedTask;
    }

    [Test]
    public Task GetCommitteeInfoAsync_WithEpoch_ReturnsCommitteeInfo()
    {
        // arrange
        ulong epoch = 1;

        // act
        var result = _target!.GetCommitteeInfoAsync(epoch).Result;

        // assert epoch is 1
        Assert.That(result.Epoch, Is.EqualTo(new BigInteger(1)));
        Assert.That(result.Validators[0].AuthorityName, Is.Not.Null);
        Assert.That(result.Validators[0].StakeUnit, Is.GreaterThanOrEqualTo(BigInteger.Zero));


        return Task.CompletedTask;
    }

    [Test]
    public Task GetLatestIotaSystemStateAsync_ReturnsSystemState()
    {
        // act
        var result = _target!.GetLatestIotaSystemStateAsync().Result;

        // assert
        // TODO
        Assert.Fail("Test not implemented");

        return Task.CompletedTask;
    }

    [Test]
    public Task GetLatestIotaSystemStateV2Async_ReturnsSystemState()
    {
        // act
        var result = _target!.GetLatestIotaSystemStateV2Async().Result;

        // assert
        // TODO
        Assert.Fail("Test not implemented");

        return Task.CompletedTask;
    }


    // get system state?!

    //public Task GetIotaSystemStateAsync_ReturnsSystemState()
    //{
    //    target.

    //        return Task.CompletedTask;
    //}

    [Test]
    public Task GetReferenceGasPriceAsync_ReturnsGasPrice()
    {
        // act
        var result = _target!.GetReferenceGasPriceAsync().Result;

        // assert > 0
        Assert.That(result, Is.GreaterThan(0));

        return Task.CompletedTask;
    }

    [Test]
    public Task GetValidatorsApyAsync_ReturnsValidatorApys()
    {
        // act
        var result = _target!.GetValidatorsApyAsync().Result;

        // assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Apys);
        Assert.GreaterOrEqual(result.Apys.Count, 1);
        Assert.IsNotNull(result.Epoch);

        foreach (var apy in result.Apys)
        {
            Assert.IsNotNull(apy.Address);
            Assert.GreaterOrEqual(apy.Apy, 0);
        }

        return Task.CompletedTask;
    }
}