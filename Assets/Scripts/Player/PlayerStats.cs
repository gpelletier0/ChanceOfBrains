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


    public void Defaults(float ammoCount)
    {
        HP = DEFAULT_STAT;
        maxHP = DEFAULT_STAT;
        ST= DEFAULT_STAT;
        maxST= DEFAULT_STAT;
        minJumpST = 0;
        AmmoCount = DEFAULT_STAT;
    }

    #region Getters/Setters
    // HP
    public float HP { get; set; }

    public float maxHP { get; set; } = DEFAULT_STAT;


    // Stamina
    public float ST { get; set; }

    public float maxST { get; set; } = DEFAULT_STAT;

    public float minJumpST { get; set; }

    public float AmmoCount { get; set; }

    #endregion
}