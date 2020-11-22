using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    [Header("Drop")]
    public float m_DropHeight = 60f;

    private float m_ObeliskSpawnTime;
    private float m_SupplyDropTime;
    private bool m_GameOver;

    private SceneLoader m_SceneLoader;
    private GameObject m_Obelisk;
    private GameObject[] m_SupplyDropPrefabs;

    private List<GameObject> m_ObeliskList = new List<GameObject>();
    private List<GameObject> m_SpawnPoints = new List<GameObject>();

    private void Awake()
    {
        m_SceneLoader = GetComponent<SceneLoader>();
        m_Obelisk = Resources.Load<GameObject>("Prefabs/Enemies/Obelisk");
        m_SupplyDropPrefabs = Resources.LoadAll<GameObject>("Prefabs/Pickups");
        m_SpawnPoints.AddRange((FindObjectsOfType(typeof(GameObject)) as GameObject[]).Where(go => go.CompareTag("Spawnable")));        
    }

    private void Start()
    {
        m_ObeliskSpawnTime = GameSettings.Instance.m_ObeliskSpawnTime;
        m_SupplyDropTime = GameSettings.Instance.m_SupplySpawnTime;

        PlayerController.Instance.Initialize();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerHUD.Instance.ShowObjectiveText("Destroy all Obelisks", Color.red);
        
        StartCoroutine(SpawnMoreOverlords());
        StartCoroutine(SpawnSupplyDrop());
    }

    private void Update()
    {
        m_ObeliskList.RemoveAll(item => item == null);

        if (!m_GameOver)
        {
            if (PlayerStats.Instance.HP <= 0 || m_ObeliskList.Count <= 0)
            {
                m_GameOver = true;
                PlayerController.Instance.CanMove = false;
                
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                m_SceneLoader.ShowGameOver(PlayerStats.Instance.HP <= 0 ? "YOU DIED" : "YOU WIN");
            }
        }
    }

    private void SpawnObelisk()
    {
        PlayerHUD.Instance.ShowObjectiveText("Obelisk has spawned", Color.red);
        GameObject go = Instantiate(m_Obelisk, GetRandomSpawnPoint(), Quaternion.identity);
        go.GetComponent<Obelisk>().m_NbVampireBats = GameSettings.Instance.NbVampireBats;
        m_ObeliskList.Add(go);
    }

    private IEnumerator SpawnMoreOverlords()
    {
        SpawnObelisk();

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
            Instantiate(m_SupplyDropPrefabs[0], GetRandomSpawnPoint(), Quaternion.identity);
        }
    }

    private Vector3 GetRandomSpawnPoint()
    {
        GameObject go = m_SpawnPoints [Random.Range(0, m_SpawnPoints.Count)];
        return new Vector3(go.transform.position.x, m_DropHeight, go.transform.position.z);
    }
}