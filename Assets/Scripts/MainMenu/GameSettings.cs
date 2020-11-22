using UnityEngine;
using UnityEngine.Audio;
public class GameSettings : Singleton<GameSettings>
{
    [Header("SpawnTimes")]
    public float m_ObeliskSpawnTime;
    public float m_SupplySpawnTime;

    [Header("Enemies")]
    public int m_NbVampireBats;

    #region Getters/Setters

    public float ObeliskSpawnTime
    {
        get => Instance.m_ObeliskSpawnTime;
        set => Instance.m_ObeliskSpawnTime = value;
    }

    public float SupplySpawnTime
    {
        get => Instance.m_SupplySpawnTime;
        set => Instance.m_SupplySpawnTime = value;
    }

    public float NbVampireBats
    {
        get => Instance.m_NbVampireBats;
        set => Instance.m_NbVampireBats = (int)value;
    }

    #endregion
}
