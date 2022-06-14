using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages UI elements for the game scene.
/// </summary>
public class UIManager : MonoBehaviour
{
    public Text timerText;
    public Text livesText;
    public Text scoreText;

    [Tooltip("Parent object that has UI elements to take the player back to the title screen.")]
    public GameObject gameEndedUI;
    public Text gameEndedText;

    #region Singleton
    private static UIManager instance;

    /// <summary>
    /// Create a singleton of the game manager
    /// </summary>
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("The UIManager is NULL.");
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

    private void Start()
    {
        gameEndedUI.SetActive(false);
    }

    public void UpdateTime()
    {
        timerText.text = $"Timer: {Mathf.Floor(GameManager.Instance.CountdownTime)}";
    }

    public void UpdateLives()
    {
        livesText.text = $"Lives: {GameManager.Instance.Lives}";
    }

    public void UpdateScore()
    {
        scoreText.text = $"Score: {GameManager.Instance.Score}";
    }

    public void InitiateAllUI()
    {
        UpdateTime();
        UpdateLives();
        UpdateScore();
    }

    /// <summary>
    /// Show the End Game UI object that has an appropriate text message
    /// and a button to take the player back to the title screen.
    /// </summary>
    /// <param name="endGameMessage">Text that tells what happened after the game ended.</param>
    public void EndGameEnabled(string endGameMessage)
    {
        gameEndedText.text = endGameMessage;
        gameEndedUI.SetActive(true);
        Time.timeScale = 0;
    }

    /// <summary>
    /// Set the time scale back to 1 and return to the title screen.
    /// </summary>
    public void ReturnToTitle()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
