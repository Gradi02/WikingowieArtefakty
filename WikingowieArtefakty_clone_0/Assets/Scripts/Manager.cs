using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

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
        AssignBarToPlayerClientRpc();
    }

    [ClientRpc]
    void StartGameClientRpc(int pc)
    {
        for (int i=0; i<playersCount; i++)
        {
            HealthBars[i].SetActive(true);
        }
    }

    [ClientRpc]
    void AssignBarToPlayerClientRpc()
    {
        for(int i = 0; i < playersCount; i++)
        {
            NetworkManager.Singleton.ConnectedClientsList[i].PlayerObject.GetComponent<PlayerInfo>().HealthBar = HealthBars[i];
        }
    }






    private void SetPlayer()
    {
        player = Instantiate(playerPref, middle + new Vector3(-5,0.75f,0), Quaternion.identity);
        //player.GetComponent<PlayerMovement>().SetStartPosition(middle);

        cam.GetComponent<CameraFollow>().Target = player.transform;
    }

    private void ResetCameraSmoothTime()
    {
        cam.GetComponent<CameraFollow>().ResetSmooth();
        player.GetComponent<PlayerMovement>().enabled = true;
    }
}
