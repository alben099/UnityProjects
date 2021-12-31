using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Sets the enemy to turn at one spot.
/// </summary>
public class EnemyTurning : Enemy
{
    [Tooltip("How many seconds the enemy turns")]
    [Range(0.0f, 10.0f)]
    public float turnTime;
    [Tooltip("How many seconds the enemy stops before turning again")]
    [Range(0.0f, 5.0f)]
    public float stopTime;
    [Tooltip("How fast the enemy rotates")]
    [Range(-180.0f, 180.0f)]
    public float rotationSpeed;

    private bool turning = true;

    /// <summary>
    /// Start to turn at some number of seconds at the first frame.
    /// </summary>
    void Start()
    {
        StartCoroutine(StartTurning(turnTime));
    }

    /// <summary>
    /// Check every frame to find the player while turning.
    /// </summary>
    void FixedUpdate()
    {
        FindPlayer();
        Turn();
    }

    /// <summary>
    /// Keep turning on the y axis.
    /// </summary>
    private void Turn()
    {
        if (turning)
        {
            transform.Rotate(new Vector3(0, 1, 0) * rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Enemy turns, the stops, then turns in the opposite direction.
    /// </summary>
    /// <param name="turnTime">The number of seconds it turns until it stops.</param>
    /// <returns>Yields time for turning until it stops.</returns>
    private IEnumerator StartTurning(float turnTime)
    {
        yield return new WaitForSeconds(turnTime);

        StartCoroutine(StopTurning(turnTime));

        rotationSpeed *= -1;
        turning = false;
    }

    /// <summary>
    /// Stops turning for some time before allowing the enemy to turn again.
    /// </summary>
    /// <param name="turnTime">The number of seconds to hold the enemy still.</param>
    /// <returns>Yields time for stopping until it's ready to turn again.</returns>
    private IEnumerator StopTurning(float turnTime)
    {
        yield return new WaitForSeconds(stopTime);

        turning = true;

        StartCoroutine(StartTurning(turnTime));
    }
}
