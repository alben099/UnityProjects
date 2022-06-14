using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemies will go at a set direction to go to their target and hit the storage area.
/// If they are successful, the player will lose lives. If the enemy is defeated,
/// then the player gets extra time.
/// </summary>
public class Enemy : MonoBehaviour
{
    public enum Direction { Right, Left, Forward, Backward }

    [Tooltip("The enemy can go left/right (x axis) or forward/backward (z axis)")]
    public Direction direction;
    [Tooltip("Initially, this is how fast the enemy moves.")]
    public float defaultSpeed = 1.0f;
    [Tooltip("As the score goes up, this is the maximum speed the enemy can go.")]
    public float maxSpeed = 5.0f;
    [Tooltip("This is the amount of time the player gets back after defeating the enemy.")]
    public float secondsToAddBack = 1.0f;
    [Tooltip("This is how many times the player must click on the enemy to defeat it.")]
    public int clickLimit = 1;

    private float speed;
    private int clickTimes;

    /// <summary>
    /// Start on frame one to set the enemy's current speed and rotation.
    /// </summary>
    private void Start()
    {
        speed = defaultSpeed;
        clickTimes = 0;

        if (direction == Direction.Right)
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f));
        }
        else if (direction == Direction.Left)
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 270.0f, 0.0f));
        }
        else if (direction == Direction.Forward)
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
        }
        else if (direction == Direction.Backward)
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f));
        }
    }

    /// <summary>
    /// Every fixed frame, translate this enemy based on its direction.
    /// It should be heading towards the storage target.
    /// </summary>
    private void FixedUpdate()
    {
        this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    /// <summary>
    /// If the enemy hits the storage target with the "Enemy Target" tag, 
    /// the player loses a life.
    /// </summary>
    /// <param name="other">The other Collider with a trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy Target"))
        {
            GameManager.Instance.LoseLife();
            UIManager.Instance.UpdateLives();
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Every time the enemy is clicked on, it takes damage.
    /// If it takes enough hits, it deactivates itself and
    /// gives the player extra time.
    /// </summary>
    public void ClickToDamage()
    {
        clickTimes++;
        if (clickTimes >= clickLimit)
        {
            GameManager.Instance.AddBackTime(secondsToAddBack);
            clickTimes = 0;
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// The EnemyManager will set this enemy's speed
    /// based on the score the player has.
    /// </summary>
    /// <param name="speedAdjustment">This is how much more speed is added to its current speed.</param>
    public void SetSpeed(float speedAdjustment)
    {
        speed = defaultSpeed + speedAdjustment;
        if (speed > maxSpeed)
        {
            speed = maxSpeed;
        }
    }
}
