# IOTA .NET SDK

This is a .NET implementation of the IOTA SDK, ported from the original [IOTA Rust SDK](https://docs.iota.org/references/rust-sdk). It provides a comprehensive set of APIs to interact with the IOTA network.

## Overview

The IOTA .NET SDK enables developers to build applications that interact with the IOTA network using C# and .NET. It communicates with IOTA nodes using the [IOTA JSON-RPC API](https://docs.iota.org/iota-api-ref) for accessing both full node and indexer functionality.

## Implementation Status

**⚠️ This is an alpha version release and is Work in Progress ⚠️**

The IOTA .NET SDK is currently under active development. 
Its a private and non commercial project. Feel free to contribute.

[![Latest Release](https://img.shields.io/github/v/release/mayeral/iota-sdk?include_prereleases&label=Latest%20Release)](https://github.com/mayeral/iota-sdk/releases)
[![Build Status](https://github.com/mayeral/iota-sdk/actions/workflows/dotnet-release.yml/badge.svg)](https://github.com/mayeral/iota-sdk/actions/workflows/dotnet-release.yml)
Released under the [MIT License](https://licenses.nuget.org/MIT).

### Implementation Overview

The following table shows the current implementation status of the various APIs:

| API | Implemented | Tests |
|-----|-------------|--------|
| CoinReadApi | ✅ | ✅ |
| GovernanceApi | ✅ | ✅ |
| ReadApi | ⚠️ | ⚠️ |
| EventApi | ✅ | ✅ |
| TransactionBuilder | ❌ | ❌ |
| QuorumDriverApi | ❌ | ❌ |

## Getting Started

### Installation

Add the IOTA .NET SDK NuGet package to your project:

```bash
dotnet add package IOTA.SDK
```

### Basic Usage

The main building block of the IOTA .NET SDK is the `IotaClientBuilder`, which provides a simple way to connect to an IOTA network and access the available APIs:

```csharp
using IOTA.SDK;
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        try
        {
            // Connect to IOTA testnet
            var iotaTestnet = await new IotaClientBuilder()
                .BuildTestnet();
            Console.WriteLine($"IOTA testnet version: {iotaTestnet.ApiVersion()}");

            // Connect to IOTA devnet
            var iotaDevnet = await new IotaClientBuilder()
                .BuildDevnet();
            Console.WriteLine($"IOTA devnet version: {iotaDevnet.ApiVersion()}");

            // Connect to IOTA mainnet
            var iotaMainnet = await new IotaClientBuilder()
                .BuildMainnet();
            Console.WriteLine($"IOTA mainnet version: {iotaMainnet.ApiVersion()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
```

## Connecting to IOTA Network

The SDK supports connecting to various IOTA networks:

- Local: `http://127.0.0.1:9000`
- Devnet: `https://api.devnet.iota.cafe:443`
- Testnet: `https://api.testnet.iota.cafe:443`
- Mainnet: `https://api.mainnet.iota.cafe:443`

For indexer functionality, use the corresponding indexer endpoints:
- Local: `http://127.0.0.1:9124`
- Devnet: `https://indexer.devnet.iota.cafe:443`
- Testnet: `https://indexer.testnet.iota.cafe:443`
- Mainnet: `https://indexer.mainnet.iota.cafe:443`

Example:

```csharp
// Connect to a specific node
var client = await new IotaClientBuilder()
    .Build("http://127.0.0.1:9000");

// Or use convenience methods
var devnetClient = await new IotaClientBuilder().BuildDevnet();
var testnetClient = await new IotaClientBuilder().BuildTestnet();
var mainnetClient = await new IotaClientBuilder().BuildMainnet();
```

## Reading Balances

You can easily check balances for an address:

```csharp
using IOTA.SDK;
using IOTA.SDK.Types;
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        try
        {
            var client = await new IotaClientBuilder().BuildTestnet();
            
            var address = IotaAddress.FromString("<YOUR IOTA ADDRESS>");
            var balances = await client.CoinReadApi().GetAllBalancesAsync(address);
            
            Console.WriteLine($"Balances for address {address}: {string.Join(", ", balances)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
```

## Documentation

For more detailed documentation and examples, please refer to the inline code documentation and the original [IOTA Rust SDK documentation](https://github.com/iotaledger/iota/tree/develop/crates/iota-sdk) which this library is based on.

## Disclaimer (Haftungsausschluss)

This software is provided "as is" without warranty of any kind, either expressed or implied, including but not limited to the warranties of merchantability, fitness for a particular purpose, and non-infringement. In no event shall the authors or copyright holders be liable for any claim, damages, or other liability, whether in an action of contract, tort or otherwise, arising from, out of, or in connection with the software or the use or other dealings in the software.

The use of this software for financial transactions or interactions with the IOTA network is at your own risk. Make sure to thoroughly test your applications and understand the implications of interacting with distributed ledger technologies before deploying to production environments.

## License

This project is licensed under the [MIT Licence](https://licenses.nuget.org/MIT).


# API Implementation Status Table

| API Method | Category | Implemented | UTest |
|------------|----------|------------|-------|
| iota_devInspectTransactionBlock | Write API | ❌ | ❌ |
| iota_dryRunTransactionBlock | Write API | ❌ | ❌ |
| iota_executeTransactionBlock | Write API | ❌ | ❌ |
| iota_getChainIdentifier | Read API | ✅ | ✅ |
| iota_getCheckpoint | Read API | ✅ | ✅ |
| iota_getCheckpoints | Read API | ✅ | ✅ |
| iota_getEvents | Event API | ✅ | ✅ |
| iota_getLatestCheckpointSequenceNumber | Read API | ✅ | ✅ |
| iota_getMoveFunctionArgTypes | Move Utils | ❌ | ❌ |
| iota_getNormalizedMoveFunction | Move Utils | ❌ | ❌ |
| iota_getNormalizedMoveModule | Move Utils | ❌ | ❌ |
| iota_getNormalizedMoveModulesByPackage | Move Utils | ❌ | ❌ |
| iota_getNormalizedMoveStruct | Move Utils | ❌ | ❌ |
| iota_getObject | Read API | ✅ | ✅ |
| iota_getProtocolConfig | Read API | ✅ | ✅ |
| iota_getTotalTransactionBlocks | Read API | ✅ | ✅ |
| iota_getTransactionBlock | Read API | ✅ | ✅ |
| iota_multiGetObjects | Read API | ✅ | ✅ |
| iota_multiGetTransactionBlocks | Read API | ✅ | ✅ |
| iota_tryGetPastObject | Read API | ❌ | ❌ |
| iota_tryMultiGetPastObjects | Read API | ❌ | ❌ |
| iotax_getAllBalances | Coin Query API | ✅ | ✅ |
| iotax_getAllCoins | Coin Query API | ✅ | ✅ |
| iotax_getBalance | Coin Query API | ✅ | ✅ |
| iotax_getBridgeObjectInitialSharedVersion API DOC? | Read API | ❌ | ❌ |
| iotax_getCirculatingSupply | Coin Query API | ✅ | ✅ |
| iotax_getCoinMetadata | Coin Query API | ✅ | ✅ |
| iotax_getCoins | Coin Query API | ✅ | ✅ |
| iotax_getCommitteeInfo | Governance Read API | ✅ | ✅ |
| iotax_getDynamicFieldObject | Read API | ❌ | ❌ |
| iotax_getDynamicFieldObjectV2 | Read API | ❌ | ❌ |
| iotax_getDynamicFields API DOC? | Read API | ❌ | ❌ | 
| iotax_getLatestBridge API DOC? | Read API | ❌ | ❌ | 
| iotax_getLatestIotaSystemState | Governance Read API | ✅ | ✅ |
| iotax_getLatestIotaSystemStateV2 | Governance Read API | ✅ | ✅ |
| iotax_getOwnedObjects | Read API | ✅ | ✅ |
| iotax_getReferenceGasPrice | Governance Read API | ✅ | ✅ |
| iotax_getStakes | Governance Read API | ✅ | ✅ |
| iotax_getStakesByIds | Governance Read API | ✅ | ✅ |
| iotax_getTimelockedStakes | Governance Read API | ✅ | ✅ |
| iotax_getTimelockedStakesByIds | Governance Read API | ✅ | ✅ |
| iotax_getTotalSupply | Coin Query API | ✅ | ✅ |
| iotax_getValidatorsApy | Governance Read API | ✅ | ✅ |
| iotax_iotaNamesFindAllRegistrationNFTs | Read API | ❌ | ❌ |
| iotax_iotaNamesLookup | Read API | ❌ | ❌ |
| iotax_iotaNamesReverseLookup | Read API | ❌ | ❌ |
| iotax_queryEvents | Event API | ✅ | ✅ |
| iotax_queryTransactionBlocks | Read API | ❌ | ❌ |
| iotax_subscribeEvent | Read API | - | - |
| iotax_subscribeTransaction | Read API | - | - |
| unsafe_batchTransaction | Transaction API | ❌ | ❌ |
| unsafe_mergeCoins | Transaction API | ❌ | ❌ |
| unsafe_moveCall | Transaction API | ❌ | ❌ |
| unsafe_pay | Transaction API | ❌ | ❌ |
| unsafe_payAllIota | Transaction API | ❌ | ❌ |
| unsafe_payIota | Transaction API | ❌ | ❌ |
| unsafe_publish | Transaction API | ❌ | ❌ |
| unsafe_requestAddStake | Transaction API | ❌ | ❌ |
| unsafe_requestAddTimelockedStake | Transaction API | ❌ | ❌ |
| unsafe_requestWithdrawStake | Transaction API | ❌ | ❌ |
| unsafe_requestWithdrawTimelockedStake | Transaction API | ❌ | ❌ |
| unsafe_splitCoin | Transaction API | ❌ | ❌ |
| unsafe_splitCoinEqual | Transaction API | ❌ | ❌ |
| unsafe_transferIota | Transaction API | ❌ | ❌ |
| unsafe_transferObject | Transaction API | ❌ | ❌ |