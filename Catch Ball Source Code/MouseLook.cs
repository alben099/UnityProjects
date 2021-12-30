using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The mouse controls to rotate the player and move the camera
/// </summary>
public class MouseLook : MonoBehaviour
{
    // Mouse settings
    public float mouseSensitivity = 100f;
    public float smoothCameraSpeed = 10f;

    // The player's body to rotate
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

        /*Quaternion desiredRotation = transform.localRotation;
        desiredRotation.x = -Mathf.Clamp(mouseY, -0.5f, 0.5f);
        if (mouseY != 0)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, desiredRotation, smoothCameraSpeed * Time.deltaTime);
        }*/

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -75f, 75f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
