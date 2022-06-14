using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles the game's time, score, and lives. Also, it handles
/// collectible spawning.
/// </summary>
[RequireComponent (typeof (ObjectPools))]
public class GameManager : MonoBehaviour
{
    public enum EndGameMode { Win, Lose }

    public delegate void OnScoreChanged();
    public OnScoreChanged onScoreChangedCallback;

    [Header("Collectible Object Variables")]
    public string[] largeObjectPoolNames;
    public string[] mediumObjectPoolNames;
    public string[] smallObjectPoolNames;
    public float timeToStartSpawning = 0.25f;
    public float timeToSpawnCollectible = 0.75f;
    [Range(10, 50)] public int collectibleLimit = 30;

    [Header("Size Spawn Rates")]
    [Range(0.1f, 0.9f)] public float mediumSmallAdj = 0.5f;
    [Range(1.1f, 1.9f)] public float largeMediumAdj = 1.5f;

    [Header("Collect Drop (CD) and Storage Drop (SD) Points Variables")]
    public Transform collectibleDropPoint;
    [Range(-10, 0)] public float leftCDAdjust = -1;
    [Range(0, 10)] public float rightCDAdjust = 1;
    [Range(-10, 0)] public float backCDAdjust = -2;
    [Range(0, 10)] public float forwardCDAdjust = 2;
    public Transform storageDropPoint;
    [Range(-10, 0)] public float backSDAdjust = -2;
    [Range(0, 10)] public float forwardSDAdjust = 2;

    [Header("Game Variables")]
    [Range(5.5f, 120.5f)] public float startTime = 60.5f;
    public int startLives = 3;

    private ObjectPools collectiblePools;
    private float countdownTime;
    private int lives;
    private int score;
    private int collectiblesInCollectDropZone;

    public float CountdownTime
    {
        get { return countdownTime; }
    }

    public int Lives
    {
        get { return lives; }
    }

    public int Score
    {
        get { return score; }
    }

    #region Singleton
    private static GameManager instance;

    /// <summary>
    /// Create a singleton of the game manager
    /// </summary>
    public static GameManager Instance
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
    /// When the game starts, set this game object as a singleton
    /// </summary>
    private void Awake()
    {
        instance = this;
    }
    #endregion

    /// <summary>
    /// Initialized game variables and set the delegate while repeating
    /// the method SpawnCollectible().
    /// </summary>
    private void Start()
    {
        Time.timeScale = 1;
        countdownTime = startTime;
        lives = startLives;
        score = 0;
        collectiblesInCollectDropZone = 0;
        collectiblePools = this.GetComponent<ObjectPools>();
        UIManager.Instance.InitiateAllUI();
        onScoreChangedCallback += UIManager.Instance.UpdateScore;
        onScoreChangedCallback += EnemyManager.Instance.AddEnemySpeed;

        InvokeRepeating("SpawnCollectible", timeToStartSpawning, timeToSpawnCollectible);
    }

    /// <summary>
    /// Used as a countdown. If it reaches zero, the game ends as a win.
    /// </summary>
    private void Update()
    {
        if (countdownTime <= 0.5f)
        {
            EndGame(EndGameMode.Win);
        }

        countdownTime -= Time.deltaTime;
        UIManager.Instance.UpdateTime();
    }

    public void AddBackTime(float addedTime)
    {
        countdownTime += addedTime;
    }

    /// <summary>
    /// If there are no more lives, then the game ends as a lose.
    /// </summary>
    public void LoseLife()
    {
        lives--;
        if (lives <= 0)
        {
            EndGame(EndGameMode.Lose);
        }
    }

    public int GetScore() 
    {
        return score;
    }

    /// <summary>
    /// The delegate calls methods from other scripts whenever the score changes.
    /// </summary>
    /// <param name="pointsAdded">This is the number of points added to the score.</param>
    public void AddToScore(int pointsAdded)
    {
        score += pointsAdded;
        if (onScoreChangedCallback != null)
            onScoreChangedCallback.Invoke();
    }

    /// <summary>
    /// Drops the collectible in either the storage or collectable zones. Their positions are adjusted based
    /// on the adjust variables.
    /// </summary>
    /// <param name="inStorage">Boolean to tell if this collectible will go in storage or not.</param>
    /// <returns>The drop position.</returns>
    public Vector3 DropCollectibleRandomly(bool inStorage)
    {
        Vector3 dropPos = Vector3.zero;

        if (!inStorage)
        {
            dropPos.x = Random.Range((collectibleDropPoint.position.x + leftCDAdjust), (collectibleDropPoint.position.x + rightCDAdjust));
            dropPos.y = collectibleDropPoint.position.y;
            dropPos.z = Random.Range((collectibleDropPoint.position.z + backCDAdjust), (collectibleDropPoint.position.z + forwardCDAdjust));
        }
        else if (inStorage)
        {
            dropPos.x = storageDropPoint.position.x;
            dropPos.y = storageDropPoint.position.y;
            dropPos.z = Random.Range((storageDropPoint.position.z + backSDAdjust), (storageDropPoint.position.z + forwardSDAdjust));
        }

        return dropPos;
    }

    /// <summary>
    /// If the collectible limit has not been reached, then a collectible will spawn and
    /// will account for its size and drop position.
    /// </summary>
    private void SpawnCollectible()
    {
        if (collectiblesInCollectDropZone < collectibleLimit)
        {
            collectiblesInCollectDropZone++;
            float sizePicker = Random.Range(0.0f, 2.0f);
            if (sizePicker >= 0.0f && sizePicker <= mediumSmallAdj)
            {
                int smallPicker = Random.Range(0, smallObjectPoolNames.Length);
                collectiblePools.SpawnFromPool(smallObjectPoolNames[smallPicker], DropCollectibleRandomly(false), Random.rotation);
            }
            else if (sizePicker > mediumSmallAdj && sizePicker <= largeMediumAdj)
            {
                int mediumPicker = Random.Range(0, mediumObjectPoolNames.Length);
                collectiblePools.SpawnFromPool(mediumObjectPoolNames[mediumPicker], DropCollectibleRandomly(false), Random.rotation);
            }
            else if (sizePicker > largeMediumAdj && sizePicker < 2.0f)
            {
                int largePicker = Random.Range(0, largeObjectPoolNames.Length);
                collectiblePools.SpawnFromPool(largeObjectPoolNames[largePicker], DropCollectibleRandomly(false), Random.rotation);
            }
        }
    }

    /// <summary>
    /// This is used every time a collectible was collected from
    /// the collectible zone. It indicates how many of them are in
    /// that zone so that a limited number of collectibles are on
    /// screen.
    /// </summary>
    public void SubtractCollectiblesInCollectDropZone()
    {
        collectiblesInCollectDropZone--;
    }

    /// <summary>
    /// The game will end by setting the time scale to 0 and
    /// showing the UI that the game is over. The game over text
    /// will changed depending if it was a win or loss.
    /// Also, it records a high score if it beated the
    /// previous high score.
    /// </summary>
    /// <param name="mode">Win or loss.</param>
    public void EndGame(EndGameMode mode)
    {
        if (score > PlayerPrefs.GetInt("High Score", 0))
        {
            PlayerPrefs.SetInt("High Score", score);
        }

        string endGameMessage = "";
        switch (mode)
        {
            case EndGameMode.Win:
                endGameMessage = "Time's Up!";
                break;
            case EndGameMode.Lose:
                endGameMessage = "Storage Busted!";
                break;
        }
        ClickController.Instance.SetCanClick(false);
        UIManager.Instance.EndGameEnabled(endGameMessage);
    }
}
