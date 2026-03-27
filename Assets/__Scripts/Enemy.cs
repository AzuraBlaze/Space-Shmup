using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundsCheck))]
public class Enemy : MonoBehaviour
{
    [Header("Inscribed")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score = 100;

    private BoundsCheck boundsCheck;

    void Awake()
    {
        boundsCheck = GetComponent<BoundsCheck>();
    }

    public Vector3 position
    {
        get { return this.transform.position; }
        set { this.transform.position = value; }
    }

    void Update()
    {
        Move();

        if (boundsCheck.LocationIs(BoundsCheck.eScreenLocations.offDown))
        {
            Destroy(gameObject);
        }
    }

    public virtual void Move()
    {
        Vector3 tempPos = position;
        tempPos.y -= speed * Time.deltaTime;
        position = tempPos;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        GameObject otherGO = collision.gameObject;

        if (otherGO.GetComponent<ProjectileHero>() != null)
        {
            Destroy(otherGO);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Enemy hit by non-ProjectileHero: " + otherGO.name);
        }
    }
}
