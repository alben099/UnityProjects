using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the Player script for controlling the Player object.
/// It can move the player and allow them to slow down time.
/// </summary>
public class Player : MonoBehaviour
{
    [Tooltip("How fast the player can move by default")]
    [SerializeField] float speed = 3;
    [Tooltip("How many seconds time slows down")]
    [SerializeField] float timeSlowed = 3;
    [Tooltip("How many seconds before the player can slow down time again")]
    [SerializeField] float coolDown = 3;

    private float horizontalInput;
    private float verticalInput;
    private bool slowingDown = false;

    /// <summary>
    /// Check every frame to see if it the player can stop time or
    /// they can go the next scene / restart their current scene.
    /// </summary>
    void Update()
    {
        if (!GameManager.Instance.GetIsGameOver() && !GameManager.Instance.GetIsNextLevel())
        {
            if (Input.GetKeyDown(KeyCode.Space) && !slowingDown)
            {
                Time.timeScale = 0.5f;
                slowingDown = true;
                GetComponent<MeshRenderer>().material.color = Color.blue;
                StartCoroutine(TimeSlowed(timeSlowed));
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameManager.Instance.Scenes();
        }
    }

    /// <summary>
    /// Updates the player's movement when time is slowed down or not.
    /// </summary>
    void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Time.timeScale != 0)
        {
            transform.Translate((horizontalInput * speed * (Time.deltaTime / Time.timeScale)), 0.0f, (verticalInput * speed * (Time.deltaTime / Time.timeScale)));
        } 
    }

    /// <summary>
    /// Check if the player has collided with the object to go to the next level.
    /// </summary>
    /// <param name="other">The other object that collided with the player.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            Destroy(other.gameObject);
            GameManager.Instance.NextLevel();
        }
    }

    /// <summary>
    /// Slow down time so that every object goes slowly.
    /// </summary>
    /// <param name="timeSlowed">How many seconds the time will be slowed down.</param>
    /// <returns>Yields some time for slowing down until going back to normal.</returns>
    private IEnumerator TimeSlowed(float timeSlowed)
    {
        yield return new WaitForSecondsRealtime(timeSlowed);

        if (!GameManager.Instance.GetIsGameOver() && !GameManager.Instance.GetIsNextLevel())
        {
            Time.timeScale = 1;
            GetComponent<MeshRenderer>().material.color = Color.yellow;

            StartCoroutine(CoolDown(coolDown));
        }
            
    }

    /// <summary>
    /// Uses a cool down after slowing down time.
    /// </summary>
    /// <param name="coolDown">The number of seconds the cool down lasts.</param>
    /// <returns>Yields some time for the cool down until going back to normal.</returns>
    private IEnumerator CoolDown(float coolDown)
    {
        yield return new WaitForSecondsRealtime(coolDown);

        if (!GameManager.Instance.GetIsGameOver() && !GameManager.Instance.GetIsNextLevel())
        {
            GetComponent<MeshRenderer>().material.color = Color.black;

            slowingDown = false;
        }   
    }
}
