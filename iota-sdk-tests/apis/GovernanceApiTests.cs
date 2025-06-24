using iota_sdk;
using iota_sdk.apis.governance;
using iota_sdk_tests.utils;
using System.Numerics;
using iota_sdk.model.governance;

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
        Assert.IsNotNull(result.First().StakingPool);
        Assert.IsNotNull(result.First().ValidatorAddress);
        Assert.IsNotNull(result.First().Stakes);
        
        foreach (var stake in result.First().Stakes)
        {
            Assert.IsNotNull(stake.EstimatedReward);
            Assert.IsNotNull(stake.Principal);
            Assert.IsNotNull(stake.StakeActiveEpoch);
            Assert.IsNotNull(stake.StakedIotaId);
            Assert.IsNotNull(stake.StakeRequestEpoch);
            Assert.IsNotNull(stake.Status);
        }   

        return Task.CompletedTask;
    }

    [Test]
    public Task GetStakesByIds_ReturnsStakes()
    {
        // First get stakes for our test address
        var delegatedStakes = _target!.GetStakesAsync(_testAddress).Result;
        Assert.IsNotNull(delegatedStakes, "Should return delegated stakes");

        // Skip test if no stakes found
        Assume.That(delegatedStakes.Any(), "No stakes found for test address");

        // Extract staked IOTA IDs from the delegated stakes
        var stakedIotaIds = delegatedStakes
            .SelectMany(delegatedStake => delegatedStake.Stakes)
            .Select(stake => stake.StakedIotaId)
            .ToArray();

        // Call GetStakesByIdsAsync with the IDs we found
        var result = _target!.GetStakesByIdsAsync(stakedIotaIds).Result;

        // Assert
        Assert.IsNotNull(result, "Result should not be null");
        Assert.IsTrue(result.Any(), "Result should contain stakes");

        // Verify that all requested IDs are present in the result
        foreach (var id in stakedIotaIds)
        {
            Assert.IsTrue(result.Any(delegatedStake =>
                    delegatedStake.Stakes.Any(stake => stake.StakedIotaId == id)),
                $"Requested stake ID {id} not found in results");
        }

        // Verify basic properties of returned stakes
        foreach (var delegatedStake in result)
        {
            Assert.IsNotNull(delegatedStake.Stakes, "Stakes collection should not be null");

            foreach (var stake in delegatedStake.Stakes)
            {
                Assert.IsNotNull(stake.StakedIotaId, "StakedIotaId should not be null");
                Assert.IsNotNull(stake.Status, "Status should not be null");
            }
        }

        return Task.CompletedTask;
    }

    [Test]
    public Task GetTimelockedStakesAsync_ReturnsTimelockedStakes()
    {
        // act
        var result = _target!.GetTimelockedStakesAsync(_testAddress).Result;
        // assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.First().StakingPool);
        Assert.IsNotNull(result.First().ValidatorAddress);
        Assert.IsNotNull(result.First().Stakes);

        foreach (var timelockedStakes in result.First().Stakes)
        {
            Assert.IsNotNull(timelockedStakes.TimelockedStakedIotaId);
            Assert.IsNotNull(timelockedStakes.StakeRequestEpoch);
            Assert.IsNotNull(timelockedStakes.StakeActiveEpoch);
            Assert.Greater(timelockedStakes.Principal, 0);
            Assert.IsNotNull(timelockedStakes.Status);
            Assert.IsNotNull(timelockedStakes.EstimatedReward);
            Assert.Greater(timelockedStakes.ExpirationTimestampMs, 0);
            Assert.IsNotNull(timelockedStakes.Label);
        }

        return Task.CompletedTask;
    }

    [Test]
    public Task GetTimelockedStakesByIds_ReturnsStakes()
    {
        // First get timelocked stakes for our test address
        var delegatedTimelockedStakes = _target!.GetTimelockedStakesAsync(_testAddress).Result;
        Assert.IsNotNull(delegatedTimelockedStakes, "Should return delegated timelocked stakes");

        // Skip test if no timelocked stakes found
        Assume.That(delegatedTimelockedStakes.Any(), "No timelocked stakes found for test address");

        // Extract timelocked staked IOTA IDs from the delegated stakes
        var timelockedStakedIotaIds = delegatedTimelockedStakes
            .SelectMany(delegatedStake => delegatedStake.Stakes)
            .Select(stake => stake.TimelockedStakedIotaId)
            .ToArray();

        // Call GetTimelockedStakesByIdsAsync with the IDs we found
        var result = _target!.GetTimelockedStakesByIdsAsync(timelockedStakedIotaIds).Result;

        // Assert
        Assert.IsNotNull(result, "Result should not be null");
        Assert.IsTrue(result.Any(), "Result should contain timelocked stakes");

        // Verify that all requested IDs are present in the result
        foreach (var id in timelockedStakedIotaIds)
        {
            Assert.IsTrue(result.Any(delegatedStake =>
                    delegatedStake.Stakes.Any(stake => stake.TimelockedStakedIotaId == id)),
                $"Requested timelocked stake ID {id} not found in results");
        }

        // Verify basic properties of returned timelocked stakes
        foreach (var delegatedStake in result)
        {
            Assert.IsNotNull(delegatedStake.Stakes, "Stakes collection should not be null");

            foreach (var stake in delegatedStake.Stakes)
            {
                Assert.IsNotNull(stake.TimelockedStakedIotaId, "TimelockedStakedIotaId should not be null");
                Assert.IsNotNull(stake.Status, "Status should not be null");
            }
        }

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
    public Task GetLatestIotaSystemStateAsync_ReturnsSystemStateV2()
    {
        // act
        var result = _target!.GetLatestIotaSystemStateAsync().Result;

        // assert
        Assert.That(result!.IsV1, Is.False);
        Assert.That(result.IsV2, Is.True);
        Assert.IsNotNull(result.V2);
        Assert.IsNull(result.V1);

        IotaSystemStateSummaryV2 systemState = result.V2;

        // Basic system state properties
        Assert.That(systemState.Epoch, Is.GreaterThan(new BigInteger(1)));
        Assert.That(systemState.ProtocolVersion, Is.GreaterThan(0UL));
        Assert.That(systemState.SystemStateVersion, Is.GreaterThan(0UL));
        Assert.That(systemState.IotaTotalSupply, Is.GreaterThan(0UL));
        Assert.IsNotNull(systemState.IotaTreasuryCapId);
        Assert.That(systemState.StorageFundTotalObjectStorageRebates, Is.GreaterThanOrEqualTo(0UL));
        Assert.That(systemState.StorageFundNonRefundableBalance, Is.GreaterThanOrEqualTo(0UL));
        Assert.That(systemState.ReferenceGasPrice, Is.GreaterThan(0UL));

        // Safe mode properties
        Assert.That(systemState.SafeMode, Is.False);  // Assuming safe mode is off in normal testing
        Assert.That(systemState.SafeModeStorageCharges, Is.GreaterThanOrEqualTo(0UL));
        //Assert.That(systemState.SafeModeComputationRewards, Is.GreaterThanOrEqualTo(0UL)); // only v1
        Assert.That(systemState.SafeModeComputationCharges, Is.GreaterThanOrEqualTo(0UL));
        Assert.That(systemState.SafeModeComputationChargesBurned, Is.GreaterThanOrEqualTo(0UL));
        Assert.That(systemState.SafeModeStorageRebates, Is.GreaterThanOrEqualTo(0UL));
        Assert.That(systemState.SafeModeNonRefundableStorageFee, Is.GreaterThanOrEqualTo(0UL));

        // Epoch properties
        Assert.That(systemState.EpochStartTimestampMs, Is.GreaterThan(0UL));
        Assert.That(systemState.EpochDurationMs, Is.GreaterThan(0UL));

        // Validator constraints
        Assert.That(systemState.MinValidatorCount, Is.GreaterThan(0UL));
        Assert.That(systemState.MaxValidatorCount, Is.GreaterThan(systemState.MinValidatorCount));
        Assert.That(systemState.MinValidatorJoiningStake, Is.GreaterThan(0UL));
        Assert.That(systemState.ValidatorLowStakeThreshold, Is.GreaterThan(0UL));
        Assert.That(systemState.ValidatorVeryLowStakeThreshold, Is.GreaterThan(0UL));
        Assert.That(systemState.ValidatorLowStakeGracePeriod, Is.GreaterThan(0UL));

        // Stake and validators
        Assert.That(systemState.TotalStake, Is.GreaterThanOrEqualTo(0UL));
        Assert.IsNotNull(systemState.ActiveValidators);
        Assert.That(systemState.ActiveValidators.Count, Is.GreaterThan(0));

        // Pending validators
        Assert.IsNotNull(systemState.PendingActiveValidatorsId);
        Assert.That(systemState.PendingActiveValidatorsSize, Is.GreaterThanOrEqualTo(0UL));
        Assert.IsNotNull(systemState.PendingRemovals);

        // Staking pool mappings
        Assert.IsNotNull(systemState.StakingPoolMappingsId);
        Assert.That(systemState.StakingPoolMappingsSize, Is.GreaterThanOrEqualTo(0UL));

        // Inactive pools
        Assert.IsNotNull(systemState.InactivePoolsId);
        Assert.That(systemState.InactivePoolsSize, Is.GreaterThanOrEqualTo(0UL));

        // Validator candidates
        Assert.IsNotNull(systemState.ValidatorCandidatesId);
        Assert.That(systemState.ValidatorCandidatesSize, Is.GreaterThanOrEqualTo(0UL));

        // Risk and reporting
        Assert.IsNotNull(systemState.AtRiskValidators);
        Assert.IsNotNull(systemState.ValidatorReportRecords);

        // Check the first active validator if there are any
        if (systemState.ActiveValidators.Count > 0)
        {
            var firstValidator = systemState.ActiveValidators[0];
            Assert.IsNotNull(firstValidator);
            Assert.IsNotNull(firstValidator.IotaAddress);
            Assert.IsNotNull(firstValidator.Name);
            Assert.That(firstValidator.VotingPower, Is.GreaterThan(0UL));
            Assert.IsNotNull(firstValidator.StakingPoolId);
        }

        // Check committee members' validators
        foreach (var memberIndex in systemState.CommitteeMembers)
        {
            var validator = systemState.ActiveValidators[(int)memberIndex];
            Assert.IsNotNull(validator);
            Assert.IsNotNull(validator.IotaAddress);
            Assert.IsNotNull(validator.Name);
            Assert.That(validator.VotingPower, Is.GreaterThan(0UL));
            Assert.IsNotNull(validator.StakingPoolId);
        }


        return Task.CompletedTask;
    }

    [Test]
    public Task GetLatestIotaSystemStateV1Async_ReturnsSystemStateV1()
    {
        // act
        var result = _target!.GetlatestIotaSystemStateV1Async().Result;

        // assert
        Assert.That(result!.IsV1, Is.True);
        Assert.That(result.IsV2, Is.False);
        Assert.IsNotNull(result.V1);
        Assert.IsNull(result.V2);

        IotaSystemStateSummaryV1 systemState = result.V1;

        // Basic system state properties
        Assert.That(systemState.Epoch, Is.GreaterThan(new BigInteger(1)));
        Assert.That(systemState.ProtocolVersion, Is.GreaterThan(0UL));
        Assert.That(systemState.SystemStateVersion, Is.GreaterThan(0UL));
        Assert.That(systemState.IotaTotalSupply, Is.GreaterThan(0UL));
        Assert.IsNotNull(systemState.IotaTreasuryCapId);
        Assert.That(systemState.StorageFundTotalObjectStorageRebates, Is.GreaterThanOrEqualTo(0UL));
        Assert.That(systemState.StorageFundNonRefundableBalance, Is.GreaterThanOrEqualTo(0UL));
        Assert.That(systemState.ReferenceGasPrice, Is.GreaterThan(0UL));

        // Safe mode properties
        Assert.That(systemState.SafeMode, Is.False);  // Assuming safe mode is off in normal testing
        Assert.That(systemState.SafeModeStorageCharges, Is.GreaterThanOrEqualTo(0UL));
        Assert.That(systemState.SafeModeComputationRewards, Is.GreaterThanOrEqualTo(0UL));
        //Assert.That(systemState.SafeModeComputationCharges, Is.GreaterThanOrEqualTo(0UL)); only v2
        //Assert.That(systemState.SafeModeComputationChargesBurned, Is.GreaterThanOrEqualTo(0UL)); only v2
        Assert.That(systemState.SafeModeStorageRebates, Is.GreaterThanOrEqualTo(0UL));
        Assert.That(systemState.SafeModeNonRefundableStorageFee, Is.GreaterThanOrEqualTo(0UL));

        // Epoch properties
        Assert.That(systemState.EpochStartTimestampMs, Is.GreaterThan(0UL));
        Assert.That(systemState.EpochDurationMs, Is.GreaterThan(0UL));

        // Validator constraints
        Assert.That(systemState.MinValidatorCount, Is.GreaterThan(0UL));
        Assert.That(systemState.MaxValidatorCount, Is.GreaterThan(systemState.MinValidatorCount));
        Assert.That(systemState.MinValidatorJoiningStake, Is.GreaterThan(0UL));
        Assert.That(systemState.ValidatorLowStakeThreshold, Is.GreaterThan(0UL));
        Assert.That(systemState.ValidatorVeryLowStakeThreshold, Is.GreaterThan(0UL));
        Assert.That(systemState.ValidatorLowStakeGracePeriod, Is.GreaterThan(0UL));

        // Stake and validators
        Assert.That(systemState.TotalStake, Is.GreaterThanOrEqualTo(0UL));
        Assert.IsNotNull(systemState.ActiveValidators);
        Assert.That(systemState.ActiveValidators.Count, Is.GreaterThan(0));

        // Pending validators
        Assert.IsNotNull(systemState.PendingActiveValidatorsId);
        Assert.That(systemState.PendingActiveValidatorsSize, Is.GreaterThanOrEqualTo(0UL));
        Assert.IsNotNull(systemState.PendingRemovals);

        // Staking pool mappings
        Assert.IsNotNull(systemState.StakingPoolMappingsId);
        Assert.That(systemState.StakingPoolMappingsSize, Is.GreaterThanOrEqualTo(0UL));

        // Inactive pools
        Assert.IsNotNull(systemState.InactivePoolsId);
        Assert.That(systemState.InactivePoolsSize, Is.GreaterThanOrEqualTo(0UL));

        // Validator candidates
        Assert.IsNotNull(systemState.ValidatorCandidatesId);
        Assert.That(systemState.ValidatorCandidatesSize, Is.GreaterThanOrEqualTo(0UL));

        // Risk and reporting
        Assert.IsNotNull(systemState.AtRiskValidators);
        Assert.IsNotNull(systemState.ValidatorReportRecords);

        // Check the first active validator if there are any
        if (systemState.ActiveValidators.Count > 0)
        {
            var firstValidator = systemState.ActiveValidators[0];
            Assert.IsNotNull(firstValidator);
            Assert.IsNotNull(firstValidator.IotaAddress);
            Assert.IsNotNull(firstValidator.Name);
            Assert.That(firstValidator.VotingPower, Is.GreaterThan(0UL));
            Assert.IsNotNull(firstValidator.StakingPoolId);
        }

        // Check committee members' validators
        //foreach (var memberIndex in systemState.CommitteeMembers) // only v2
        //{
        //    var validator = systemState.ActiveValidators[(int)memberIndex];
        //    Assert.IsNotNull(validator);
        //    Assert.IsNotNull(validator.IotaAddress);
        //    Assert.IsNotNull(validator.Name);
        //    Assert.That(validator.VotingPower, Is.GreaterThan(0UL));
        //    Assert.IsNotNull(validator.StakingPoolId);
        //}

        return Task.CompletedTask;
    }


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