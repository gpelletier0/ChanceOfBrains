using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMain : MonoBehaviour
{
    private const string OBELISK_PREFAB_PATH = "Prefabs/Enemies/Obelisk";


    [Header("Drop")]
    public float m_DropHeight = 60f;

    [Header("Spawn Time")]
    public float m_ObeliskTime = 30f;
    public float m_SupplyTime = 45f;

    private bool m_isGameOver;
    private bool m_isFirstObeliskSpawned;

    private SceneLoader m_SceneLoader;
    private List<GameObject> m_SupplyDropPrefabs = new List<GameObject>();
    private List<GameObject> m_SpawnPoints = new List<GameObject>();


    private void Awake()
    {
        m_SceneLoader = GetComponent<SceneLoader>();
        m_SpawnPoints.AddRange(GameObject.FindGameObjectsWithTag("Spawnable"));

        m_SupplyDropPrefabs = Resources.LoadAll("Prefabs/Pickups", typeof(GameObject))
                                .Cast<GameObject>()
                                .Where(go => go.CompareTag("SupplyDrop"))
                                .ToList();
    }

    private void Start()
    {
        ObjectPooler.Initialize();

        if(GameSettings.Instance.m_ObeliskSpawnTime > 0)
            m_ObeliskTime = GameSettings.Instance.m_ObeliskSpawnTime;
        if(GameSettings.Instance.m_SupplySpawnTime > 0)
            m_SupplyTime = GameSettings.Instance.SupplySpawnTime;

        PlayerController.Instance.Initialize();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerHUD.Instance.ShowObjectiveText("Destroy all Obelisks", Color.red);
        
        StartCoroutine(SpawnMoreOverlords());
        StartCoroutine(SpawnMoreSupplyDrop());
    }

    private void Update()
    {
        if (!m_isGameOver && m_isFirstObeliskSpawned)
        {
            if (PlayerStats.Instance.HP <= 0 || GameObject.FindGameObjectsWithTag("Obelisk").Length == 0)
            {
                m_isGameOver = true;
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

        GameObject go = ObjectPooler.GetPooledObject(OBELISK_PREFAB_PATH);
        go.GetComponent<Obelisk>().m_NbVampireBats = GameSettings.Instance.NbVampireBats;
        go.transform.position = GetRandomSpawnPoint();
        
        go.GetComponent<BoxCollider>().isTrigger = true;
        go.GetComponent<Rigidbody>().isKinematic = false;
        go.SetActive(true);
    }

    private void SpawnSupplyDrop()
    {
        PlayerHUD.Instance.ShowObjectiveText("Supply Drop Inbound", Color.red);
        Instantiate(m_SupplyDropPrefabs[Random.Range(0, m_SupplyDropPrefabs.Count)], GetRandomSpawnPoint(), Quaternion.identity);
    }

    private IEnumerator SpawnMoreOverlords()
    {
        yield return new WaitForSeconds(PlayerHUD.Instance.m_TextDisplayTime);

        SpawnObelisk();
        m_isFirstObeliskSpawned = true;

        while (true)
        {
            yield return new WaitForSeconds(m_ObeliskTime);

            SpawnObelisk();
        }
    }

    private IEnumerator SpawnMoreSupplyDrop()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_SupplyTime);

            SpawnSupplyDrop();
        }
    }

    private Vector3 GetRandomSpawnPoint()
    {
        GameObject go = m_SpawnPoints [Random.Range(0, m_SpawnPoints.Count)];
        return new Vector3(go.transform.position.x, m_DropHeight, go.transform.position.z);
    }
}