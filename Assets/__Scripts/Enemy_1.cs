using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1: Enemy
{
    [Header("Enemy_1 Inscribed Fields")]
    [Tooltip("# of seconds for a full sine wave")]
    public float waveFrequency = 2;
    [Tooltip("Sine wave width in meters")]
    public float waveWidth = 4;
    [Tooltip("Amount the ship will roll left and right with the sine wave")]
    public float waveRotY = 45;

    private float x0;
    private float birthTime;

    void Start()
    {
        x0 = position.x;
        birthTime = Time.time;
    }

    public override void Move()
    {
        Vector3 tempPosition = position;
        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        tempPosition.x = x0 + waveWidth * sin;
        position = tempPosition;

        Vector3 rotation = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rotation);

        base.Move();
    }
}
