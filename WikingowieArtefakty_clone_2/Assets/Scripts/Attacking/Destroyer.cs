using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class Destroyer : NetworkBehaviour
{
    public float time = 2;
    private void Awake()
    {
        Invoke(nameof(DespawnObjectServerRpc), 2);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnObjectServerRpc()
    {
        GetComponent<NetworkObject>().Despawn();
    }
}
