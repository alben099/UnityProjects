using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The manager that lets the user go to different scenes or quit the game.
/// </summary>
public class CommandManager : MonoBehaviour
{
    private static CommandManager instance;

    public static CommandManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("The CommandManager is NULL.");
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Go to the next scene when the associated UI element is clicked on.
    /// </summary>
    public void NextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex < 3)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    /// <summary>
    /// Close the application when the associated UI element is clicked on.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Game exited");
        Application.Quit();
    }

    /// <summary>
    /// Go to the title scene when the associated UI element is clicked on.
    /// </summary>
    public void ReturnToTitle()
    {
        SceneManager.LoadScene(0);
    }
}
