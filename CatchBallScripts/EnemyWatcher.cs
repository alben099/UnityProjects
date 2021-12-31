using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The enemy script that makes the enemy throw balls only if the player is
/// within its throwing range
/// </summary>
public class EnemyWatcher : Enemy
{
    [Tooltip("If the player is less than this radius, the enemy looks at the player and starts throwing balls")]
    public float watchRadius = 15.0f;
    [Tooltip("How fast the the enemy can rotate")]
    public float rotationSpeed = 5.0f;

    private GameObject player;

    private float distance;
    private bool startThrowing;

    /// <summary>
    /// Start on frame one to setting the variables to look at the player
    /// and start throwing if they are near the enemy
    /// </summary>
    private void Start()
    {
        throwDest = this.gameObject.transform.GetChild(0).transform;
        player = GameManager.Instance.GetPlayer();
        startThrowing = false;
    }

    /// <summary>
    /// Check every frame if the player is close to the player
    /// and start throwing balls, otherwise stop throwing
    /// </summary>
    private void Update()
    {
        distance = Vector3.Distance(player.transform.position, transform.position);

        if (!startThrowing && distance <= watchRadius)
        {
            startThrowing = true; 
            StartThrowCycle();
        } 
        else if (startThrowing && distance > watchRadius)
        {
            startThrowing = false;
        }
    }

    /// <summary>
    /// Check every frame if the player is close to the player
    /// and start rotate to face them, otherwise stop looking
    /// </summary>
    private void FixedUpdate()
    {
        if (distance <= watchRadius)
        {
            FacePlayer();
        }
    }

    /// <summary>
    /// Overritten method to only start throwing balls when
    /// the player is near the enemy
    /// </summary>
    public override void StartThrowCycle()
    {
        if (startThrowing)
        {
            StartCoroutine(ThrowCoolDown(coolDownTime));
        }
    }

    /// <summary>
    /// Look at the player and rotate on the x and z axis only
    /// </summary>
    private void FacePlayer()
    {
        Vector3 lookDirection = (transform.position - player.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(lookDirection.x, 0.0f, lookDirection.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}
