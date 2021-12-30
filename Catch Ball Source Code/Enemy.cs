using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The enemy script that makes the enemy throw balls automatically
/// </summary>
public class Enemy : MonoBehaviour
{
    // Throw power and cool down time
    public float forwardPower = 1500.0f;
    public float upwardPower = 500.0f;
    public float coolDownTime = 2.0f;

    // Private variables
    private Transform throwDest;

    /// <summary>
    /// Start at frame one to get position to throw balls, get the object pool of
    /// balls, and start throwing them automatically
    /// </summary>
    void Start()
    {
        throwDest = this.gameObject.transform.GetChild(0).transform;
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
        StartCoroutine(ThrowCoolDown(coolDownTime));
    }

    /// <summary>
    /// Yields time before another ball is thrown
    /// </summary>
    /// <param name="coolDownTime">Time in seconds before it throws another ball</param>
    /// <returns>Time until it goes back to normal</returns>
    IEnumerator ThrowCoolDown(float coolDownTime)
    {
        yield return new WaitForSeconds(coolDownTime);
        ThrowBall();
    }
}
