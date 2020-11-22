using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMain : MonoBehaviour
{
    [Header("Drop")]
    public float m_DropHeight = 60f;

    [Header("Spawn Time")]
    public float m_ObeliskTime = 30f;
    public float m_SupplyTime = 45f;

    private bool m_GameOver;

    private SceneLoader m_SceneLoader;
    private GameObject m_Obelisk;

    private List<GameObject> m_SupplyDropPrefabs = new List<GameObject>();
    private List<GameObject> m_ObeliskList = new List<GameObject>();
    private List<GameObject> m_SpawnPoints = new List<GameObject>();

    private void Awake()
    {
        m_SceneLoader = GetComponent<SceneLoader>();
        m_SpawnPoints.AddRange(GameObject.FindGameObjectsWithTag("Spawnable"));
        
        m_Obelisk = Resources.Load<GameObject>("Prefabs/Enemies/Obelisk");
        m_SupplyDropPrefabs = Resources.LoadAll("Prefabs/Pickups", typeof(GameObject))
                                .Cast<GameObject>()
                                .Where(go => go.tag.Equals("SupplyDrop"))
                                .ToList();
    }

    private void Start()
    {
        if(GameSettings.Instance.m_ObeliskSpawnTime > 0)
            m_ObeliskTime = GameSettings.Instance.m_ObeliskSpawnTime;
        if(GameSettings.Instance.m_SupplySpawnTime > 0)
            m_SupplyTime = GameSettings.Instance.SetSupplySpawnTime;

        PlayerController.Instance.Initialize();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerHUD.Instance.ShowObjectiveText("Destroy all Obelisks", Color.red);

        StartCoroutine(SpawnMoreOverlords());
        StartCoroutine(SpawnMoreSupplyDrop());
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

    private void SpawnSupplyDrop()
    {

        PlayerHUD.Instance.ShowObjectiveText("Supply Drop Inbound", Color.red);
        Instantiate(m_SupplyDropPrefabs[Random.Range(0, m_SupplyDropPrefabs.Count)], GetRandomSpawnPoint(), Quaternion.identity);
    }

    private IEnumerator SpawnMoreOverlords()
    {
        SpawnObelisk();

        while (m_ObeliskList.Count > 0)
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