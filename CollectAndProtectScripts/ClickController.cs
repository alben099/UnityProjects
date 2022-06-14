using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Use the mouse to click on collectables and enemies.
/// </summary>
[RequireComponent(typeof(Camera))]
public class ClickController : MonoBehaviour
{
    [Tooltip("How far can the mouse click on objects.")]
    public float raycastDistance = 100;
    [Tooltip("Layer that this controller uses to click on collectables & enemies that have that layer.")] 
    public LayerMask clickableMask;

    [Header("Effect Variables")]
    public ParticleSystem collectParticles;
    public ParticleSystem enemyParticles;

    Camera mainCamera;
    private bool canClick;

    #region Singleton
    private static ClickController instance;

    /// <summary>
    /// Create a singleton of the click controller
    /// </summary>
    public static ClickController Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("The ClickController is NULL.");
            }

            return instance;
        }
    }

    /// <summary>
    /// When the game starts, set this game object as a singleton
    /// </summary>
    private void Awake()
    {
        instance = this;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        canClick = true;
    }

    /// <summary>
    /// While the game is not over, use the mouse and raycasts to get
    /// collectables or damage enemies.
    /// </summary>
    void Update()
    {
        if (canClick && Input.GetButtonDown("Fire1"))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, raycastDistance, clickableMask))
            {
                if (hitInfo.collider.GetComponent<Collectible>())
                {
                    collectParticles.transform.position = hitInfo.transform.position;
                    collectParticles.Play();
                    hitInfo.collider.GetComponent<Collectible>().ClickToCollect();
                }
                else if (hitInfo.collider.GetComponent<Enemy>())
                {
                    enemyParticles.transform.position = hitInfo.transform.position;
                    enemyParticles.Play();
                    hitInfo.collider.GetComponent<Enemy>().ClickToDamage();
                }
            }
        }
    }

    public void SetCanClick(bool newCanClick)
    {
        canClick = newCanClick;
    }
}
