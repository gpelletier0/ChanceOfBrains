using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    [Header("Player")]
    public PlayerController m_PlayerController;

    [Header("Obelisk")]
    public GameObject m_Obelisk;
    public float m_ObeliskSpawnTime;
    public float m_SupplyDropTime;

    private bool m_BeginGame = false;
    private bool m_EndGame = false;
    
    private List<GameObject> m_ObeliskList = new List<GameObject>();
    
    private GameObject[] m_SupplyDropPrefabs;
    private List<GameObject> m_Spawnable = new List<GameObject>();

    private void Awake()
    {
        m_SupplyDropPrefabs = Resources.LoadAll<GameObject>("Prefabs/Pickups");
    }

    private void Start()
    {
        GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        
        foreach (GameObject go in gos)
        {
            if(go.CompareTag("Spawnable"))
            {
                m_Spawnable.Add(go);
            }
        }

        StartCoroutine(SpawnMoreOverlords());
        StartCoroutine(SpawnSupplyDrop());

        PlayerHUD.Instance.ShowObjectiveText("Destroy all Obelisks", Color.red);
    }

    private void Update()
    {
        if (!m_EndGame)
        {
            if (PlayerStats.Instance.HP <= 0)
            {
                m_PlayerController.CanMove = false;
                PlayerHUD.Instance.ShowObjectiveText("YOU DIED", Color.red);
                PlayerHUD.Instance.Fade(2, eFadeType.OUT);

                StartCoroutine(Pause());
            }
            else if (AreAllObelisksDead())
            {
                m_PlayerController.CanMove = false;
                PlayerHUD.Instance.ShowObjectiveText("YOU WIN", Color.red);
                PlayerHUD.Instance.Fade(2, eFadeType.OUT);
                
                StartCoroutine(Pause());
            }
        }
    }

    private bool AreAllObelisksDead()
    {
        if (m_BeginGame)
        {
            foreach (var go in m_ObeliskList)
            {
                if (go != null)
                    return false;
            }

            return true;
        }

        return false;
    }

    private void SpawnObelisk()
    {
        PlayerHUD.Instance.ShowObjectiveText("Obelisk has spawned", Color.red);
        GameObject go = m_Spawnable[Random.Range(0, m_Spawnable.Count)];
        
        Debug.Log(go.name);

        m_ObeliskList.Add(Instantiate(m_Obelisk, new Vector3(go.transform.position.x, 60, go.transform.position.z), Quaternion.identity));
    }

    private IEnumerator SpawnMoreOverlords()
    {
        SpawnObelisk();
        m_BeginGame = true;
        do
        {
            yield return new WaitForSeconds(m_ObeliskSpawnTime);
            SpawnObelisk();
        }
        while (m_ObeliskList.Count > 0);
    }

    private IEnumerator SpawnSupplyDrop()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_SupplyDropTime);
            PlayerHUD.Instance.ShowObjectiveText("Supply Drop Inbound", Color.red);
            GameObject go = m_Spawnable[Random.Range(0, m_Spawnable.Count)];
            
            go.name = go.name + Random.Range(0, 100);
            Debug.Log(go.name);
            //Debug.LogError("");

            Instantiate(m_SupplyDropPrefabs[0], new Vector3(go.transform.position.x, 60, go.transform.position.z), Quaternion.identity);
        }
    }

    public IEnumerator Pause()
    {
        m_EndGame = true;
        yield return new WaitForSeconds(PlayerHUD.Instance.m_TextDisplayTime);
        // yield return null;
        Time.timeScale = 0;
    }
}