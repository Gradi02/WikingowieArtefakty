using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Bridge : NetworkBehaviour
{
    public GameObject bridgePrefab;
    public ParticleSystem buildParticle;
    private GameObject bridgeObj;

    [ServerRpc(RequireOwnership = false)]
    public void BuildBridgeServerRpc()
    {
        if (bridgeObj != null)
        {
            Debug.Log("Most ju¿ zosta³ tutaj zbudowany!");
            return;
        }

        BuildBridgeClientRpc();   

        if(transform.Find("water") != null)
        {
            transform.Find("water").GetComponent<NetworkObject>().Despawn();
        }
    }

    [ClientRpc]
    public void BuildBridgeClientRpc()
    {
        bridgeObj = Instantiate(bridgePrefab, transform.position, Quaternion.identity, transform);
        bridgeObj.transform.localPosition -= new Vector3(0, 0.5f, 0);
        buildParticle.Play();
        GetComponent<BoxCollider>().enabled = false;
    }
}
