using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{
    [Header("Enemy_2 Inscribed Fields")]
    public float lifeTime = 10;
    [Tooltip("Determines how much the Sine wave will ease the interpolation")]
    public float sinEccentricity = 0.6f;
    public AnimationCurve rotationCurve;

    [Header("Enemy_2 Private Fields")]
    [SerializeField] private float birthTime;
    private Quaternion baseRotation;
    [SerializeField] private Vector3 p0, p1;

    void Start()
    {
        p0 = Vector3.zero;
        p0.x = -boundsCheck.cameraHeight - boundsCheck.radius;
        p0.y = Random.Range(-boundsCheck.cameraHeight, boundsCheck.cameraHeight);

        p1 = Vector3.zero;
        p1.x = boundsCheck.cameraHeight + boundsCheck.radius;
        p1.y = Random.Range(-boundsCheck.cameraHeight, boundsCheck.cameraHeight);

        if (Random.value > 0.5f)
        {
            p0.x *= -1;
            p1.x *= -1;
        }

        birthTime = Time.time;

        transform.position = p0;
        transform.LookAt(p1, Vector3.back);
        baseRotation = transform.rotation;
    }

    public override void Move()
    {
        float u = (Time.time - birthTime) / lifeTime;

        if (u > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        float shipRotation = rotationCurve.Evaluate(u) * 360;
        transform.rotation = baseRotation * Quaternion.Euler(-shipRotation, 0, 0);

        u += sinEccentricity * Mathf.Sin(u * Mathf.PI * 2);

        position = ((1 - u) * p0) + (u * p1);
    }
}
