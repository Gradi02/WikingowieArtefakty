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
    private List<GameObject> enemiesList = new List<GameObject>();

    [Header("prefabs")]
    public GameObject enemy1;

    [Header("references")]
    public TimeManager timeManager;
    private bool night = false;
    private bool nextSpawn = true;

    private void Update()
    {
        if (!IsHost) return;

        if(timeManager != null)
        {
            if (timeManager.n_hour.Value > 22 || timeManager.n_hour.Value < 6)
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

    [ContextMenu("Spawn")]
    private void SpawnEnemySetUp()
    {
        int playersNum = NetworkManager.Singleton.ConnectedClients.Count;
        int numOfEnemy = Random.Range(waveCountMin, waveCountMax+1) + playersNum;

        float posX;
        float posY;
        Vector3 spawn;
        Vector3[] curPos = new Vector3[numOfEnemy];

        for (int i=0; i<numOfEnemy; i++)
        {
            do {
                posX = Random.Range(-10, 10);
                posY = Random.Range(-10, 10);
                spawn = new Vector3(posX, 0.25f, posY) + NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.ConnectedClientsList[Random.Range(0, playersNum)].ClientId].PlayerObject.transform.position;
            } while (!CheckForSpawnPlace(spawn, i, curPos));

            curPos[i] = spawn;
            SpawnEnemyServerRpc(spawn);
        }
        nextSpawn = true;
    }

    private bool CheckForSpawnPlace(Vector3 pos, int ij, Vector3[] tab)
    {
        RaycastHit hit;
        if(Physics.Raycast(pos + new Vector3(0,10,0), Vector3.down, out hit, 10))
        {
            if (hit.transform.GetComponent<BlockManager>() != null)
            {
                return false;
            }

            if (hit.transform.GetComponent<Bridge>() != null)
            {
                return false;
            }
        }

        for(int i = 0; i < ij; i++)
        {
            if(pos == tab[i])
            {
                return false;
            }
        }

        return true;
    }


    [ServerRpc]
    private void SpawnEnemyServerRpc(Vector3 pos)
    {
        if (enemiesList.Count <= maxEnemies)
        {
            GameObject e = Instantiate(enemy1, pos, Quaternion.identity);
            e.GetComponent<NetworkObject>().Spawn();
            //e.transform.position = pos; 
            enemiesList.Add(e);
            SpawnEnemyClientRpc(e.GetComponent<NetworkObject>().NetworkObjectId, SelectType());
        }
    }

    [ClientRpc]
    private void SpawnEnemyClientRpc(ulong id, int type)
    {
        GameObject e = NetworkManager.Singleton.SpawnManager.SpawnedObjects[id].gameObject;
        e.name = "spawner";
        e.GetComponent<SpawnerManager>().monsterType = type;
    }


    int SelectType()
    {
        return Random.Range(0, 2);
    }
}
