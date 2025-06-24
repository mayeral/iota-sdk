using iota_sdk.model.read.@object;
using iota_sdk.model.read.transaction;

namespace iota_sdk_tests.utils;

public static class TestsUtils
{
    // Generic method to get a configuration value from environment variables
    private static string GetEnvironmentValue(string envKey, string errorMessage)
    {
        string value = Environment.GetEnvironmentVariable(envKey);

        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidOperationException(errorMessage);
        }

        return value;
    }
    
    public static string InitTestAddress()
    {
        return GetEnvironmentValue(
            "TestSettings__Address",
            "Test address not found in environment variables. " +
            "Set the TestSettings__Address environment variable."
        );
    }
    
    public static ObjectId InitTestObjectId()
    {
        string objectId = GetEnvironmentValue(
            "TestSettings__ObjectId",
            "Test object ID not found in environment variables. " +
            "Set the TestSettings__ObjectId environment variable."
        );
        
        return new ObjectId(objectId);
    }
    
    public static TransactionDigest InitTransactionDigest()
    {
        string transactionDigest = GetEnvironmentValue(
            "TestSettings__TransactionDigest",
            "Test transaction digest not found in environment variables. " +
            "Set the TestSettings__TransactionDigest environment variable."
        );
        
        return TransactionDigest.Parse(transactionDigest);
    }
    
    public static TransactionDigest InitTransactionDigest2()
    {
        string transactionDigest = GetEnvironmentValue(
            "TestSettings__TransactionDigest2",
            "Second test transaction digest not found in environment variables. " +
            "Set the TestSettings__TransactionDigest2 environment variable."
        );
        
        return TransactionDigest.Parse(transactionDigest);
    }
    
    public static ObjectId InitTestObjectId2()
    {
        string objectId = GetEnvironmentValue(
            "TestSettings__ObjectId2",
            "Second test object ID not found in environment variables. " +
            "Set the TestSettings__ObjectId2 environment variable."
        );
        
        return new ObjectId(objectId);
    }
}