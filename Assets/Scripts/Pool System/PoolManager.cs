using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] Pool[] enemyPools;
    [SerializeField] Pool[] playerProjectilesPool;
    [SerializeField] Pool[] enemyProjectilesPool;
    [SerializeField] Pool[] vfxPools;
    [SerializeField] Pool[] lootItemPools;
    [SerializeField] Pool[] bulletHellSpawner;


    static Dictionary<GameObject, Pool> dictionary;


    private void Awake() 
    {
        dictionary = new Dictionary<GameObject, Pool>();
        Initialize(enemyPools);
        Initialize(playerProjectilesPool);
        Initialize(enemyProjectilesPool);
        Initialize(vfxPools);
        Initialize(lootItemPools);
        Initialize(bulletHellSpawner);


    }

    private void OnDestroy()
    {
        CheckPoolSize(enemyPools);
        CheckPoolSize(playerProjectilesPool);
        CheckPoolSize(enemyProjectilesPool);
        CheckPoolSize(vfxPools);
        CheckPoolSize(lootItemPools);
        CheckPoolSize(bulletHellSpawner);

    }

    void CheckPoolSize(Pool[] pools)
    {
        foreach(var pool in pools)
        if(pool.RuntimeSize > pool.Size)
        {
            Debug.LogWarning(string.Format("Pool: {0} has a runtime size {1} bigger than its initial size{2}" ,
            pool.Prefab.name,
            pool.RuntimeSize,
            pool.Size));
        }
    }
    void Initialize(Pool[] pools)
    {
        foreach(var pool in pools)
        {
        #if UNITY_EDITOR
            if(dictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("Same Prefab in multiple pools! prefab:" + pool.Prefab.name);
                continue;
            }
        #endif

            dictionary.Add(pool.Prefab, pool);
            Transform poolParent =  new GameObject("Pool : " + pool.Prefab.name).transform;

            poolParent.parent = transform;
            pool.Initialize(poolParent);
        }
    }

    public static GameObject Release(GameObject prefab)
    {
        #if UNITY_EDITOR
        if(!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager Could not Find Prefab : " + prefab.name);
            return null;
        }
        #endif
        return dictionary[prefab].PreparedObject();
    }

    public static GameObject Release(GameObject prefab, Vector3 position)
    {
        #if UNITY_EDITOR
        if(!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager Could not Find Prefab : " + prefab.name);
            return null;
        }
        #endif
        return dictionary[prefab].PreparedObject(position);
    }

    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        #if UNITY_EDITOR
        if(!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager Could not Find Prefab : " + prefab.name);
            return null;
        }
        #endif
        return dictionary[prefab].PreparedObject(position, rotation);
    }

    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        #if UNITY_EDITOR
        if(!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager Could not Find Prefab : " + prefab.name);
            return null;
        }
        #endif
        return dictionary[prefab].PreparedObject(position, rotation, localScale);
    }
}
