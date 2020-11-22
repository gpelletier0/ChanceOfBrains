using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSettings : MonoBehaviour
{
    public GameObject m_FullScreenToggle;
    public GameObject m_VolumeSlider;
    public GameObject m_ObeliskSlider;
    public GameObject m_SupplyDropSlider;
    public GameObject m_VampireBatSlider;

    // Start is called before the first frame update
    void Start()
    {
        GameSettings.Instance.SetFullScreen = m_FullScreenToggle.GetComponent<Toggle>().isOn;
        GameSettings.Instance.SetVolume = m_VolumeSlider.GetComponent<Slider>().value;
        GameSettings.Instance.SetObeliskSpawnTime = m_ObeliskSlider.GetComponent<Slider>().value;
        GameSettings.Instance.SetSupplySpawnTime = m_SupplyDropSlider.GetComponent<Slider>().value;
        GameSettings.Instance.NbVampireBats = m_VampireBatSlider.GetComponent<Slider>().value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
