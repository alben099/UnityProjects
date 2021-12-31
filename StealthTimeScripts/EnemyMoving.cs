using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Sets the enemy to move around the stage.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMoving : Enemy
{
    [Tooltip("Points in the scene where the enemy goes in a set order")]
    public GameObject[] waypoints;

    private NavMeshAgent agent;
    private int WPIndex = 0;
    private bool readyForWP = false;

    /// <summary>
    /// Starts by turning all the waypoints invisible and heads to a waypoint.
    /// </summary>
    void Start()
    {
        foreach (GameObject wp in waypoints)
        {
            wp.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        agent = this.GetComponent<NavMeshAgent>();
        GoToWaypoint(1.0f);
    }

    /// <summary>
    /// Check every frame to find a player and go to a waypoint if possible.
    /// </summary>
    void FixedUpdate()
    {
        FindPlayer();
        GoToWaypoint(6.0f);
    }

    /// <summary>
    /// The enemy heads towards a waypoint and stops at different ones.
    /// </summary>
    /// <param name="waitTime">How long it takes for the enemy to move to another
    /// waypoint when it gets close enough to it's current waypoint.</param>
    private void GoToWaypoint(float waitTime)
    {
        if (Vector3.Distance(this.transform.position, agent.destination) < 0.6f && !readyForWP)
        {
            agent.SetDestination(waypoints[WPIndex].transform.position);

            if (WPIndex < waypoints.Length - 1)
            {
                WPIndex++;
            }
            else
            {
                WPIndex = 0;
            }

            readyForWP = true;

            StartCoroutine(ReadyForWaypoint(waitTime));
        }
    }

    /// <summary>
    /// Let's the enemy know when it can go to the next waypoint.
    /// </summary>
    /// <param name="waitTime">The number of seconds it waits.</param>
    /// <returns>Yields time for waiting until it is done.</returns>
    private IEnumerator ReadyForWaypoint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        readyForWP = false;
    }
}
