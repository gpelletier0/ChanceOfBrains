using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject m_MainCanvas;
    public GameObject m_SettingsCanvas;

    public void OnStartGame()
    {
        SceneManager.LoadScene("Map_v1");
    }

    public void OnSettings()
    {
        m_MainCanvas.SetActive(!m_MainCanvas.activeSelf);
        m_SettingsCanvas.SetActive(!m_SettingsCanvas.activeSelf);
    }
}
