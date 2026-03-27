using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static private Main S;

    [Header("Inscribed")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyInsetDefault = 1.5f;

    private BoundsCheck boundsCheck;

    void Awake()
    {
        S = this;
        boundsCheck = GetComponent<BoundsCheck>();
        Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);
    }

    public void SpawnEnemy()
    {
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

        Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);
    }
}
