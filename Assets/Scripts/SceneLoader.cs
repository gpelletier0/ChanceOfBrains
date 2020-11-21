using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject m_GameOverCanvas;
    TextMeshProUGUI m_DisplayText;

    private void Awake()
    {
        m_DisplayText = m_GameOverCanvas.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ShowGameOver(string str)
    {
        m_GameOverCanvas.SetActive(true);
        m_DisplayText.text = str;
        Time.timeScale = 0;
    }
    public void ReloadGame()
    {
        PlayerStats.Instance.HP = PlayerController.Instance.m_HP;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
