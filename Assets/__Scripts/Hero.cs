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
        fireEvent += TempFire;
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

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     TempFire();
        // }

        if (Input.GetAxis("Jump") == 1 && fireEvent != null)
        {
            fireEvent();
        }
    }

    void TempFire()
    {
        GameObject projectileGO = Instantiate<GameObject>(projectilePrefab);
        projectileGO.transform.position = transform.position;
        Rigidbody rigidBody = projectileGO.GetComponent<Rigidbody>();

        ProjectileHero projectile = projectileGO.GetComponent<ProjectileHero>();
        projectile.type = eWeaponType.blaster;
        float typeSpeed = Main.GET_WEAPON_DEFINITION(projectile.type).velocity;
        rigidBody.velocity = Vector3.up * typeSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        
        if (go == lastTriggerGo) return;
        lastTriggerGo = go;

        Enemy enemy = go.GetComponent<Enemy>();
        if (enemy != null)
        {
            shieldLevel--;
            Destroy(go);
        }
        else
        {
            Debug.LogWarning("Shield trigger hit by non-Enemy: " + go.name);
        }
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
}
