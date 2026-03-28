using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    static public Vector3 Bezier(float u, params Vector3[] points)
    {
        Vector3[,] vectors = new Vector3[points.Length, points.Length];

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
}
