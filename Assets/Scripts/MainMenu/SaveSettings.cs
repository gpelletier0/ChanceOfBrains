using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSettings : MonoBehaviour
{

    public GameObject m_ObeliskSlider;
    public GameObject m_SupplyDropSlider;
    public GameObject m_VampireBatSlider;

    private void Awake()
    {
        GameSettings.Instance.ObeliskSpawnTime = m_ObeliskSlider.GetComponent<Slider>().value;
        GameSettings.Instance.SupplySpawnTime = m_SupplyDropSlider.GetComponent<Slider>().value;
        GameSettings.Instance.NbVampireBats = m_VampireBatSlider.GetComponent<Slider>().value;
    }

    void Start()
    {
        GameSettings.Instance.ObeliskSpawnTime = m_ObeliskSlider.GetComponent<Slider>().value;
        GameSettings.Instance.SupplySpawnTime = m_SupplyDropSlider.GetComponent<Slider>().value;
        GameSettings.Instance.NbVampireBats = m_VampireBatSlider.GetComponent<Slider>().value;
    }
}
