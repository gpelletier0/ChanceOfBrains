using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy Stats Class
/// </summary>
public class EnemyStats
{
    [Header("Enemy Stats")]
    [SerializeField] private float m_HP = 1;
    [SerializeField] private float m_StartingHP = 1;
    [SerializeField] private float m_Damage = 1;
    [SerializeField] private float m_AttackSpeed = 1;
    [SerializeField] private float m_DamageTimer = 1;
    [SerializeField] private float m_MoveSpeed = 1;
    [SerializeField] private float m_AttackDistance = 1;
    [SerializeField] private float m_AggroDistance = 1;

    #region Getters/Setters
    
    /// <summary>
    /// HP Management
    /// </summary>
    public float HP
    {
        get => m_HP;
        set => m_HP = value;
    }

    /// <summary>
    /// Save Starting HP for object pooling
    /// </summary>
    public float StartingHP
    {
        get => m_StartingHP;
    }


    /// <summary>
    /// ReadOnly Damage
    /// </summary>
    public float Damage
    {
        get => m_Damage;
    }


    /// <summary>
    /// ReadOnly AttackSpeed
    /// </summary>
    public float AttackSpeed
    {
        get => m_AttackSpeed;
    }


    /// <summary>
    /// Timer for all mobs
    /// </summary>
    public float DamageTimer 
    {
        get => m_DamageTimer;
        set => m_DamageTimer = value;
    }
    
    /// <summary>
    /// ReadOnly MoveSpeed
    /// </summary>
    public float MoveSpeed
    {
        get => m_MoveSpeed;
    }


    /// <summary>
    /// ReadOnly AttackDistance
    /// </summary>
    public float AttackDistance
    {
        get => m_AttackDistance;
    }


    /// <summary>
    /// ReadOnly Aggro Distance
    /// </summary>
    public float AggroDistance
    {
        get => m_AggroDistance;
    }

    #endregion
}
