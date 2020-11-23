﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class Obelisk : MonoBehaviour, IDamageable
{
    private const string VAMPIRE_BAT_PREFAB = "Prefabs/Enemies/VampireBat";
    private const string ZOMBIE_PREFAB = "Prefabs/Enemies/Zombie";

    [SerializeReference] public EnemyStats m_EnemyStats = new EnemyStats(10);
    public float m_VampireBatSpawnTime = 2.0f;
    public float m_ZombieSpawnTime = 10.0f;
    public float m_NbVampireBats = 1;

    public List<GameObject> m_EnemyPrefabs;
    public List<GameObject> m_PickupDrops;

    private List<GameObject> m_VampireBatList = new List<GameObject>();
    private AudioSource m_AudioSource;


    private float RandomSpawnPos() => Random.Range(-3f, 3f);

    private void Awake()
    {
        name += Random.Range(0, 100).ToString();
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Roads"))
        {
            GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<Rigidbody>().isKinematic = true;
            transform.position = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);
            
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
        else
        {
            Die();
        }
    }

    public void GiveDamage(){}

    public void Die()
    {
        Debug.Log("Obelisk Died");

        gameObject.SetActive(false);

        Debug.Log("Obelisk Drops Pickups");
        foreach (GameObject go in m_PickupDrops)
        {
            Vector3 pos = new Vector3(transform.position.x + RandomSpawnPos(), transform.position.y + 2, transform.position.z + RandomSpawnPos());
            Instantiate(go, pos, Quaternion.identity);
        }
    }

    private IEnumerator SpawnEnemy()
    {
        float timer = 0f;
        while (timer < m_VampireBatSpawnTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        for(int i = 0; i < m_NbVampireBats; i++)
        {
            Debug.Log($"{name} Spawns VampireBat");
            GameObject go = ObjectPooler.GetPooledObject(VAMPIRE_BAT_PREFAB);
            go.transform.position = new Vector3(transform.position.x + RandomSpawnPos(), transform.position.y + 2, transform.position.z + RandomSpawnPos());
            go.SetActive(true);
        }
        
        while (true)
        {
            timer = 0f;
            while (timer < m_ZombieSpawnTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            Debug.Log($"{name} Spawns Zombie");

            Vector3 pos = new Vector3(transform.position.x + RandomSpawnPos(), 0f, transform.position.z + RandomSpawnPos());
            GameObject z = Instantiate(m_EnemyPrefabs[1], pos, Quaternion.identity);
            z.GetComponent<Zombie>().m_Obelisk = transform;
        }
    }
}