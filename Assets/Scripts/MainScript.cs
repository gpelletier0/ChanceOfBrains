using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    [Header("HUD")]
    public PlayerHUD m_PlayerHUD;

    [Header("Player")]
    public PlayerController m_PlayerController;

    [Header("Obelisk")]
    public GameObject m_Obelisk;
    public float m_ObeliskSpawnTime;

    private float m_Timer;
    private List<GameObject> m_ObeliskList = new List<GameObject>();

    private void Awake()
    {
        foreach (var t in GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>())
        {
            if (!t.name.Equals("SpawnPoints"))
            {
                Vector3 pos = new Vector3(t.position.x, m_Obelisk.transform.position.y, t.position.z);
                GameObject go = Instantiate(m_Obelisk, pos, Quaternion.identity);
                go.SetActive(false);

                m_ObeliskList.Add(go);
            }
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnMoreOverlords());
    }

    private void Update()
    {
        if (PlayerStats.Instance.HP <= 0)
        {
            m_PlayerController.CanMove = false;
            m_PlayerHUD.ShowObjectiveText("YOU DIED", Color.red);
            Invoke("m_PlayerHUD.HideObjectiveText", m_PlayerHUD.m_TextDisplayTime);
            m_PlayerHUD.Fade(2, eFadeType.OUT);
        }
        else if (AreAllObelisksDead())
        {
            m_PlayerController.CanMove = false;
            m_PlayerHUD.ShowObjectiveText("YOU WIN", Color.red);
            Invoke("m_PlayerHUD.HideObjectiveText", m_PlayerHUD.m_TextDisplayTime);
            m_PlayerHUD.Fade(2, eFadeType.OUT);
        }
    }

    private bool AreAllObelisksDead()
    {
        foreach(var go in m_ObeliskList)
        {
            if (go != null)
                return false;
        }

        return true;
    }

    private IEnumerator SpawnMoreOverlords()
    {
        foreach (var go in m_ObeliskList)
        {
            yield return new WaitForSeconds(m_ObeliskSpawnTime);
            go.SetActive(true);
        }
    }
}