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

    [Header("Dynamic")] [Range(0, 4)]
    public float shieldLevel = 1;

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
    }
}
