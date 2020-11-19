using System.Collections.Generic;
using UnityEngine;

public static class ObjectPooler
{
    #region Attributes

    private const int DEFAULT_SIZE = 10;
    private const bool m_bExpand = true;
    private static readonly Dictionary<string, List<GameObject>> m_ObjectPools = new Dictionary<string, List<GameObject>>();


    private static bool Exists(string path)
    {
        return m_ObjectPools.ContainsKey(path);
    }

    private static bool IsAvailable(GameObject go)
    {
        return !go.activeSelf;
    }

    public static GameObject GetPooledObject(string path, int size = DEFAULT_SIZE)
    {
        if (Exists(path))
            return FindFirst(m_ObjectPools[path]) ?? Expand(path, m_ObjectPools[path]);
        
        return Create(path, size);
    }

    private static GameObject Create(string path, int poolSize = DEFAULT_SIZE)
    {
        CreateObjectPool(path, poolSize);

        return GetPooledObject(path);
    }

    private static GameObject FindFirst(List<GameObject> pool)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (IsAvailable(pool[i]))
                return pool[i];
        }

        return null;
    }

    private static GameObject Expand(string path, List<GameObject> pool)
    {
        if (m_bExpand)
        {
            GameObject prefab = Resources.Load<GameObject>(path);

            GameObject instance = Object.Instantiate(prefab) as GameObject;
            pool.Add(instance);
            return instance;
        }
    }

    public static List<GameObject> CreateObjectPool(string path, int nb)
    {
        if (nb <= 0)
            nb = 1;

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