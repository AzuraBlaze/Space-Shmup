using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundsCheck))]
public class PowerUp : MonoBehaviour
{
    [Header("Inscribed")]
    [Tooltip("x holds a min value and y a max value for a Random.Range() call.")]
    public Vector2 rotationMinMax = new(15, 90);
    [Tooltip("x holds a min value and y a max value for a Random.Range() call.")]
    public Vector2 driftMinMax = new(0.25f, 2);
    public float lifeTime = 10;
    public float fadeTime = 4;

    [Header("Dynamic")]
    public eWeaponType _type;
    public GameObject cube;
    public TextMesh letter;
    public Vector3 rotationsPerSecond;
    public float birthTime;

    private Rigidbody rigid;
    private BoundsCheck boundsCheck;
    private Material cubeMaterial;

    void Awake()
    {
        cube = transform.GetChild(0).gameObject;
        letter = GetComponent<TextMesh>();
        rigid = GetComponent<Rigidbody>();
        boundsCheck = GetComponent<BoundsCheck>();
        cubeMaterial = cube.GetComponent<Renderer>().material;

        Vector3 velocity = Random.onUnitSphere;
        velocity.z = 0;
        velocity.Normalize();

        velocity *= Random.Range(driftMinMax[0], driftMinMax[1]);
        rigid.velocity = velocity;

        transform.rotation = Quaternion.identity;

        rotationsPerSecond = new(
            Random.Range(rotationMinMax[0], rotationMinMax[1]),
            Random.Range(rotationMinMax[0], rotationMinMax[1]),
            Random.Range(rotationMinMax[0], rotationMinMax[1])
        );

        birthTime = Time.time;
    }

    void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotationsPerSecond * Time.time);

        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
        
        if (u >= 1)
        {
            Destroy(this.gameObject);
            return;
        }

        if (u > 0)
        {
            Color color = cubeMaterial.color;
            color.a = 1f - u;
            cubeMaterial.color = color;

            color = letter.color;
            color.a = 1f - (u * 0.5f);
            letter.color = color;
        }

        if (!boundsCheck.isOnScreen)
        {
            Destroy(gameObject);
        }
    }

    public eWeaponType type
    {
        get { return _type; }
        set { SetType(value); }
    }

    public void SetType(eWeaponType weaponType)
    {
        WeaponDefinition defintion = Main.GET_WEAPON_DEFINITION(weaponType);
        cubeMaterial.color = defintion.powerUpColor;
        letter.text = defintion.letter;
        _type = weaponType;
    }

    public void AbsorbedBy(GameObject target)
    {
        Destroy(this.gameObject);
    }
}
