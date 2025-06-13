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
}