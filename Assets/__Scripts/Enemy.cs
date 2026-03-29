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
    public float powerUpDropChance = 1f;
    public GameObject explosionPrefab;

    protected bool calledShipDestroyed = false;
    protected BoundsCheck boundsCheck;

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

        var projectile = otherGO.GetComponent<ProjectileHero>();
        if (projectile != null)
        {
            if (boundsCheck.isOnScreen)
            {
                health -= Main.GET_WEAPON_DEFINITION(projectile.type).damageOnHit;
                
                if (health <= 0)
                {
                    if (!calledShipDestroyed)
                    {
                        calledShipDestroyed = true;
                        Main.ADD_SCORE(score);
                        Main.SHIP_DESTROYED(this);
                        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                    }

                    Destroy(this.gameObject);
                }
            }
            
            Destroy(otherGO);
        }
        else
        {
            print("Enemy hit by non-ProjectileHero: " + otherGO.name);
        }
    }
}
