using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The mouse controls to rotate the player and move the camera
/// </summary>
public class MouseLook : MonoBehaviour
{
    [Tooltip("How fast the player can move the camera my moving the mouse cursor")]
    public float mouseSensitivity = 100f;
    [Tooltip("The player's transform for the camera to move with it")]
    public Transform playerBody;

    // Private variables
    private float xRotation = 0f;

    /// <summary>
    /// Start on frame one to keep the mouse at the center of the screen
    /// </summary>
    void Start()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Mouse will rotate the player left and right, and move the camera up or down
    /// </summary>
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -75f, 75f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
