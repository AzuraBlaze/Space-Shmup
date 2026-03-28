using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static private Main S;
    static private Dictionary<eWeaponType, WeaponDefinition> WEAPON_DICT;

    [Header("Inscribed")]
    public bool spawnEnemies = true;
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyInsetDefault = 1.5f;
    public float gameRestartDelay = 2;
    public WeaponDefinition[] weaponDefinitions;

    private BoundsCheck boundsCheck;

    void Awake()
    {
        S = this;
        boundsCheck = GetComponent<BoundsCheck>();
        Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);

        WEAPON_DICT = new();
        foreach (var definition in weaponDefinitions)
        {
            WEAPON_DICT[definition.type] = definition;
        }
    }

    public void SpawnEnemy()
    {
        if (!spawnEnemies)
        {
            Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);
            return;
        }
        
        int index = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[index]);

        float enemyInset = enemyInsetDefault;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyInset = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        Vector3 position = Vector3.zero;
        float xMin = -boundsCheck.cameraWidth + enemyInset;
        float xMax = boundsCheck.cameraWidth - enemyInset;
        position.x = Random.Range(xMin, xMax);
        position.y = boundsCheck.cameraHeight + enemyInset;
        go.transform.position = position;

        Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);
    }

    void DelayedRestart()
    {
        Invoke(nameof(Restart), gameRestartDelay);
    }

    void Restart()
    {
        SceneManager.LoadScene("__Scene_0");
    }

    static public void HERO_DIED()
    {
        S.DelayedRestart();
    }

    static public WeaponDefinition GET_WEAPON_DEFINITION(eWeaponType weaponType)
    {
        return WEAPON_DICT.ContainsKey(weaponType) ? WEAPON_DICT[weaponType] : new();
    }
}
