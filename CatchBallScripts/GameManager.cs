using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// The Game Manager that handles UI and system functions
/// </summary>
public class GameManager : MonoBehaviour
{
    // Player variable
    [Tooltip("Put the player object here when other scripts need it")]
    public GameObject player;

    // Text variables
    [Tooltip("Text displaying how much HP they have left")]
    public TextMeshProUGUI healthText;
    [Tooltip("Text that will display after the win condition is met")]
    public TextMeshProUGUI winText;
    [Tooltip("Text that will display after the player loses all HP")]
    public TextMeshProUGUI gameOverText;
    [Tooltip("Text instructing the player how to continue the game")]
    public TextMeshProUGUI continueText;

    // Private variables, including the singleton
    private static GameManager instance;
    private bool isWin = false;
    private bool isDone = false;

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
    /// Creates the instance of this object as a singleton
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Check at the end of every frame that all enemies are gone
    /// </summary>
    private void LateUpdate()
    {
        GameManager.Instance.CheckAllEnemies();
        if (!isDone)
        {
            Time.timeScale = 1;
        }
    }

    /// <summary>
    /// Gets the player game object
    /// </summary>
    /// <returns>The player game object</returns>
    public GameObject GetPlayer()
    {
        return player;
    }

    /// <summary>
    /// Activates the appropriate UI for when the player wins or not
    /// </summary>
    /// <param name="winCondition">The Boolean for when the player wins (true) or not (false)</param>
    public void WonGame(bool winCondition)
    {
        if (winCondition == true)
        {
            winText.gameObject.SetActive(true);
            continueText.gameObject.SetActive(true);
            isWin = true;
        }
        else
        {
            gameOverText.gameObject.SetActive(true);
            continueText.gameObject.SetActive(true);
            isWin = false;
        }

        isDone = true;
        Time.timeScale = 0;
    }

    /// <summary>
    /// Updates the UI for health for the player
    /// </summary>
    /// <param name="health">Number of hit points the player has</param>
    public void UpdateHealth(int health)
    {
        healthText.SetText("Health: " + health);
    }

    /// <summary>
    /// Check how many enemies are on the scene, end the game when none are there
    /// </summary>
    public void CheckAllEnemies()
    {
        int size = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (size == 0)
        {
            WonGame(true);
        }
    }

    /// <summary>
    /// Move to a different scene if the player wins, restart scene if player loses
    /// </summary>
    public void ContinueScenes()
    {
        if (isDone)
        {
            Time.timeScale = 1;
            if (isWin)
            {
                if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    SceneManager.LoadScene(0);
                }
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    /// <summary>
    /// Function for the start button to move to the first level
    /// </summary>
    public void StartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Function for the exit button to quit the application
    /// </summary>
    public void ExitButton()
    {
        Debug.Log("Game exited");
        Application.Quit();
    }
}
