using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obelisk : MonoBehaviour, IDamageable
{
    private const int NO_VAMPIREBAT = 1;
    private const int NO_ZOMBIES = 10;
    
    private const string BAT_PREFAB = "Prefabs/Enemies/VampireBat";
    private const string ZOMBIE_PREFAB = "Prefabs/Enemies/Zombie";
    

    public EnemyStats m_EnemyStats = new EnemyStats(10);
    public List<GameObject> m_ZombiesList;
    public List<GameObject> m_BatsList;

    private void Awake()
    {
        for(int i = 0; i < NO_VAMPIREBAT; i++)
        {
            GameObject bat = Instantiate(Resources.Load<GameObject>(BAT_PREFAB));
            bat.SetActive(false);
            m_BatsList.Add(bat);
        }

        for(int i = 0; i < NO_ZOMBIES; i++)
        {
            GameObject zombie = Instantiate(Resources.Load<GameObject>(ZOMBIE_PREFAB));
            zombie.SetActive(false);
            m_ZombiesList.Add(zombie);
        }
    }

    public void Update()
    {
        
    }

    public void Die()
    {
        Debug.Log("Obelisk Died");
        Destroy(gameObject);
    }

    public void GiveDamage()
    {

    }

    public void TakeDamage(float dmg)
    {
        if (m_EnemyStats.HP >= 0)
        {
            Debug.Log($"Obelisk is hit for: {dmg}");
            m_EnemyStats.HP -= dmg;
        }

        if (m_EnemyStats.HP <= 0)
        {
            Die();
        }
    }
}
