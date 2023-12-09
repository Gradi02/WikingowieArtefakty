using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UIElements;
using TMPro;

public class Manager : NetworkBehaviour
{
    public GameObject playerPref;
    public Camera cam;
    private Vector3 middle = new();

    public GameObject[] HealthBars;
    private int playersCount;

    public void SetMiddle(Vector3 md)
    {
        middle = md;
    }

    [ServerRpc]
    public void StartGameServerRpc()
    {
        playersCount = NetworkManager.Singleton.ConnectedClients.Count;
        StartGameClientRpc(playersCount);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < playersCount; i++)
        {
            AssignBarToPlayerClientRpc(players[i].GetComponent<NetworkObject>().NetworkObjectId, i);
        }
    }

    [ClientRpc]
    public void StartGameClientRpc(int pc)
    {
        for (int i=0; i<pc; i++)
        {
            HealthBars[i].SetActive(true);
        }
    }

    [ClientRpc]
    void AssignBarToPlayerClientRpc(ulong p, int num)
    {
        GameObject player = NetworkManager.Singleton.SpawnManager.SpawnedObjects[p].gameObject;
        HealthBars[num].GetComponent<HealthController>().AssignPlayer(player);
    }
}
