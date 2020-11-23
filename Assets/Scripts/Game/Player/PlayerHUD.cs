using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Fade state
/// </summary>
public enum eFadeType { IN, OUT }


public class PlayerHUD : Singleton<PlayerHUD>
{
    public float m_TextDisplayTime = 4.0f;

    private GameObject m_crosshair;
    private Text m_HPPct;
    private Text m_STPct;
    private Text m_AmmoCount;
    private Text m_ObjectiveText;
    private Text m_ObjectInteractionText;
    private Image m_FadeImage;

    // Fade
    private float m_FadeAlpha = 1.0f;
    private IEnumerator m_FadeCoroutine;

    /// <summary>
    /// MonoBehaviour
    /// </summary>
    protected override void Awake()
    {
        m_crosshair = GameObject.Find("Crosshair");
        m_HPPct = GameObject.Find("HPPct").GetComponent<Text>();
        m_STPct = GameObject.Find("STPct").GetComponent<Text>();
        m_AmmoCount = GameObject.Find("AmmoCount").GetComponent<Text>();
        m_ObjectiveText = GameObject.Find("ObjectiveText").GetComponent<Text>();
        m_ObjectInteractionText = GameObject.Find("ObjectInteractionText").GetComponent<Text>();
        m_FadeImage = GameObject.Find("FadeImage").GetComponent<Image>();
    }

    /// <summary>
    /// MonoBehaviour
    /// </summary>
    private void Start()
    {
        if (m_FadeImage)
        {
            Color color = m_FadeImage.color;
            color.a = m_FadeAlpha;
            m_FadeImage.color = color;

            Fade(m_TextDisplayTime * 2, eFadeType.IN);
        }
    }

    /// <summary>
    /// Fade the image alpha
    /// </summary>
    /// <param name="seconds"> seconds of fade </param>
    /// <param name="direction"> in or out enum </param>
    public void Fade(float seconds, eFadeType direction)
    {
        if (m_FadeCoroutine != null) 
            StopCoroutine(m_FadeCoroutine);

        float targetFade;

        if (direction.Equals(eFadeType.IN))
            targetFade = 0.0f;
        else
            targetFade = 1.0f;

        m_FadeCoroutine = FadeCoroutine(seconds, targetFade);
        StartCoroutine(m_FadeCoroutine);
    }

    /// <summary>
    /// Coroutine to fade the fade image
    /// </summary>
    /// <param name="seconds"> secods of fade </param>
    /// <param name="targetFade"> desired alpha </param>
    /// <returns></returns>
    private IEnumerator FadeCoroutine(float seconds, float targetFade)
    {
        if (m_FadeImage)
        {
            float timer = 0;
            float srcFade = m_FadeAlpha;
            Color oldColor = m_FadeImage.color;

            if (seconds < 0.1f)
                seconds = 0.1f;

            while (timer < seconds)
            {
                timer += Time.deltaTime;
                
                m_FadeAlpha = Mathf.Lerp(srcFade, targetFade, timer / seconds);
                oldColor.a = m_FadeAlpha;
                m_FadeImage.color = oldColor;
                
                yield return null;
            }

            oldColor.a = m_FadeAlpha = targetFade;
            m_FadeImage.color = oldColor;
        }

        yield break;
    }

    /// <summary>
    /// Display current player stats 
    /// </summary>
    public void UpdateStats()
    {
        if (PlayerStats.Instance != null)
        {
            if (m_HPPct)
                m_HPPct.text = ((int)PlayerStats.Instance.HP).ToString();
            if (m_STPct)
                m_STPct.text = ((int)PlayerStats.Instance.ST).ToString();
            if (m_AmmoCount)
                m_AmmoCount.text = ((int)PlayerStats.Instance.AmmoCount).ToString();
        }
    }

    /// <summary>
    /// Set the object interaction text and invoke hide
    /// </summary>
    /// <param name="str"></param>
    public void SetObjectInteractionText(string str)
    {
        if (m_ObjectInteractionText)
        {
            if (str != null)
                m_ObjectInteractionText.text = str;

            m_ObjectInteractionText.gameObject.SetActive(true);
        }

        Invoke("HideObjectInteractionText", m_TextDisplayTime);
    }

    /// <summary>
    /// Hide the object interaction text
    /// </summary>
    public void HideObjectInteractionText()
    {
        if (m_ObjectInteractionText != null)
            m_ObjectInteractionText.gameObject.SetActive(false);
    }


    /// <summary>
    /// Display objective text and invoke hide
    /// </summary>
    /// <param name="str"></param>
    public void ShowObjectiveText(string str)
    {
        if (m_ObjectiveText != null)
        {
            m_ObjectiveText.gameObject.SetActive(true);

            if (str != null)
                m_ObjectiveText.text = str;
        }

        Invoke("HideObjectiveText", m_TextDisplayTime);
    }


    /// <summary>
    /// Hide objective text
    /// </summary>
    public void HideObjectiveText()
    {
        if (m_ObjectiveText != null)
            m_ObjectiveText.gameObject.SetActive(false);
    }
}