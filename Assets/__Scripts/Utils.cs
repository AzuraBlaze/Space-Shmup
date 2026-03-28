using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    static public Vector3 Bezier(float u, params Vector3[] points)
    {
        var vectors = new Vector3[points.Length, points.Length];

        int row = points.Length - 1;
        for (int column = 0; column < points.Length; column++)
        {
            vectors[row, column] = points[column];
        }

        for (row--; row >= 0; row--)
        {
            for (int column = 0; column <= row; column++)
            {
                vectors[row, column] = Vector3.LerpUnclamped(
                    vectors[row + 1, column],
                    vectors[row + 1, column + 1],
                    u
                );
            }
        }

        return vectors[0, 0];
    }

    static public Material[] GetAllMaterials(GameObject go)
    {
        var renderers = go.GetComponentsInChildren<Renderer>();
        var materials = new Material[renderers.Length];
        
        for (int i = 0; i < renderers.Length; i++)
        {
            materials[i] = renderers[i].material;
        }

        return materials;
    }
}
