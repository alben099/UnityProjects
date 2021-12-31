using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script that creates pools of objects for reusability and performance enhancement
/// </summary>
public class ObjectPools : MonoBehaviour
{
    /// <summary>
    /// The pool class that has a number of objects organized with a tag
    /// </summary>
    [System.Serializable]
    public class Pool
    {
        [Tooltip("The name of the pool that will be used exactly as it is by other scripts")]
        public string tag;
        [Tooltip("The object of the pool")]
        public GameObject prefab;
        [Tooltip("The number of the prefabs in the pool")]
        public int size;
    }

    #region Singleton
    public static ObjectPools instance;

    public static ObjectPools Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("The GameManager is NULL.");
            }

            return instance;
        }
    }

    /// <summary>
    /// Creates the instance of this object as a singleton
    /// </summary>
    private void Awake()
    {
        instance = this;
    }
    #endregion

    // Public variables for getting object pools in a dictionary
    public List<Pool> pools;
    public Dictionary<string, List<GameObject>> poolDictionary;

    /// <summary>
    /// Start at frame one to make a dictionary of object pools that have
    /// instantiated a specified number of objects
    /// </summary>
    void Start()
    {
        poolDictionary = new Dictionary<string, List<GameObject>>();
        foreach (Pool pool in pools)
        {
            List<GameObject> objectPool = new List<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Add(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    /// <summary>
    /// Grabs an inactive object from a specified pool
    /// </summary>
    /// <param name="tag">The pool's name</param>
    /// <returns>An activated object from the pool</returns>
    public GameObject SpawnFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        foreach (GameObject obj in poolDictionary[tag])
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        return null;
    }
}
