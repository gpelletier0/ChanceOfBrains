using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    public GameObject m_GameOverCanvas;
    TextMeshProUGUI m_DisplayText;


    /// <summary>
    /// MonoBehaviour
    /// </summary>
    protected override void Awake()
    {
        m_DisplayText = m_GameOverCanvas.GetComponentInChildren<TextMeshProUGUI>();
    }
    
    /// <summary>
    /// Displays game over canvas
    /// </summary>
    /// <param name="str"></param>
    public void ShowGameOver(string str)
    {
        m_GameOverCanvas.SetActive(true);
        m_DisplayText.text = str;
        Time.timeScale = 0;
    }

    /// <summary>
    /// Reloads the scene
    /// </summary>
    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }


    /// <summary>
    /// Quits the game
    /// </summary>
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
