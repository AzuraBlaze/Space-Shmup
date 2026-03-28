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
    //
}
