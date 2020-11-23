using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats
{
    [Header("Enemy Stats")]
    [SerializeField] private float m_HP;
    [SerializeField] private float m_StartingHP;
    [SerializeField] private float m_Damage;
    [SerializeField] private float m_AttackSpeed;
    [SerializeField] private float m_DamageTimer;
    [SerializeField] private float m_MoveSpeed ;
    [SerializeField] private float m_AttackDistance;
    [SerializeField] private float m_AggroDistance;

    #region Getters/Setters
    
    public float HP
    {
        get => m_HP;
        set => m_HP = value;
    }

    public float StartingHP
    {
        get => m_StartingHP;
    }

    public float Damage
    {
        get => m_Damage;
    }

    public float AttackSpeed
    {
        get => m_AttackSpeed;
    }

    public float DamageTimer 
    {
        get => m_DamageTimer;
        set => m_DamageTimer = value;
    }

    public float MoveSpeed
    {
        get => m_MoveSpeed;
    }

    public float AttackDistance
    {
        get => m_AttackDistance;
    }

    public float AggroDistance
    {
        get => m_AggroDistance;
    }

    #endregion
}
