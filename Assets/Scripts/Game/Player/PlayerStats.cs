using UnityEngine;

/// <summary>
/// Player Stats singleton class
/// </summary>
public class PlayerStats : Singleton<PlayerStats>
{
    public const float DEFAULT_STAT = 100;
    
    #region Getters/Setters
    
    // HP
    public float HP { get; set; } = DEFAULT_STAT;

    public float MaxHP { get; set; } = DEFAULT_STAT;


    // Stamina
    public float ST { get; set; } = DEFAULT_STAT;

    public float MaxST { get; set; } = DEFAULT_STAT;

    public float MinJumpST { get; set; } = 0;

    public float AmmoCount { get; set; } = 0;

    #endregion
}