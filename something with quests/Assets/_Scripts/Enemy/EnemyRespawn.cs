using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawn : MonoBehaviour
{
    public static EnemyRespawn Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public IEnumerator Respawn(float time, GameObject enemy, Vector3 position, Quaternion rotation, GameObject toDestroy)
    {
        yield return new WaitForSeconds(time);
        GameObject spawnedEnemy = Instantiate(enemy, position, rotation);

        // yield return new WaitForSeconds(time);
        // destroy old game object
        Destroy(toDestroy);
        
    }
}
