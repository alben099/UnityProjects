using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The boundary that lowers the player's health to zero if touched
/// </summary>
public class Boundary : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.UpdateHealth(0);
            GameManager.Instance.WonGame(false);
        }
    }
}
