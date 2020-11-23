using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]

public class VampireBat : MonoBehaviour, IDamageable
{
    [SerializeReference] public EnemyStats m_EnemyStats;
    public GameObject m_BatSpit;
    public Transform m_FirePoint;
    public float m_RotationSpeed = 5f;
    public float m_GroundDistace = 3f;
    public float m_RotationAdjustX = 40f;


    private bool m_isAlive = true;
    private Animation m_Animation;
    private Transform m_Player;

    [HideInInspector] public Transform m_ObeliskTransform;

    private float RandomSpawnPos() => Random.Range(-3f, 3f);

    private void Awake()
    {
        m_Animation = GetComponent<Animation>();
        m_EnemyStats.DamageTimer = m_EnemyStats.AttackSpeed;
    }

    public void Initialize(Transform t)
    {
        m_ObeliskTransform = t;
        m_EnemyStats.HP = m_EnemyStats.StartingHP;
        m_isAlive = true;

        transform.position = new Vector3(t.transform.position.x + RandomSpawnPos(), t.transform.position.y + m_GroundDistace, t.transform.position.z + RandomSpawnPos());
    }

    private void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (m_isAlive)
        {
            Quaternion rotation = Quaternion.LookRotation(transform.position - m_Player.position);
            rotation *= Quaternion.Euler(m_RotationAdjustX, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);

            float distance = Vector3.Distance(m_Player.position, transform.position);
            
            if (distance >= m_EnemyStats.AttackDistance)
            {
                Vector3 flyTargetPos = new Vector3(m_Player.position.x, m_Player.position.y + m_GroundDistace, m_Player.position.z);
                transform.position = Vector3.MoveTowards(transform.position, flyTargetPos, m_EnemyStats.MoveSpeed * Time.deltaTime);
            }
            else
            {
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
            Debug.Log("Bat is hit for: " + dmg);
            m_EnemyStats.HP -= dmg;
            StartCoroutine(DamageAnimation());
        }
        else
        {
            Die();
        }

    }

    public void Die()
    {
        m_isAlive = false;
        StartCoroutine(Despawn());
    }

    public void GiveDamage()
    {
        StartCoroutine(AttackAnimation());

        GameObject batspit = Instantiate(m_BatSpit, m_FirePoint.position, Quaternion.identity);
        batspit.GetComponent<BatSpit>().m_Target = new Vector3(m_Player.position.x, m_Player.position.y + 1f, m_Player.position.z);
        batspit.GetComponent<BatSpit>().m_Dammage = m_EnemyStats.Damage;
        
        m_EnemyStats.DamageTimer = m_EnemyStats.AttackSpeed;
    }

    private IEnumerator AttackAnimation()
    {
        m_Animation.Play("Attack");
        
        yield return new WaitForSeconds(1.5f);
        
        m_Animation.Play("FlyCycle");
    }

    private IEnumerator DamageAnimation()
    {
        m_Animation.Play("GetDamage");
        
        yield return new WaitForSeconds(0.5f);
        
        m_Animation.Play("FlyCycle");
    }

    private IEnumerator Despawn()
    {
        Debug.Log("Bat Died");
        m_Animation.Play("Die");
        
        yield return new WaitForSeconds(1.5f);

        gameObject.SetActive(false);
    }
}