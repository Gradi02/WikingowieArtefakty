using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnerManager : NetworkBehaviour
{
    public int monsterType = 0;
    private void Awake()
    {
        if (IsHost)
        {
            Waiter(5);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnEnemyServerRpc()
    {
        //spawn enemy animation
        Debug.Log("Spawned: " + monsterType + " at position " + transform.position);

        gameObject.GetComponent<NetworkObject>().Despawn();
    }

    IEnumerator Waiter(float sec)
    {
        yield return new WaitForSeconds(sec);
        SpawnEnemyServerRpc();
    }
}
