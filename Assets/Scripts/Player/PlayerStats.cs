using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    public const float DEFAULT_STAT = 100;

    #region Singleton
    private static readonly PlayerStats instance = new PlayerStats();

    static PlayerStats()
    {
    }

    private PlayerStats()
    {
    }

    public static PlayerStats Instance
    {
        get { return instance; }
    }
    #endregion


    public void Defaults()
    {
        HP = DEFAULT_STAT;
        MaxHP = DEFAULT_STAT;
        ST= DEFAULT_STAT;
        MaxST= DEFAULT_STAT;
        MinJumpST = 0;
        AmmoCount = DEFAULT_STAT;
    }

    #region Getters/Setters
    // HP
    public float HP { get; set; }

    public float MaxHP { get; set; } = DEFAULT_STAT;


    // Stamina
    public float ST { get; set; }

    public float MaxST { get; set; } = DEFAULT_STAT;

    public float MinJumpST { get; set; }

    public float AmmoCount { get; set; }

    #endregion
}