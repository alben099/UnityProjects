using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The ball script that collides with other objects
/// </summary>
public class Ball : MonoBehaviour
{
    // Private variables
    private const int DamagePlayer = 0;
    private const int DamageEnemy = 1;
    private int currentTarget = DamagePlayer;

    /// <summary>
    /// Changes the ball to damage the player
    /// </summary>
    public void ThrownByEnemy()
    {
        currentTarget = DamagePlayer;
        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    /// <summary>
    /// Changes the ball to damage the enemy
    /// </summary>
    public void GrabbedByPlayer()
    {
        currentTarget = DamageEnemy;
        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    /// <summary>
    /// Checks what the ball hit then deactivates itself for reuse in the object pool
    /// </summary>
    /// <param name="collision">The object the ball collided with</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (currentTarget == DamagePlayer && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(1);
        }
        else if (currentTarget == DamageEnemy && collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }

        this.gameObject.SetActive(false);
    }
}
