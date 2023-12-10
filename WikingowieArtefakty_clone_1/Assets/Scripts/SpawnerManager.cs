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
        Invoke("SpawnEnemyServerRpc", 2);
    }

    [ServerRpc]
    void SpawnEnemyServerRpc()
    {
        //spawn enemy animation
        Debug.Log("Spawned: " + monsterType + " at position " + transform.position);

        /// 
        //GameObject e = Instantiate(enemiesPrefab[monsterType], transform.position, Quaternion.identity);
        

        gameObject.GetComponent<NetworkObject>().Despawn();
    }
}
