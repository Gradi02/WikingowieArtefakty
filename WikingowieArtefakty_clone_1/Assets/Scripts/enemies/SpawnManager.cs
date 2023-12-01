using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnManager : NetworkBehaviour
{
    [Header("spawn settings")]
    public int waveCountMin;
    public int waveCountMax;
    public int maxEnemies;
    public float spawnPeriods;
    private List<GameObject> enemiesList = new List<GameObject> ();

    [Header("prefabs")]
    public GameObject enemy1;

    [Header("references")]
    public TimeManager timeManager;
    public bool night = false;
    private bool nextSpawn = true;

    private void Update()
    {
        if (!IsHost) return;

        if(timeManager != null)
        {
            if(timeManager.n_hour.Value > 22 && timeManager.n_hour.Value < 6)
            {
                night = true;
            }
            else
            {
                night = false;
            }
        }

        if(night && nextSpawn)
        {
            nextSpawn = false;
            Invoke(nameof(SpawnEnemySetUp), spawnPeriods);
        }
    }

    private void SpawnEnemySetUp()
    {
        int numOfEnemy = Random.Range(waveCountMin, waveCountMax+1);

        for(int i=0; i<numOfEnemy; i++)
        {
            float posX = Random.Range(-10, 10);
            float posY = Random.Range(-10, 10);
            Vector3 spawn = new Vector3(posX, 0.25f, posY);

            SpawnEnemyServerRpc(spawn);
        }
    }

    [ServerRpc]
    private void SpawnEnemyServerRpc(Vector3 pos)
    {
        if (enemiesList.Count <= maxEnemies)
        {
            GameObject e = Instantiate(enemy1, transform.position, Quaternion.identity);
            e.GetComponent<NetworkObject>().Spawn();
            e.transform.position = pos; 
            e.name = "enemy";
            enemiesList.Add(e);
        }
    }
}
