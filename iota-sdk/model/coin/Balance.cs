﻿using Newtonsoft.Json;

namespace iota_sdk.model.coin;

public class Balance
{
    [JsonProperty("coinType")]
    public string CoinType { get; set; }

    [JsonProperty("coinObjectCount")]
    public decimal CoinObjectCount { get; set; }

    [JsonProperty("totalBalance")]
    public string TotalBalance { get; set; }

    public override string ToString()
    {
        return $"CoinType: {CoinType}, Count: {CoinObjectCount}, Balance: {TotalBalance}";
    }
}