using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The enemy script that makes the enemy throw balls automatically
/// </summary>
public class Enemy : MonoBehaviour
{
    // Throw power and cool down time
    [Tooltip("How much power is used to throw the ball in front of the enemy")]
    public float forwardPower = 1500.0f;
    [Tooltip("How much power is used to throw the ball upwards from the enemy")]
    public float upwardPower = 500.0f;
    [Tooltip("How many seconds it takes before the enemy can throw the ball again")]
    public float coolDownTime = 2.0f;

    // Protected variables
    protected Transform throwDest;

    /// <summary>
    /// Start at frame one to get position to throw balls, get the object pool of
    /// balls, and start the throwing cycle
    /// </summary>
    void Start()
    {
        throwDest = this.gameObject.transform.GetChild(0).transform;
        StartThrowCycle();
    }

    /// <summary>
    /// The base method to start the enemy's throwing cycle
    /// </summary>
    public virtual void StartThrowCycle()
    {
        StartCoroutine(ThrowCoolDown(coolDownTime));
    }

    /// <summary>
    /// Throws a ball at the specified direction, position, and force
    /// </summary>
    void ThrowBall()
    {
        GameObject ball = ObjectPools.Instance.SpawnFromPool("Ball");
        ball.gameObject.GetComponent<Ball>().ThrownByEnemy();
        ball.gameObject.GetComponent<Rigidbody>().Sleep();
        ball.transform.position = throwDest.position + throwDest.forward;
        ball.transform.localRotation = throwDest.localRotation;
        ball.GetComponent<Rigidbody>().AddForce(throwDest.forward * forwardPower);
        ball.GetComponent<Rigidbody>().AddForce(throwDest.up * upwardPower);
        StartThrowCycle();
    }

    /// <summary>
    /// Yields time before another ball is thrown
    /// </summary>
    /// <param name="coolDownTime">Time in seconds before it throws another ball</param>
    /// <returns>Time until it goes back to normal</returns>
    protected IEnumerator ThrowCoolDown(float coolDownTime)
    {
        yield return new WaitForSeconds(coolDownTime);
        ThrowBall();
    }
}
