using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]

public class Zombie : MonoBehaviour, IDamageable
{
    [SerializeReference] private EnemyStats m_EnemyStats = new EnemyStats(2, 1, 1);
    public float m_AggroDistance = 20f;
    public float m_AttackDistance = 1f;
    public float m_DestroyTime = 5f;

    
    private NavMeshPath m_navMeshPath;
    private NavMeshAgent m_Agent;
    private Animator m_Animator;
    private Collider m_Collider;
    private Transform m_Player;
    [HideInInspector] public Transform m_Obelisk;

    private void Awake()
    {
        m_navMeshPath = new NavMeshPath();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        m_Collider = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (m_EnemyStats.HP <= 0 && !m_Animator.GetBool("isDead"))
            Die();

        if (m_Agent.isActiveAndEnabled)
        {
            float distance = Vector3.Distance(m_Player.position, transform.position);
            
            if (distance < m_AggroDistance)
            {
                m_Agent.CalculatePath(m_Player.position, m_navMeshPath);
                
                if (m_navMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                    m_Agent.SetDestination(m_Player.position);
                }
            }
            else if (m_Obelisk != null)
            {
                m_Agent.SetDestination(m_Obelisk.position);
            }

            m_Animator.SetBool("isAttacking", distance < m_AttackDistance ? true : false);
            m_Animator.SetBool("isMoving", m_Agent.velocity != Vector3.zero ? true : false);

            if (m_Animator.GetBool("isAttacking"))
            {
                Quaternion rotation = Quaternion.LookRotation(m_Player.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);

                if (m_EnemyStats.DamageTimer <= 0)
                    GiveDamage();      
            }

            if (m_EnemyStats.DamageTimer > 0)
                m_EnemyStats.DamageTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(float dmg)
    {
        if (m_EnemyStats.HP >= 0)
        {
            Debug.Log("Zombie is hit for: " + dmg);
            m_EnemyStats.HP -= dmg;
        }
    }

    public void GiveDamage()
    {
        m_EnemyStats.DamageTimer = m_EnemyStats.AttackSpeed;
        Debug.Log("Zombie hits for: " + m_EnemyStats.Damage);
        PlayerStats.Instance.HP -= m_EnemyStats.Damage;
    }

    public void Die()
    {
        m_Animator.SetBool("isDead", true);
        m_Agent.enabled = false;
        m_Collider.enabled = false;
        
        StartCoroutine(Despawn());
    }

    private IEnumerator Despawn()
    {
        Debug.Log("Zombie Died");
        yield return new WaitForSeconds(m_DestroyTime);
        Destroy(gameObject);
    }
}