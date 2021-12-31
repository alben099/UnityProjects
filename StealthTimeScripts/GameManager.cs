using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// The script to handle GUI and scene functions.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Tooltip("Text that will appear when the player is spotted by an enemy")]
    public TextMeshProUGUI gameOverText;
    [Tooltip("Text that will appear when the player completes the objective")]
    public TextMeshProUGUI winText;
    [Tooltip("Text that appears when after a win or a game over")]
    public TextMeshProUGUI pressEnterText;

    private static GameManager instance;
    private bool isGameOver = false;
    private bool isNextLevel = false;

    /// <summary>
    /// Create a singleton of the game manager.
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
    /// Load the object that has this script.
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Announce that it's game over and stop the game.
    /// </summary>
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        pressEnterText.gameObject.SetActive(true);
        isGameOver = true;
        Time.timeScale = 0;
    }

    /// <summary>
    /// Returns if the game is over or not.
    /// </summary>
    /// <returns>The game over boolean variable.</returns>
    public bool GetIsGameOver()
    {
        return isGameOver;
    }

    /// <summary>
    /// Returns if the player can go to the next level.
    /// </summary>
    /// <returns>The next level boolean variable.</returns>
    public bool GetIsNextLevel()
    {
        return isNextLevel;
    }

    /// <summary>
    /// Announce that the player beated the level and moves on to the next level.
    /// </summary>
    public void NextLevel()
    {
        winText.gameObject.SetActive(true);
        pressEnterText.gameObject.SetActive(true);
        isNextLevel = true;
        Time.timeScale = 0;
    }

    /// <summary>
    /// Checks if it's game over or can go to the next level.
    /// </summary>
    public void Scenes()
    {
        if (isGameOver)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (isNextLevel)
        {
            Time.timeScale = 1;
            if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            Debug.Log("Scene not over");
        }
    }
}
