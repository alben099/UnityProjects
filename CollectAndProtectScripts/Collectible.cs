using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Set the collectable object so that whenever it is clicked on, points are added
/// to the Game Manager.
/// </summary>
public class Collectible : MonoBehaviour
{
    [Tooltip("Item object that will be clicked on for points.")] 
    public Item item;
    [Tooltip("Number of seconds the collectable deactivates itself so that it can be reused with object pools.")]
    public float deactivationDelay = 10.0f;

    private int clickTimes;
    private bool isClickable;

    private void Start()
    {
        clickTimes = 0;
        isClickable = true;
    }

    /// <summary>
    /// Clicking on this object adds how many times it was clicked. If
    /// the number of clicks on this object gets to its set click limit,
    /// then this object will go to the storage area in the scene
    /// and add points to the Game Manager.
    /// </summary>
    public void ClickToCollect()
    {
        if (isClickable)
        {
            clickTimes++;
            if (clickTimes >= item.clickLimit)
            {
                isClickable = false;
                clickTimes = 0;
                this.GetComponent<Rigidbody>().Sleep();
                this.transform.position = GameManager.Instance.DropCollectibleRandomly(true);
                GameManager.Instance.AddToScore(item.pointValue);
                GameManager.Instance.SubtractCollectiblesInCollectDropZone();
                StartCoroutine(Deactivate());
            }
        }
    }

    /// <summary>
    /// Wait a set number of seconds before this object deactivates itself.
    /// </summary>
    /// <returns>IEnumerator to wait for a set number of seconds.</returns>
    private IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(deactivationDelay);
        clickTimes = 0;
        isClickable = true;
        
        this.gameObject.SetActive(false);
    }
}
