using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]

public class Zombie : MonoBehaviour, IDamageable
{
    [SerializeReference] private EnemyStats m_EnemyStats;
    public float m_DestroyTime = 5f;


    private NavMeshPath m_navMeshPath;
    private NavMeshAgent m_Agent;

    private Collider m_Collider;
    private Transform m_Player;
    private Transform m_ObeliskTransform;

    [HideInInspector] public Animator m_Animator;

    /// <summary>
    /// Generate radom spawn position
    /// </summary>
    /// <returns></returns>
    private float RandomSpawnPos() => Random.Range(-3f, 3f);

    /// <summary>
    /// MonoBehaviour
    /// </summary>
    private void Awake()
    {
        m_navMeshPath = new NavMeshPath();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        m_Collider = GetComponent<CapsuleCollider>();
    }


    /// <summary>
    /// Initialize Class to default
    /// </summary>
    /// <param name="t"> transform of spawning obelisk </param>
    public void Initialize(Transform t)
    {
        m_EnemyStats.HP = m_EnemyStats.StartingHP;
        m_Animator.SetBool("isDead", false);
        m_Agent.enabled = true;
        m_Collider.enabled = true;

        m_ObeliskTransform = t;
        transform.position = new Vector3(t.transform.position.x + RandomSpawnPos(), t.position.y, t.transform.position.z + RandomSpawnPos());
    }

    private void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    /// <summary>
    /// MonoBehaviour
    /// </summary>
    private void Update()
    {
        if (m_EnemyStats.HP <= 0 && !m_Animator.GetBool("isDead"))
            Die();

        if (m_Agent.isActiveAndEnabled)
        {
            float distance = Vector3.Distance(m_Player.position, transform.position);

            if (distance < m_EnemyStats.AggroDistance)
            {
                m_Agent.CalculatePath(m_Player.position, m_navMeshPath);

                if (m_navMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                    m_Agent.SetDestination(m_Player.position);
                }
            }
            else if (m_ObeliskTransform != null)
            {
                m_Agent.SetDestination(m_ObeliskTransform.position);
            }

            m_Animator.SetBool("isAttacking", distance < m_EnemyStats.AttackDistance);
            m_Animator.SetBool("isMoving", m_Agent.velocity != Vector3.zero);

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


    /// <summary>
    /// IDamagable TakeDamage implementation
    /// </summary>
    public void TakeDamage(float dmg)
    {
        if (m_EnemyStats.HP >= 0)
        {
            Debug.Log("Zombie is hit for: " + dmg);
            m_EnemyStats.HP -= dmg;
        }
    }

    /// <summary>
    /// IDamagable GiveDamage implementation
    /// </summary>
    public void GiveDamage()
    {
        m_EnemyStats.DamageTimer = m_EnemyStats.AttackSpeed;
        Debug.Log("Zombie hits for: " + m_EnemyStats.Damage);
        PlayerStats.Instance.HP -= m_EnemyStats.Damage;
    }

    /// <summary>
    /// IDamagable Die implementation
    /// </summary>
    public void Die()
    {
        m_Animator.SetBool("isDead", true);
        m_Agent.enabled = false;
        m_Collider.enabled = false;

        StartCoroutine(Despawn());
    }

    /// <summary>
    /// Coroutine deactivating the gameobject
    /// </summary>
    /// <returns></returns>
    private IEnumerator Despawn()
    {
        Debug.Log("Zombie Died");

        yield return new WaitForSeconds(m_DestroyTime);

        gameObject.SetActive(false);
    }
}