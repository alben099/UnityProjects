using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manage the title scene to make sure the time scale is set to 1
/// for the game scene.
/// </summary>
public class TitleManager : MonoBehaviour
{
    public Text highScoreText;

    private void Start()
    {
        Time.timeScale = 1;
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("High Score", 0).ToString();
    }

    public void PlayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ResetScore()
    {
        PlayerPrefs.DeleteKey("High Score");
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("High Score", 0).ToString();
    }
}
