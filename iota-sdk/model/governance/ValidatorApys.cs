using System.Numerics;
using Newtonsoft.Json;

namespace iota_sdk.model.governance;

/// <summary>
/// Represents APY (Annual Percentage Yield) information for validators at a specific epoch
/// </summary>
public class ValidatorApys
{
    /// <summary>
    /// List of validator APY entries
    /// </summary>
    [JsonProperty("apys")]
    public List<ValidatorApy> Apys { get; set; } = new List<ValidatorApy>();

    /// <summary>
    /// The epoch for which these APY values are valid
    /// </summary>
    [JsonProperty("epoch")]
    public BigInteger Epoch { get; set; }

    public ValidatorApys()
    {
    }

    public ValidatorApys(List<ValidatorApy> apys, BigInteger epoch)
    {
        Apys = apys;
        Epoch = epoch;
    }
}

/// <summary>
/// Represents APY information for a specific validator
/// </summary>
public class ValidatorApy
{
    /// <summary>
    /// The address of the validator
    /// </summary>
    [JsonProperty("address")]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// The Annual Percentage Yield for the validator
    /// </summary>
    [JsonProperty("apy")]
    public double Apy { get; set; }

    public ValidatorApy()
    {
    }

    public ValidatorApy(string address, double apy)
    {
        Address = address;
        Apy = apy;
    }
}