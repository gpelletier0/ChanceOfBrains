using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private const int DEFAULT_POOL_SIZE = 10;
    public static bool m_Expand = true;
    public static Dictionary<string, List<GameObject>> m_ObjectPools = new Dictionary<string, List<GameObject>>();

    private static bool ExistsForPrefab(string path)
    {
        return m_ObjectPools.ContainsKey(path);
    }

    private static bool isAvailable(GameObject obj)
    {
        return !obj.activeSelf;
    }

    private static GameObject GetObject(string path, List<GameObject> pool)
    {
        if (!m_Expand) 
            return null;

        GameObject prefab = Resources.Load<GameObject>(path);
        GameObject instance = Object.Instantiate(prefab) as GameObject;

        pool.Add(instance);

        return instance;
    }

    public static List<GameObject> CreatePool(string path, int num)
    {
        if (num <= 0) 
            num = 1;

        GameObject prefab = Resources.Load<GameObject>(path);
        List<GameObject> ObjectList = new List<GameObject>();

        for(int i = 0; i < num; i++)
        {
            GameObject instance = Instantiate<GameObject>(prefab);
            ObjectList.Add(instance);

            instance.SetActive(false);
        }

        m_ObjectPools.Add(path, ObjectList);

        return ObjectList;
    }

    public static GameObject GetObject(string path, int size = DEFAULT_POOL_SIZE)
    {
        if (!ExistsForPrefab(path))
        {
            return Create(path, size);
        }

        List<GameObject> pool = m_ObjectPools[path];
        GameObject instance = FindFirst(pool);
        
        if(instance == null)
            Create(path, size);

        return instance;
    }

    private static GameObject Create(string path, int size = DEFAULT_POOL_SIZE)
    {
        CreatePool(path, size);
        return GetObject(path);
    }

    private static GameObject FindFirst(List<GameObject> pool)
    {
        foreach(GameObject go in pool)
        {
            if (isAvailable(go))
                return go;
        }

        return null;
    }


}
