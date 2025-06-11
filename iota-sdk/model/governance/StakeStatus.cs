namespace iota_sdk.model.governance;

/// <summary>
/// Represents the status of a stake
/// </summary>
public enum StakeStatus
{
    /// <summary>
    /// The stake is active and earning rewards
    /// </summary>
    Active,
        
    /// <summary>
    /// The stake is pending activation
    /// </summary>
    Pending,
        
    /// <summary>
    /// The stake has been unstaked
    /// </summary>
    Unstaked
}