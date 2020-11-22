using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    public void HideObjectInteractionText()
    {
        if (m_ObjectInteractionText != null)
            m_ObjectInteractionText.gameObject.SetActive(false);
    }

    public void ShowObjectiveText(string str, Color color)
    {
        if (m_ObjectiveText != null)
        {
            m_ObjectiveText.gameObject.SetActive(true);
            m_ObjectInteractionText.color = color;

            if (str != null)
                m_ObjectiveText.text = str;
        }

        Invoke("HideObjectiveText", m_TextDisplayTime);
    }

    public void HideObjectiveText()
    {
        if (m_ObjectiveText != null)
            m_ObjectiveText.gameObject.SetActive(false);
    }
}