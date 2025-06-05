// See https://aka.ms/new-console-template for more information

using iota_sdk;
using iota_sdk.apis;

Console.WriteLine("Hello, World!");

// Create client
var client = await new IotaClientBuilder().BuildMainnet().ConfigureAwait(false);

// Get balances for an address
string address = "0x94f1a597b4e8f709a396f7f6b1482bdcd65a673d111e49286c527fab7c2d0961";
List<Balance> balances = await client.CoinReadApi().GetAllBalances(address).ConfigureAwait(false);

// Display the results
foreach (var balance in balances)
{
    Console.WriteLine($"Coin: {balance.CoinType}, Amount: {balance.TotalBalance}");
}