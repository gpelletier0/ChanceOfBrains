using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obelisk : MonoBehaviour, IDamageable
{
    [SerializeReference] public EnemyStats m_EnemyStats = new EnemyStats(10);
    public float m_VampireBatSpawnTime = 2.0f;
    public float m_ZombieSpawnTime = 10.0f;
    public List<GameObject> m_EnemyPrefabs;
    public List<GameObject> m_PickupDrops;

    private AudioSource m_AudioSource;


    private float RandomSpawnPos() => Random.Range(-3, 3);

    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();

        Vector3 pos = new Vector3(transform.position.x + RandomSpawnPos(), m_EnemyPrefabs[0].transform.position.y, transform.position.z + RandomSpawnPos());
        m_EnemyPrefabs[0] = Instantiate(m_EnemyPrefabs[0], pos, Quaternion.identity);
        m_EnemyPrefabs[0].SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Roads"))
        {
            GetComponent<BoxCollider>().isTrigger = false;
            StartCoroutine(SpawnEnemy());
        }   
    }

    public void TakeDamage(float dmg)
    {
        if (m_EnemyStats.HP >= 0)
        {
            Debug.Log($"Obelisk is hit for: {dmg}");
            m_EnemyStats.HP -= dmg;
        }

        if (m_EnemyStats.HP <= 0)
            Die();
    }

    public void GiveDamage(){}

    public void Die()
    {
        Debug.Log("Obelisk Died");
        m_AudioSource.Stop();
        Destroy(gameObject);

        Debug.Log("Drop Pickups");
        foreach (GameObject go in m_PickupDrops)
        {
            Vector3 pos = new Vector3(transform.position.x + RandomSpawnPos(), transform.position.y + 2, transform.position.z + RandomSpawnPos());
            Instantiate(go, pos, Quaternion.identity);
        }
    }

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(m_VampireBatSpawnTime);

        m_EnemyPrefabs[0].SetActive(true);

        while (true)
        {
            yield return new WaitForSeconds(m_ZombieSpawnTime);
            
            Debug.Log("Spawn Enemy");
            Vector3 pos = new Vector3(transform.position.x + RandomSpawnPos(), 0, transform.position.z + RandomSpawnPos());
            Instantiate(m_EnemyPrefabs[1], pos, Quaternion.identity);
        }
    }
}