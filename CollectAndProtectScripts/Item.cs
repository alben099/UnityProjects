using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable Object for the collectables. Contains variables for the name,
/// point value, and how many times it needs to be clicked before it's collected.
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public new string name;
    public int pointValue;
    public int clickLimit;
}
