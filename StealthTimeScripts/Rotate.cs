using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotates an object at a specific angle on the x, y, and z axes.
/// </summary>
public class Rotate : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.Rotate(1.0f, 0.8f, 0.7f);
    }
}
