using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy Manager that handles enemy spawning and adding their speeds
/// according to the player's current score.
/// </summary>
[RequireComponent(typeof(ObjectPools))]
public class EnemyManager : MonoBehaviour
{
    [Header("Object Pool Variables")]
    public string[] enemyPoolNames;
    [Header("Spawn Variables")]
    public Transform enemySpawnPoint;
    public float initialSpawnTime = 1.5f;
    public float timeToSpawnEnemy = 5.0f;
    [Header("Adjustment Variables")]
    [Tooltip("If this is low, then the rate for enemies to move faster decreases and vice versa.")] 
    public float speedAdjDenominator = 10.0f;
    public float xSpawnAdjustment = 3.0f;
    public float zSpawnAdjustment = 3.0f;

    private ObjectPools enemyPools;
    private float speedAdded;

    #region Singleton
    private static EnemyManager instance;

    /// <summary>
    /// Create a singleton of the enemy manager
    /// </summary>
    public static EnemyManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("The EnemyManager is NULL.");
            }

            return instance;
        }
    }

    /// <summary>
    /// When the game starts, set this game object as a singleton
    /// </summary>
    private void Awake()
    {
        instance = this;
    }
    #endregion

    /// <summary>
    /// Start on frame one to get this manager's enemy object pools and
    /// repeat the method SpawnEnemy().
    /// </summary>
    private void Start()
    {
        enemyPools = this.GetComponent<ObjectPools>();
        speedAdded = 0.0f;

        InvokeRepeating("SpawnEnemy", initialSpawnTime, timeToSpawnEnemy);
    }

    /// <summary>
    /// Spawn an enemy from one of the object pools. Their position varies
    /// based on the xSpawnAdjustment and zSpawnAdjustment.
    /// </summary>
    private void SpawnEnemy()
    {
        int enemyPoolIndex = Random.Range(0, enemyPoolNames.Length);
        Vector3 adjustedSpawnPoint = new Vector3(enemySpawnPoint.position.x + Random.Range(-xSpawnAdjustment, xSpawnAdjustment + 1), 
            enemySpawnPoint.position.y, enemySpawnPoint.position.z + Random.Range(-zSpawnAdjustment, zSpawnAdjustment + 1));

        GameObject activatedEnemy = enemyPools.SpawnFromPool(enemyPoolNames[enemyPoolIndex], adjustedSpawnPoint);
        activatedEnemy.GetComponent<Enemy>().SetSpeed(speedAdded);
    }

    /// <summary>
    /// Spawn enemy from beginning position,
    /// deactivate it from the scene after it is clicked or hits target,
    /// then grab it and activate it again while setting it to the beginning position.
    /// </summary>
    public void AddEnemySpeed()
    {
        float currentScore = (float) GameManager.Instance.GetScore();
        speedAdded = currentScore / speedAdjDenominator;
    }
}
