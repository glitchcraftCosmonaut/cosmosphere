using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;
    private List<GameObject> pooledObjects;
    public List<ObjectPoolItem> itemsToPool;
    // public GameObject objectToPool;
    // public int amountToPool;

    // public bool shouldExpand = true;

    private void Awake()
    {
        SharedInstance = this;    
    }
    // Start is called before the first frame update
    void Start()
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool) 
        {
            for (int i = 0; i < item.amountToPool; i++) 
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObjects.Count; i++) 
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag) 
            {
                return pooledObjects[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool) 
        {
            if (item.objectToPool.tag == tag) 
            {
                if (item.shouldExpand) 
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }
}

[System.Serializable]
public class ObjectPoolItem 
{
    public int amountToPool;
    public GameObject objectToPool;
    public bool shouldExpand;
}
