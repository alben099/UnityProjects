using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Use game object pools to make a bunch of objects ahead of time
/// to be used and recycled when necessary.
/// </summary>
public class ObjectPools : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string name;
        public GameObject prefab;
        public int poolSize;
    }

    public List<Pool> objectPools;
    public Dictionary<string, List<GameObject>> poolDictionary;

    /// <summary>
    /// Start by making lists of game objects based on the Pool class.
    /// The poolDictionary holds the key/name of the pool and makes the
    /// given number of objects based on the poolSize.
    /// </summary>
    private void Start()
    {
        poolDictionary = new Dictionary<string, List<GameObject>>();
        foreach (Pool pool in objectPools)
        {
            List<GameObject> prefabPool = new List<GameObject>();

            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject prefab = Instantiate(pool.prefab);
                prefab.SetActive(false);
                prefabPool.Add(prefab);
            }

            poolDictionary.Add(pool.name, prefabPool);
        }
    }

    /// <summary>
    /// Activate a non-active game object from the poolDictionary using a key/name of
    /// the pool. The object will be at a given position.
    /// </summary>
    /// <param name="name">The key/name of the pool to get an object in it.</param>
    /// <param name="position">The Vector3 position to put the object.</param>
    /// <returns>The activated game object.</returns>
    public GameObject SpawnFromPool(string name, Vector3 position)
    {
        if (!poolDictionary.ContainsKey(name))
        {
            Debug.LogWarning($"Object pool with key called \"{name}\" does not exist!");
            return null;
        }

        foreach (GameObject obj in poolDictionary[name])
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                obj.transform.position = position;
                return obj;
            }
        }

        return null;
    }

    /// <summary>
    /// Activate a non-active game object from the poolDictionary using a key/name of
    /// the pool. The object will be at a given position and rotation.
    /// </summary>
    /// <param name="name">The key/name of the pool to get an object in it.</param>
    /// <param name="position">The Vector3 position to put the object.</param>
    /// <param name="rotation">The Quaternion rotation to set the object</param>
    /// <returns>The activated game object.</returns>
    public GameObject SpawnFromPool(string name, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(name))
        {
            Debug.LogWarning($"Object pool with key called \"{name}\" does not exist!");
            return null;
        }

        foreach (GameObject obj in poolDictionary[name])
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                return obj;
            }
        }

        return null;
    }
}
