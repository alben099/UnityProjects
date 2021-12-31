using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base Enemy script that can spot the player.
/// </summary>
public class Enemy : MonoBehaviour
{
    private bool sawPlayer;

    /// <summary>
    /// Use a raycast to find the player.
    /// </summary>
    public void FindPlayer()
    {
        Vector3 rayOrigin = this.transform.position;
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, this.transform.forward, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Player") && !sawPlayer)
            {
                GameManager.Instance.GameOver();
                sawPlayer = true;
            }
        }
    }
}
