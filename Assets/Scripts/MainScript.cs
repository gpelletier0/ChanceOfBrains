using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    [Header("Player")]
    public PlayerController m_PlayerController;

    [Header("Drop")]
    public float m_DropHeight = 60;
    
    [Header("Obelisk")]
    public float m_ObeliskSpawnTime;

    [Header("Supply Drop")]
    public float m_SupplyDropTime = 40;

    private GameObject m_Obelisk;
    private GameObject[] m_SupplyDropPrefabs;
    private List<GameObject> m_ObeliskList = new List<GameObject>();
    private List<GameObject> m_SpawnPoints = new List<GameObject>();

    private void Awake()
    {
        //GameObject.Find("GameOverCanvas").SetActive(false);

        m_SupplyDropPrefabs = Resources.LoadAll<GameObject>("Prefabs/Pickups");
        m_Obelisk = Resources.Load<GameObject>("Prefabs/Enemies/Obelisk");

        m_SpawnPoints.AddRange((FindObjectsOfType(typeof(GameObject)) as GameObject[])
            .Where(go => go.CompareTag("Spawnable")));
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(SpawnMoreOverlords());
        StartCoroutine(SpawnSupplyDrop());

        PlayerHUD.Instance.ShowObjectiveText("Destroy all Obelisks", Color.red);
    }

    private void Update()
    {
           
    }

    private void FixedUpdate()
    {
        if (PlayerStats.Instance.HP <= 0 || m_ObeliskList.Count <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GetComponent<SceneLoader>().ShowGameOver(PlayerStats.Instance.HP <= 0 ? "YOU DIED" : "YOU WIN");
        }
    }


    private void SpawnObelisk()
    {
        PlayerHUD.Instance.ShowObjectiveText("Obelisk has spawned", Color.red);

        GameObject go = m_SpawnPoints[Random.Range(0, m_SpawnPoints.Count)];
        m_ObeliskList.Add(Instantiate(m_Obelisk, new Vector3(go.transform.position.x, m_DropHeight, go.transform.position.z), Quaternion.identity));
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

            Vector3 pos = new Vector3(m_SupplyDropPrefabs[0].transform.position.x, m_DropHeight, m_SupplyDropPrefabs[0].transform.position.z);
            Instantiate(m_SupplyDropPrefabs[0], pos, Quaternion.identity);
        }
    }
}