using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class LeaveGame : NetworkBehaviour
{
    public void CloseSession()
    {
        NetworkManager.Singleton.Shutdown();
    }

    //Wykona siê po wyjœciu z gry
    public override void OnNetworkDespawn()
    {
        if (!NetworkManager.Singleton.IsConnectedClient)
            Disconnected();

        base.OnNetworkDespawn();
    }

    public void Disconnected()
    {
        SceneManager.LoadScene(0);
    }
}
