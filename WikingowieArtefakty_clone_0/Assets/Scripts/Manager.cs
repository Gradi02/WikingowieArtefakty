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
    GameObject player;
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

        ulong[] playersList = new ulong[playersCount];
        for(int i=0; i<playersCount; i++)
        {
            playersList[i] = NetworkManager.Singleton.ConnectedClientsIds[i];
        }
            
        AssignBarToPlayerClientRpc(playersCount, playersList);
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
    void AssignBarToPlayerClientRpc(int pc, ulong[] list)
    {
        for(int i = 0; i < pc; i++)
        {
            NetworkManager.Singleton.SpawnManager.SpawnedObjects[list[i]].GetComponent<PlayerInfo>().HealthBar = HealthBars[i];
            transform.Find("nick").GetComponent<TextMeshPro>().text = NetworkManager.Singleton.SpawnManager.SpawnedObjects[list[i]].name;
        }
    }

    private void Update()
    {
        foreach(GameObject g in HealthBars)
        {
            //transform.Find("nick").GetComponent<TextMeshPro>().text = 
            //g.GetComponent<Slider>().
        }
    }
}
