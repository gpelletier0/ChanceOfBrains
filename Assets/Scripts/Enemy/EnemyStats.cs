using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats
{
    [Header("Stats")]
    [SerializeField] private float m_HP;
    [SerializeField] private float m_Damage;
    [SerializeField] private float m_DamageTimer;
    [SerializeField] private float m_AttackSpeed;

    private const float BASE_HP = 2;
    private const float BASE_DAMAGE = 2;
    private const float BASE_ATTACK_SPEED = 2;

    public EnemyStats(float hp = BASE_HP, float dmg = BASE_DAMAGE, float atkspd = BASE_ATTACK_SPEED)
    {
        m_HP = hp;
        m_Damage = dmg;
        m_AttackSpeed = atkspd;
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

    public float DamageTimer
    {
        get { return m_DamageTimer; }
        set { m_DamageTimer = value; }
    }

    public float AttackSpeed
    {
        get { return m_AttackSpeed; }
        set { m_AttackSpeed = value; }
    }
}
