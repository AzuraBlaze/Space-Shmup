using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Inscribed")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score = 100;

    public Vector3 position
    {
        get { return this.transform.position; }
        set { this.transform.position = value; }
    }

    void Update()
    {
        Move();
    }

    public virtual void Move()
    {
        Vector3 tempPos = position;
        tempPos.y -= speed * Time.deltaTime;
        position = tempPos;
    }
}
