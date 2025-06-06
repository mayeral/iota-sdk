// See https://aka.ms/new-console-template for more information

using iota_sdk;
using iota_sdk.model;

Console.WriteLine("Hello, World!");

// Create client
var client = await new IotaClientBuilder().BuildMainnet().ConfigureAwait(false);


Console.WriteLine("Available RPC methods:");
var rpcMethods = client.AvailableRpcMethods();
foreach (var method in rpcMethods)
{
    Console.WriteLine(method);
}

Console.WriteLine("Coins:");
// Get balances for an address
string address = "0x7b4a34f6a011794f0ecbe5e5beb96102d3eef6122eb929b9f50a8d757bfbdd67";
List<Balance> balances = await client.CoinReadApi().GetAllBalances(address).ConfigureAwait(false);

// Display the results
foreach (var balance in balances)
{
    Console.WriteLine($"Coin: {balance.CoinType}, Amount: {balance.TotalBalance}");
}

Console.WriteLine("Press any key to continue...");
Console.ReadKey();