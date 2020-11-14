using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    [Header("HUD")]
    public PlayerHUD m_PlayerHUD;

    [Header("Player")]
    public PlayerController m_PlayerController;

    private bool bEndGame = false;

    void Start()
    {
        PlayerStats.Instance.Initalize();   
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerStats.Instance.HP <= 0 && !bEndGame)
        {
            m_PlayerController.CanMove = false;
            m_PlayerHUD.ShowObjectiveText("YOU DIED!", Color.red);
            Invoke("m_PlayerHUD.HideObjectiveText", m_PlayerHUD.m_TextDisplayTime);
            m_PlayerHUD.Fade(2, eFadeType.OUT);
            bEndGame = true;
            //Invoke end scene
        }
    }
}
