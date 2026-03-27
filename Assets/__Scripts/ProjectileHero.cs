using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundsCheck))]
public class ProjectileHero : MonoBehaviour
{
    private BoundsCheck boundsCheck;

    void Awake()
    {
        boundsCheck = GetComponent<BoundsCheck>();
    }

    void Update()
    {
        if (boundsCheck.LocationIs(BoundsCheck.eScreenLocations.offUp))
        {
            Destroy(gameObject);
        }
    }
}
