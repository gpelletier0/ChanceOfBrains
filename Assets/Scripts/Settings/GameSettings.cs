using UnityEngine;
using UnityEngine.Audio;
public class GameSettings : Singleton<GameSettings>
{
    [Header("Audio")]
    public AudioMixer m_AudioMixer;

    [Header("SpawnTimes")]
    public float m_ObeliskSpawnTime;
    public float m_SupplySpawnTime;

    [Header("Enemies")]
    public int m_NbVampireBats;

    #region Getters/Setters
    
    public float SetVolume
    {
        set => m_AudioMixer.SetFloat("Volume", value);
    }

    public bool SetFullScreen
    {
        set => Screen.fullScreen = value;
    }

    public float SetObeliskSpawnTime
    {
        get => m_ObeliskSpawnTime;
        set => m_ObeliskSpawnTime = value;
    }

    public float SetSupplySpawnTime
    {
        get => m_SupplySpawnTime;
        set => m_SupplySpawnTime = value;
    }

    public float NbVampireBats
    {
        get => m_NbVampireBats;
        set => m_NbVampireBats = (int)value;
    }

    #endregion
}
