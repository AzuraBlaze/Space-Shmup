using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public static Hero S { get; private set; }

    [Header("Inscribed")]
    public float speed = 30;
    public float rollMultiplier = -45;
    public float pitchMultiplier = 30;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;
    public Weapon[] weapons;

    [Header("Dynamic")] [Range(0, 4)] [SerializeField]
    private float _shieldLevel = 1;
    [Tooltip("This field holds a reference to the last triggering GameObject")]
    private GameObject lastTriggerGo = null;
    public delegate void WeaponFireDelegate();
    public event WeaponFireDelegate fireEvent;

    void Awake()
    {
        if (S == null)
        {
            S = this;
        }
        else
        {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
        }
    }

    void Update()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        Vector3 position = transform.position;
        position.x += horizontalAxis * speed * Time.deltaTime;
        position.y += verticalAxis * speed * Time.deltaTime;
        transform.position = position;

        transform.rotation = Quaternion.Euler(
            verticalAxis * pitchMultiplier,
            horizontalAxis * rollMultiplier,
            0
        );

        if (Input.GetAxis("Jump") == 1 && fireEvent != null)
        {
            fireEvent();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        
        if (go == lastTriggerGo) return;
        lastTriggerGo = go;

        var enemy = go.GetComponent<Enemy>();
        var powerUp = go.GetComponent<PowerUp>();
        if (enemy != null)
        {
            shieldLevel--;
            Destroy(go);
        }
        else if (powerUp != null)
        {
            AbsorbPowerUp(powerUp);
        }
        else
        {
            Debug.LogWarning("Shield trigger hit by non-Enemy: " + go.name);
        }
    }

    public void AbsorbPowerUp(PowerUp powerUp)
    {
        Debug.Log("Absorbed PowerUp: " + powerUp.type);
        switch (powerUp.type)
        {
            case eWeaponType.shield:
                shieldLevel++;
                break;
            
            default:
                if (powerUp.type == weapons[0].type)
                {
                    Weapon weapon = GetEmptyWeaponSlot();
                    if (weapon != null)
                    {
                        weapon.SetType(powerUp.type);
                    }
                }
                else
                {
                    ClearWeapons();
                    weapons[0].SetType(powerUp.type);
                }
                break;
        }
        powerUp.AbsorbedBy(this.gameObject);
    }

    public float shieldLevel
    {
        get { return _shieldLevel; }
        private set
        {
            _shieldLevel = Mathf.Min(value, 4);

            if (value < 0)
            {
                Destroy(this.gameObject);
                Main.HERO_DIED();
            }
        }
    }

    Weapon GetEmptyWeaponSlot()
    {
        foreach (var weapon in weapons)
        {
            if (weapon.type == eWeaponType.none)
            {
                return weapon;
            }
        }

        return null;
    }

    void ClearWeapons()
    {
        foreach (var weapon in weapons)
        {
            weapon.SetType(eWeaponType.none);
        }
    }
}
