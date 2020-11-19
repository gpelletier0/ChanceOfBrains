using System.Collections;
using UnityEngine;

public class Bat : MonoBehaviour, IDamageable
{
    [SerializeReference] public EnemyStats m_EnemyStats = new EnemyStats(4, 2, 2);
    [SerializeField] public GameObject m_BatSpit;
    [SerializeField] public Transform m_FirePoint;
    public float m_MoveSpeed = 3f;
    public float m_RotationSpeed = 5f;
    public float m_AttackDistance = 5f;
    public float m_AggroDistance = 20f;
    public float m_GroundDistace = 3f;
    public float m_RotationAdjustX = 40f;

    private Transform m_Target;
    private Animation m_Animation;
    private const string BULLET_PREFAB = "Prefabs/Weapons/Bullet";

    private void Awake()
    {
        m_Animation = GetComponent<Animation>();
        m_EnemyStats.DamageTimer = m_EnemyStats.AttackSpeed;
    }

    private void Start()
    {
        m_Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        Quaternion rotation = Quaternion.LookRotation(transform.position - m_Target.position);
        rotation *= Quaternion.Euler(m_RotationAdjustX, 0, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
        
        float distance = Vector3.Distance(m_Target.position, transform.position);
        
        if (distance < m_AggroDistance && distance >= m_AttackDistance)
        {
            Vector3 flyTargetPos = new Vector3(m_Target.position.x, m_Target.position.y + m_GroundDistace, m_Target.position.z);
            transform.position = Vector3.MoveTowards(transform.position, flyTargetPos, m_MoveSpeed * Time.deltaTime);
        }

        if (distance <= m_AttackDistance && m_EnemyStats.DamageTimer <= 0)
            GiveDamage();
        else
            m_EnemyStats.DamageTimer -= Time.deltaTime;

    }

    private void ReturnToObelisk()
    {

    }

    public void TakeDamage(float dmg)
    {
        if (m_EnemyStats.HP >= 0)
        {
            Debug.Log("Bat is hit for: " + dmg);
            m_EnemyStats.HP -= dmg;
        }
        else
        {
            Die();
        }

    }

    public void Die()
    {
        Debug.Log("Bat Died");
        Destroy(gameObject);
    }

    public void GiveDamage()
    {
        GameObject batspit = Instantiate(m_BatSpit, m_FirePoint.position, Quaternion.identity);
        batspit.GetComponent<BatSpit>().m_Target = m_Target;
        batspit.GetComponent<BatSpit>().m_Dammage = m_EnemyStats.Damage;
        
        m_EnemyStats.DamageTimer = m_EnemyStats.AttackSpeed;
    }
}