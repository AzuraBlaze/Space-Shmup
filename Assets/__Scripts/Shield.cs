using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Inscribed")]
    public float rotationsPerSecond = 0.1f;

    [Header("Dynamic")]
    public int levelShown = 0;

    Material material;

    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        int currentLevel = Mathf.FloorToInt(Hero.S.shieldLevel);

        if (levelShown != currentLevel)
        {
            levelShown = currentLevel;
            material.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
        }

        float rotationZ = -(rotationsPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }
}
