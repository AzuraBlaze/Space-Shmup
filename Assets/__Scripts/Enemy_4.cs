using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyShield))]
public class Enemy_4 : Enemy
{
    [Header("Enemy_4 Inscribed Fields")]
    public float duration = 4;

    private EnemyShield[] allShields;
    private EnemyShield thisShield;
    private Vector3 p0, p1;
    private float timeStart;

    void Start()
    {
        allShields = GetComponentsInChildren<EnemyShield>();
        thisShield = GetComponent<EnemyShield>();

        p0 = p1 = position;
        InitMovement();
    }

    void InitMovement()
    {
        p0 = p1;

        float widthMinRadius = boundsCheck.cameraWidth - boundsCheck.radius;
        float heightMinRadius = boundsCheck.cameraHeight - boundsCheck.radius;
        p1.x = Random.Range(-widthMinRadius, widthMinRadius);
        p1.y = Random.Range(-heightMinRadius, heightMinRadius);

        if (p0.x * p1.x > 0 && p0.y * p1.y > 0)
        {
            if (Mathf.Abs(p0.x) > Mathf.Abs(p0.y))
            {
                p1.x *= -1;
            }
            else
            {
                p1.y *= -1;
            }
        }

        timeStart = Time.time;
    }

    public override void Move()
    {
        float u = (Time.time - timeStart) / duration;

        if (u >= 1)
        {
            InitMovement();
            u = 0;
        }

        u -= 0.15f * Mathf.Sin(u * 2 * Mathf.PI);
        position = ((1 - u) * p0) + (u * p1);
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject otherGO = collision.gameObject;

        var projectile = otherGO.GetComponent<ProjectileHero>();
        if (projectile != null)
        {
            Destroy(otherGO);

            if (boundsCheck.isOnScreen)
            {
                GameObject hitGO = collision.contacts[0].thisCollider.gameObject;
                if (hitGO == otherGO)
                {
                    hitGO = collision.contacts[0].otherCollider.gameObject;
                }

                float damage = Main.GET_WEAPON_DEFINITION(projectile.type).damageOnHit;

                bool shieldFound = false;
                foreach (var enemyShield in allShields)
                {
                    if (enemyShield.gameObject == hitGO)
                    {
                        enemyShield.TakeDamage(damage);
                        shieldFound = true;
                    }
                }
                if (!shieldFound) thisShield.TakeDamage(damage);

                if (thisShield.isActive) return;

                if (!calledShipDestroyed)
                {
                    Main.SHIP_DESTROYED(this);
                    calledShipDestroyed = true;
                }

                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("Enemy_4 hit by non-ProjectileHero: " + otherGO.name);
        }
    }
}
