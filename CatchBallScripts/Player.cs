using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The player script that allows the user to move in first person,
/// jump, and catch & throw balls at enemies
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Move and Jump Variables")]
    [Tooltip("How fast the player can move")]
    public float speed = 12f;
    [Tooltip("The force of gravity affecting the player")]
    public float gravity = -9.81f;
    [Tooltip("How high the player can jump")]
    public float jumpHeight = 3.0f;

    [Header("Ground Check")]
    [Tooltip("The child transform at the bottom of the player to check if the player is above ground")]
    public Transform groundCheck;
    [Tooltip("Radius of the groundCheck to see if the player is on the ground")]
    public float groundDistance = 0.4f;
    [Tooltip("The LayerMask to check if the object the player is standing on is the ground")]
    public LayerMask groundMask;

    [Header("Aiming Components")]
    [Tooltip("The main camera that is used for first-person viewing")]
    public Camera mainCamera;
    [Tooltip("The child transform at the front of the player to hold objects like balls")]
    public Transform dest;
    [Tooltip("The image of a reticle to show where the player can catch the balls")]
    public Image reticle;

    [Header("Throwing Variables")]
    [Tooltip("How far the player can get the balls")]
    public float pickUpRange = 10f;
    [Tooltip("How fast the ball can be charged to sent the balls further")]
    public float chargeRate = 5f;
    [Tooltip("How many seconds before the player can grab another ball after throwing one")]
    public float coolDownTime = 1.0f;
    [Tooltip("How far the player throws the ball by default")]
    public float forwardPower = 1500.0f;
    [Tooltip("How high the player throws the ball by default")]
    public float upwardPower = 500.0f;

    // Private variables
    private CharacterController controller;
    private Vector3 velocity;
    private int health;
    private float initialForwardForce;
    private float extraForwardForce;
    private bool isGrounded;
    private bool isReadyToCharge;
    private bool isHolding;
    private bool isCooledDown;

    /// <summary>
    /// Start on frame one some player settings including the player's health
    /// </summary>
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        health = 3;
        initialForwardForce = 100;
        extraForwardForce = initialForwardForce;
        isReadyToCharge = false;
        isHolding = false;
        isCooledDown = true;
    }

    /// <summary>
    /// Handles functions for catching and throwing balls
    /// </summary>
    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isHolding && isCooledDown)
        {
            PickUp();
        }

        if (Input.GetButtonUp("Fire1") && isCooledDown)
        {
            if (isHolding && !isReadyToCharge)
            {
                isReadyToCharge = true;
            }
            else if (isHolding && isReadyToCharge)
            {
                Throw();
                isReadyToCharge = false;
            }
            
        }

        if (Input.GetButton("Fire1") && isHolding && isReadyToCharge && extraForwardForce < 300.0f)
        {
            extraForwardForce += chargeRate;
            switch (extraForwardForce)
            {
                case 150:
                    dest.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
                    break;
                case 200:
                    dest.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                    break;
                case 250:
                    dest.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    break;
                case 300:
                    dest.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
                    break;
            }
        }
    }

    /// <summary>
    /// Updates the physics of the player's movement and velocity in the air
    /// </summary>
    private void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Direction based on where the player is facing
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // Jump mechanic
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Use a sphere to check the ground with the radius and mask
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    /// <summary>
    /// Updates the reticle and ability to catch & throw balls
    /// </summary>
    private void LateUpdate()
    {
        // Change reticle if it sees a ball or not, and be invisible when a ball is picked up
        if (!isHolding)
        {
            reticle.enabled = true;
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, pickUpRange))
            {
                if (hit.collider.gameObject.CompareTag("PickUp"))
                {
                    reticle.color = Color.red;
                    reticle.rectTransform.sizeDelta = new Vector2(30, 30);
                }
                else
                {
                    reticle.color = Color.yellow;
                    reticle.rectTransform.sizeDelta = new Vector2(15, 15);
                }
            }
            else
            {
                reticle.color = Color.yellow;
                reticle.rectTransform.sizeDelta = new Vector2(15, 15);
            }
        }
        else if (isHolding && isCooledDown)
        {
            reticle.enabled = false;
        }

        // Press enter when the game is over to continue
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameManager.Instance.ContinueScenes();
        }
    }

    /// <summary>
    /// Lowers the player's health until it reaches zero
    /// </summary>
    /// <param name="damage">Number of hit points taken from health</param>
    public void TakeDamage(int damage)
    {
        health -= damage;
        GameManager.Instance.UpdateHealth(health);
        if (health <= 0)
        {
            health = 0;
            GameManager.Instance.WonGame(false);
        }
    }

    /// <summary>
    /// Allows the player to pick up a ball from an enemy
    /// </summary>
    private void PickUp()
    {
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, pickUpRange))
        {
            if (!isHolding && hit.collider.gameObject.CompareTag("PickUp"))
            {
                hit.collider.GetComponent<Ball>().GrabbedByPlayer();
                hit.collider.GetComponent<Rigidbody>().useGravity = false;
                hit.collider.GetComponent<Rigidbody>().freezeRotation = true;
                hit.collider.GetComponent<Rigidbody>().velocity = Vector3.zero;
                hit.collider.GetComponent<SphereCollider>().enabled = false;
                hit.collider.transform.position = dest.position;
                hit.collider.transform.parent = dest.transform;
                isHolding = true;
            }
        }
    }

    /// <summary>
    /// Allows the player to throw a ball to an enemy
    /// </summary>
    private void Throw()
    {
        dest.GetChild(0).position = dest.position;
        dest.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
        dest.GetChild(0).GetComponent<Rigidbody>().freezeRotation = false;
        dest.GetChild(0).GetComponent<SphereCollider>().enabled = true;
        dest.GetChild(0).GetComponent<Rigidbody>().AddForce(dest.forward * forwardPower * (extraForwardForce / 100));
        dest.GetChild(0).GetComponent<Rigidbody>().AddForce(dest.up * upwardPower);
        dest.DetachChildren();
        extraForwardForce = initialForwardForce;
        isCooledDown = false;
        StartCoroutine(ThrowCoolDown(coolDownTime));
    }

    /// <summary>
    /// Yields time for when the player can catch a ball again
    /// </summary>
    /// <param name="coolDownTime">Number of seconds before player can catch a ball again</param>
    /// <returns>Time until it goes back to normal</returns>
    IEnumerator ThrowCoolDown(float coolDownTime)
    {
        reticle.enabled = true;
        reticle.color = Color.gray;
        reticle.rectTransform.sizeDelta = new Vector2(15, 15);
        yield return new WaitForSeconds(coolDownTime);
        isHolding = false;
        isCooledDown = true;
    }
}
