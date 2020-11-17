using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour, IDamageable
{
    public float m_AggroDistance = 20f;
    public float m_AttackDistance = 1f;
    private NavMeshPath m_navMeshPath;
    private NavMeshAgent m_Agent;
    private Animator m_Animator;
    private Transform m_Target;
    private EnemyStats m_EnemyStats = new EnemyStats(2);

    private void Awake()
    {
        m_navMeshPath = new NavMeshPath();
        m_Agent = gameObject.GetComponent<NavMeshAgent>();
        m_Animator = gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        m_Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        float distance = Vector3.Distance(m_Target.position, transform.position);
        if (distance < m_AggroDistance)
        {
            m_Agent.CalculatePath(m_Target.position, m_navMeshPath);
            if (m_navMeshPath.status == NavMeshPathStatus.PathComplete)
            {
                m_Agent.SetDestination(m_Target.position);
            }
        }
        m_Animator.SetBool("isAttacking", distance < m_AttackDistance ? true : false);
        m_Animator.SetBool("isMoving", m_Agent.velocity != Vector3.zero ? true : false);

        if (m_Animator.GetBool("isAttacking"))
        {
            if (m_EnemyStats.DamageTimer <= 0)
                GiveDamage();
        }

        if (m_EnemyStats.DamageTimer > 0)
            m_EnemyStats.DamageTimer -= Time.deltaTime;
    }

    private void ReturnToObelisk()
    {

    }

    public void TakeDamage(float dmg)
    {
        if(m_EnemyStats.HP >= 0)
        {
            Debug.Log("Zombie is hit for: " + dmg);
            m_EnemyStats.HP -= dmg;
        }

        if(m_EnemyStats.HP <= 0)
        {
            Die();
        }
            
    }

    public void GiveDamage()
    {
        m_EnemyStats.DamageTimer = m_EnemyStats.AttackSpeed;
        Debug.Log("Zombie hits for: " + m_EnemyStats.Damage);
        GameObject.Find("Player").GetComponent<PlayerController>().TakeDamage(m_EnemyStats.Damage);
    }

    public void Die()
    {
        Debug.Log("Zombie Died");
        Destroy(gameObject);
    }
}