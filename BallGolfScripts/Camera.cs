using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Set the camera to follow the player.
/// </summary>
public class Camera : MonoBehaviour
{
    [Tooltip("The player ball the camera follows")]
    [SerializeField] GameObject player;

    private Vector3 offset;

    /// <summary>
    /// Create an offset which is how far the camera is to the player at the first frame.
    /// </summary>
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    /// <summary>
    /// Make the camera follow the player based on the offset at the end of every frame.
    /// </summary>
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
