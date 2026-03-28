using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundsCheck))]
public class ProjectileHero : MonoBehaviour
{
    private BoundsCheck boundsCheck;
    private Renderer rend;

    [Header("Dynamic")]
    public Rigidbody rigid;
    [SerializeField] private eWeaponType _type;

    public eWeaponType type
    {
        get { return _type; }
        set { SetType(value); }
    }

    void Awake()
    {
        boundsCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (boundsCheck.LocationIs(BoundsCheck.eScreenLocations.offUp))
        {
            Destroy(gameObject);
        }
    }

    public void SetType(eWeaponType eType)
    {
        _type = eType;
        WeaponDefinition definition = Main.GET_WEAPON_DEFINITION(_type);
        rend.material.color = definition.projectileColor;
    }

    public Vector3 velocity
    {
        get { return rigid.velocity; }
        set { rigid.velocity = value; }
    }
}
