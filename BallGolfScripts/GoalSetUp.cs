using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates the goal hole by cutting a square on a plane.
/// </summary>
public class GoalSetUp : MonoBehaviour
{
    private int[] middleTriangles = { 73, 93, 175, 174, 92, 98, 113, 113 };

    /// <summary>
    /// Get rid of the starting mesh collider, delete the visible triangles, and
    /// create a new mesh collider.
    /// </summary>
    void Start()
    {
        Destroy(this.gameObject.GetComponent<MeshCollider>());
        foreach (var triangle in middleTriangles)
        {
            deleteTri(triangle);
        }
        this.gameObject.AddComponent<MeshCollider>();
    }

    /// <summary>
    /// Deletes the triangle starting at a specific index of the plane's array of triangles.
    /// </summary>
    /// <param name="index">The starting index of the triangle.</param>
    void deleteTri(int index)
    {
        Mesh mesh = transform.GetComponent<MeshFilter>().mesh;
        int[] oldTriangles = mesh.triangles;
        int[] newTriangles = new int[mesh.triangles.Length - 3];

        int i = 0;
        int j = 0;
        while (j < mesh.triangles.Length)
        {
            if (j != index * 3)
            {
                for (int count = 0; count < 3; count++)
                {
                    newTriangles[i++] = oldTriangles[j++];
                }
            }
            else
            {
                j += 3;
            }
        }

        transform.GetComponent<MeshFilter>().mesh.triangles = newTriangles;
    }
}
