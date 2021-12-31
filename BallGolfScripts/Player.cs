using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The Player script that lets the user control the player ball.
/// </summary>
public class Player : MonoBehaviour
{
    [Tooltip("How fast the ball moves by default")]
    [SerializeField] float speed;

    [Tooltip("The child transform of the pointer that makes it possible to rotate around the ball")]
    [SerializeField] GameObject pointerAnchor;
    private Transform pointer;
    private float power;
    private float pointerZPos;

    float rotationAngle;
    Vector3 movement;
    Rigidbody rb;

    [Tooltip("Text displaying how many strokes to the ball happened in a level")]
    [SerializeField] TextMeshProUGUI strokeText;
    private int strokes = 0;
    [Tooltip("The text that will display when the ball gets into the goal hole")]
    [SerializeField] TextMeshProUGUI winText;
    [Tooltip("The button to move to the next level that appears after the player wins")]
    [SerializeField] Button nextLevelButton;

    [Tooltip("The main camera that's rightside up")]
    [SerializeField] GameObject mainCam;
    [Tooltip("An alternate camera that's used with gravity makes everything go upside down")]
    [SerializeField] GameObject upsideDownCam;
    [Tooltip("All of the walls of the level that will be invisible when the world is upside down")]
    [SerializeField] GameObject wallsBelow;
    [Tooltip("All of the walls of the level that will be invisible when the world is rightside up")]
    [SerializeField] GameObject wallsAbove;
    private bool isUpsideDown = false;

    /// <summary>
    /// Start at frame one with the ball and a pointer at a set power level and angle.
    /// </summary>
    void Start()
    {
        pointer = pointerAnchor.gameObject.transform.GetChild(0);
        power = 0.8f;
        pointerZPos = -1.5f;
        rotationAngle = 0.0f;
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Updates the physics of the pointer to smoothly rotate around the ball.
    /// </summary>
    private void FixedUpdate()
    {
        AdjustPointer();
    }

    /// <summary>
    /// Check at the end of every frame to adjust the launch power, launch the player,
    /// set its movement, and enabling the pointer
    /// </summary>
    private void LateUpdate()
    {
        AdjustPower();
        LaunchPlayer();
        SetMovement();
        EnablePointer();
    }

    /// <summary>
    /// Check what object the ball touched: finish, respawn, or gravity tile.
    /// </summary>
    /// <param name="collision">The object the ball collided with.</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            winText.gameObject.SetActive(true);
            nextLevelButton.gameObject.SetActive(true);
        }
        else if (collision.gameObject.CompareTag("Respawn"))
        {
            SetWorldRightsideUp();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (collision.gameObject.CompareTag("Gravity"))
        {
            rb.Sleep();
            if (!isUpsideDown)
            {
                SetWorldUpsideDown();
            }
            else
            {
                SetWorldRightsideUp();
            }
        }
    }

    /// <summary>
    /// Adjust how far the ball will go and resize the pointer to show the power it's at.
    /// </summary>
    private void AdjustPower()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (power < 1.5f)
            {
                power += 0.1f;
            }
            else
            {
                power = 1.5f;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (power > 0.3f)
            {
                power -= 0.1f;
            }
            else
            {
                power = 0.3f;
            }
        }
    }

    /// <summary>
    /// Launch the ball with force based on the power level and pointer position.
    /// </summary>
    private void LaunchPlayer()
    {
        if (Input.GetKeyDown(KeyCode.Space) && rb.IsSleeping())
        {
            rb.AddForce(movement * power * speed);
            strokes++;
            strokeText.SetText("Strokes: " + strokes);
        }
    }

    /// <summary>
    /// Rotate the pointer left or right around the ball.
    /// </summary>
    private void AdjustPointer()
    {
        pointerAnchor.transform.position = new Vector3(transform.position.x,
            transform.position.y, transform.position.z);
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rotationAngle--;
            if (rotationAngle < 0)
            {
                rotationAngle = 360;
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rotationAngle++;
            if (rotationAngle > 360)
            {
                rotationAngle = 0;
            }
        }
        pointerAnchor.transform.localEulerAngles = new Vector3(90.0f, 0.0f, rotationAngle)
            * Time.deltaTime * 50;
        pointer.transform.localPosition = new Vector3(0.0f, power - 4.0f, pointerZPos);
        pointer.transform.localScale = new Vector3(1.0f, power, 1.0f);
    }

    /// <summary>
    /// Set what direction the ball will go opposite to the pointer.
    /// </summary>
    private void SetMovement()
    {
        if (rotationAngle >= 0.0f && rotationAngle < 90.0f)
        {
            movement = new Vector3(-rotationAngle, 0.0f, 90.0f - rotationAngle);
        }
        else if (rotationAngle >= 90.0f && rotationAngle < 180.0f)
        {
            movement = new Vector3(rotationAngle - 180.0f, 0.0f, 90.0f - rotationAngle);
        }
        else if (rotationAngle >= 180.0f && rotationAngle < 270.0f)
        {
            movement = new Vector3(rotationAngle - 180.0f, 0.0f, rotationAngle - 270.0f);
        }
        else if (rotationAngle >= 270.0f && rotationAngle < 360.0f)
        {
            movement = new Vector3(360.0f - rotationAngle, 0.0f, rotationAngle - 270.0f);
        }
        else
        {
            movement = Vector3.zero;
        }
    }

    /// <summary>
    /// Show the pointer if the ball is not moving; otherwise, make it invisible.
    /// </summary>
    private void EnablePointer()
    {
        if (!rb.IsSleeping())
        {
            pointerAnchor.SetActive(false);
        }
        else
        {
            pointerAnchor.SetActive(true);
        }
    }

    /// <summary>
    /// Change the perspective right side up. Anything above the player is invisible.
    /// </summary>
    private void SetWorldRightsideUp()
    {
        pointerZPos = -1.5f;
        upsideDownCam.SetActive(false);
        mainCam.SetActive(true);
        wallsAbove.SetActive(false);
        wallsBelow.SetActive(true);
        isUpsideDown = false;
        Physics.gravity = new Vector3(0, -9.81f, 0);
    }

    /// <summary>
    /// Change the perspective upside down. Anything above the player is invisible.
    /// </summary>
    private void SetWorldUpsideDown()
    {
        pointerZPos = 1.5f;
        mainCam.SetActive(false);
        upsideDownCam.SetActive(true);
        wallsBelow.SetActive(false);
        wallsAbove.SetActive(true);
        isUpsideDown = true;
        Physics.gravity = new Vector3(0, 9.81f, 0);
    }
}
