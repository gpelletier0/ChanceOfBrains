using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerStats
{
    private const float DEFAULT_STAT = 100;
    private const float DEFAULT_JUMP_ST = 20;

    [Header("Stats")]
    [SerializeField] float m_HP;
    [SerializeField] float m_maxHP;
    [SerializeField] float m_ST;
    [SerializeField] float m_maxST;
    [SerializeField] float m_minJumpST;
    [SerializeField] float m_AmmoCount;

    #region Data
    [Header("Data")]
    [HideInInspector] public LayerMask layersCanShoot;
    [HideInInspector] public float rateOfFire;
    [HideInInspector] public float shotDamage;
    [HideInInspector] public float shotRange;
    [HideInInspector] public float timeCanNextShoot;
    [HideInInspector] public float timeOfVisibleShotRenderer;
    #endregion


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


    public void Initalize(float ammoCount)
    {
        m_HP = DEFAULT_STAT;
        m_maxHP = DEFAULT_STAT;
        m_ST= DEFAULT_STAT;
        m_maxST= DEFAULT_STAT;
        m_minJumpST = DEFAULT_JUMP_ST;
        m_AmmoCount = ammoCount;
    }

    #region Getters/Setters
    // HP
    public float HP
    {
        get { return m_HP; }
        set { m_HP = value; }
    }

    public float maxHP
    {
        get { return m_maxHP; }
    }


    // Stamina
    public float ST
    {
        get { return m_ST; }
        set { m_ST = value; }
    }

    public float maxST
    {
        get { return m_maxST; }
    }

    public float minJumpST 
    {
        get { return m_minJumpST; }
    }

    public float AmmoCount
    {
        get { return m_AmmoCount; }
        set { m_AmmoCount = value; }
    }

    #endregion
}