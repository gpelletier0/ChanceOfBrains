using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class to precache and pool game objects
/// Mostly Proof of Concept but can be scaled
/// </summary>
public static class ObjectPooler
{
    private const int DEFAULT_SIZE = 5; // Default precach pool size
    private const bool m_bExpand = true; // Can you expand the list
    private static Dictionary<string, List<GameObject>> m_ObjectPools; // the pool list

    /// <summary>
    /// Initialize the Dictionary to a new Dictionary
    /// </summary>
    public static void Initialize()
    {
        m_ObjectPools = new Dictionary<string, List<GameObject>>();
    }

    /// <summary>
    /// if the path is contained in the dictionary as key
    /// </summary>
    /// <param name="path"> Dictionary key </param>
    /// <returns></returns>
    private static bool Exists(string path)
    {
        return m_ObjectPools.ContainsKey(path);
    }


    /// <summary>
    /// If the object is in active state or not
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    private static bool IsAvailable(GameObject go)
    {
        return !go.activeSelf;
    }

    /// <summary>
    /// Returns the first instance of a pooled game object or creates more
    /// </summary>
    /// <param name="path"> path of prefab </param>
    /// <param name="size"> size of new pool </param>
    /// <returns></returns>
    public static GameObject GetPooledObject(string path, int size = DEFAULT_SIZE)
    {
        if (Exists(path))
            return FindFirst(m_ObjectPools[path]) ?? Expand(path, m_ObjectPools[path]);
        
        return Create(path, size);
    }

    /// <summary>
    /// Creates a new pool of game objects
    /// </summary>
    /// <param name="path"> prefab path </param>
    /// <param name="poolSize"> size of pool to create </param>
    /// <returns></returns>
    private static GameObject Create(string path, int poolSize = DEFAULT_SIZE)
    {
        CreateObjectPool(path, poolSize);

        return GetPooledObject(path);
    }

    /// <summary>
    /// Returns first game object that is disabled
    /// </summary>
    /// <param name="pool"> list of game objects </param>
    /// <returns></returns>
    private static GameObject FindFirst(List<GameObject> pool)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (IsAvailable(pool[i]))
                return pool[i];
        }

        return null;
    }

    /// <summary>
    /// Expands the game object list
    /// </summary>
    /// <param name="path"> path of prefab </param>
    /// <param name="pool"> pool list </param>
    /// <returns></returns>
    private static GameObject Expand(string path, List<GameObject> pool)
    {
        if (m_bExpand)
        {
            GameObject prefab = Resources.Load<GameObject>(path);

            GameObject instance = Object.Instantiate(prefab);
            pool.Add(instance);
            return instance;
        }
    }

    /// <summary>
    /// Creates a new game object pool list
    /// </summary>
    /// <param name="path"> prefab path </param>
    /// <param name="nb"> number new objects to create, default 1 </param>
    /// <returns></returns>
    public static List<GameObject> CreateObjectPool(string path, int nb = 1)
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        List<GameObject> objects = new List<GameObject>();

        for (int i = 0; i < nb; i++)
        {
            GameObject instance = Object.Instantiate<GameObject>(prefab);
            objects.Add(instance);
            instance.SetActive(false);
        }

        m_ObjectPools.Add(path, objects);

        return objects;
    }
}