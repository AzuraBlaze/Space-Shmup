using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3 : Enemy
{
    [Header("Enemy_3 Inscribed Fields")]
    public float lifeTime = 5;
    public Vector2 midpointYRange = new Vector2(1.5f, 3);
    [Tooltip("If true, Bezier points and path are drawn in the Scene pane.")]
    public bool drawDebugInfo = true;

    [Header("Enemy_3 Private Fields")]
    [SerializeField] private Vector3[] points;
    [SerializeField] private float birthTime;

    void Start()
    {
        points = new Vector3[3];

        points[0] = position;

        float xMin = -boundsCheck.cameraWidth + boundsCheck.radius;
        float xMax = boundsCheck.cameraWidth - boundsCheck.radius;

        points[1] = Vector3.zero;
        points[1].x = Random.Range(xMin, xMax);
        float midYMultiplier = Random.Range(midpointYRange[0], midpointYRange[1]);
        points[1].y = -boundsCheck.cameraHeight * midYMultiplier;

        points[2] = Vector3.zero;
        points[2].x = Random.Range(xMin, xMax);
        points[2].y = position.y;

        birthTime = Time.time;

        if (drawDebugInfo) DrawDebug();
    }

    public override void Move()
    {
        float u = (Time.time - birthTime) / lifeTime;

        if (u > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        transform.rotation = Quaternion.Euler(u * 180, 0, 0);

        u -= 0.1f * Mathf.Sin(u * Mathf.PI * 2);
        position = Utils.Bezier(u, points);
    }

    void DrawDebug()
    {
        Debug.DrawLine(points[0], points[1], Color.cyan, lifeTime);
        Debug.DrawLine(points[1], points[2], Color.yellow, lifeTime);

        float numSections = 20;
        Vector3 previousPoint = points[0];
        Color color;
        Vector3 point;
        for (int i = 0; i < numSections; i++)
        {
            float u = i / numSections;
            point = Utils.Bezier(u, points);
            color = Color.Lerp(Color.cyan, Color.yellow, u);
            Debug.DrawLine(previousPoint, point, color, lifeTime);
            previousPoint = point;
        }
    }
}
