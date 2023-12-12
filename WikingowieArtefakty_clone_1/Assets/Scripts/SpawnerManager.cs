using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnerManager : NetworkBehaviour
{
    public int monsterType = 0;

    public GameObject[] enemiesPrefab;

    private void Awake()
    {
        if(!IsClient)
            Invoke("SpawnEnemyServerRpc", 4);
    }

    [ServerRpc]
    void SpawnEnemyServerRpc()
    {
        //Debug.Log("Spawned: " + monsterType + " at position " + transform.position);

        GameObject e = Instantiate(enemiesPrefab[monsterType], transform.position, Quaternion.identity);
        e.GetComponent<NetworkObject>().Spawn();

        //SpawnEnemyClientRpc(e.GetComponent<NetworkObject>().NetworkObjectId);
        gameObject.GetComponent<NetworkObject>().Despawn();
    }

    [ClientRpc]
    void SpawnEnemyClientRpc(ulong id)
    {
        GameObject e = NetworkManager.Singleton.SpawnManager.SpawnedObjects[id].gameObject;
        
    }

}
