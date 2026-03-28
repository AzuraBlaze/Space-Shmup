using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eWeaponType { none, blaster, spread, shield };

[System.Serializable]
public class WeaponDefinition
{
    public eWeaponType type = eWeaponType.none;
    [Tooltip("Letter to show on the PowerUp Cube")]
    public string letter;
    [Tooltip("Color of PowerUp Cube")]
    public Color powerUpColor = Color.white;
    [Tooltip("Prefab of Weapon model that is attached to Player Ship")]
    public GameObject weaponModelPrefab;
    [Tooltip("Prefab of projectile that is fired")]
    public GameObject projectilePrefab;
    [Tooltip("Color of the Projectile that is fired")]
    public Color projectileColor = Color.white;
    [Tooltip("Damage caused when a single projectile hits an Enemy")]
    public float damageOnHit = 0;
    [Tooltip("Seconds to delay between shots")]
    public float delayBetweenShots = 0;
    [Tooltip("Velocity of individual Projectiles")]
    public float velocity = 50;
}

public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;

    [Header("Dynamic")]
    [Tooltip("Setting this manually while playing does not work properly")]
    [SerializeField] private eWeaponType _type = eWeaponType.none;
    public WeaponDefinition definition;
    public float nextShotTime;

    private GameObject weaponModel;
    private Transform shotPointTransform;

    void Start()
    {
        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }

        shotPointTransform = transform.GetChild(0);

        SetType(_type);

        Hero hero = GetComponentInParent<Hero>();
        if (hero != null) hero.fireEvent += Fire;
    }

    public eWeaponType type
    {
        get { return _type; }
        set { SetType(value); }
    }

    public void SetType(eWeaponType weaponType)
    {
        _type = weaponType;
        if (type == eWeaponType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }

        definition = Main.GET_WEAPON_DEFINITION(_type);
        
        if (weaponModel != null) Destroy(weaponModel);
        weaponModel = Instantiate<GameObject>(definition.weaponModelPrefab, transform);
        weaponModel.transform.localPosition = Vector3.zero;
        weaponModel.transform.localScale = Vector3.one;

        nextShotTime = 0;
    }

    private void Fire()
    {
        if (!gameObject.activeInHierarchy) return;
        if (Time.time < nextShotTime) return;

        ProjectileHero projectile;
        Vector3 velocity = Vector3.up * definition.velocity;

        switch(type)
        {
            case eWeaponType.blaster:
                projectile = MakeProjectile();
                projectile.velocity = velocity;
                break;
            
            case eWeaponType.spread:
                projectile = MakeProjectile();
                projectile.velocity = velocity;
                projectile = MakeProjectile();
                projectile.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                projectile.velocity = projectile.transform.rotation * velocity;
                projectile = MakeProjectile();
                projectile.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                projectile.velocity = projectile.transform.rotation * velocity;
                break;
        }
    }

    private ProjectileHero MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(definition.projectilePrefab, PROJECTILE_ANCHOR);
        ProjectileHero projectile = go.GetComponent<ProjectileHero>();

        Vector3 position = shotPointTransform.position;
        position.z = 0;
        projectile.transform.position = position;

        projectile.type = type;
        nextShotTime = Time.time + definition.delayBetweenShots;
        return projectile;
    }
}
