using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats
{
    private const float BASE_HP = 1;
    private const float BASE_DAMAGE = 1;
    private const float BASE_ATTACK_SPEED = 1;

    [Header("Enemy Stats")]
    [SerializeField] public float m_HP;
    [SerializeField] public float m_Damage;
    [SerializeField] public float m_AttackSpeed;
    [SerializeField] public float m_DamageTimer;
    [SerializeField] public float m_MoveSpeed ;
    [SerializeField] public float m_RotationSpeed;
    [SerializeField] public float m_AttackDistance;
    [SerializeField] public float m_AggroDistance;


    public EnemyStats(float hp = BASE_HP, float dmg = BASE_DAMAGE, float atkspd = BASE_ATTACK_SPEED)
    {
        m_HP = hp;
        m_Damage = dmg;
        m_AttackSpeed = atkspd;
        m_DamageTimer = m_AttackSpeed;
    }

    public float HP
    {
        get { return m_HP; }
        set { m_HP = value; }
    }

    public float Damage
    {
        get { return m_Damage; }
    }

    public float AttackSpeed
    {
        get { return m_AttackSpeed; }
        set { m_AttackSpeed = value; }
    }

    public float DamageTimer {
        get { return m_DamageTimer; }
        set { m_DamageTimer = value; }
    }
}
