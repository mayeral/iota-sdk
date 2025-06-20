
using iota_sdk.model.read;
using iota_sdk_tests.apis;
using Microsoft.Extensions.Configuration;

namespace iota_sdk_tests.utils;

public static class TestsUtils
{
    public static string InitTestAddress()
    {
        string testAddress = "";
        // Load configuration from user secrets
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<GovernanceApiTests>()
            .Build();

        // Get test address from secrets
        testAddress = configuration["TestSettings:Address"] ?? throw new InvalidOperationException(
            "Test address not found in user secrets. Set the TestSettings:Address key in user secrets.\r\n" +
            "Example: dotnet user-secrets init --project iota-sdk-tests\r\n" +
            "dotnet user-secrets set \"TestSettings:Address\" \"0xIOTA_ADDRESS\" --project iota-sdk-tests\r\n");

        return testAddress;
    }

    public static ObjectId InitTestObjectId()
    {
        string objectId;

        var configuration = new ConfigurationBuilder()
        .AddUserSecrets<GovernanceApiTests>()
        .Build();

        objectId = configuration["TestSettings:ObjectId"] ?? throw new InvalidOperationException(
            "Test address not found in user secrets. Set the TestSettings:Address key in user secrets.\r\n" +
            "Example: dotnet user-secrets init --project iota-sdk-tests\r\n" +
            "dotnet user-secrets set \"TestSettings:ObjectId\" \"0xIOTA_ADDRESS\" --project iota-sdk-tests\r\n");

        return new ObjectId(objectId);
    }

    /// <summary>
    /// Initializes a TransactionDigest using a value from user secrets.
    /// </summary>
    /// <returns>A TransactionDigest initialized with the value from user secrets.</returns>
    public static TransactionDigest InitTransactionDigest()
    {
        string transactionDigest;

        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<GovernanceApiTests>()
            .Build();

        transactionDigest = configuration["TestSettings:TransactionDigest"] ?? throw new InvalidOperationException(
            "Test transaction digest not found in user secrets. Set the TestSettings:TransactionDigest key in user secrets.\r\n" +
            "Example: dotnet user-secrets init --project iota-sdk-tests\r\n" +
            "dotnet user-secrets set \"TestSettings:TransactionDigest\" \"YOUR_TRANSACTION_DIGEST\" --project iota-sdk-tests\r\n");

        return TransactionDigest.Parse(transactionDigest);
    }
}