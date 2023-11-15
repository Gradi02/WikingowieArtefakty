using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Core;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Networking.Transport.Relay;
using Unity.Netcode.Transports.UTP;
using TMPro;

public class NetworkInit : NetworkBehaviour
{

    [SerializeField] private GameObject hostButton;
    [SerializeField] private GameObject joinButton;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject inputCode;
    [SerializeField] private TextMeshProUGUI code;
    [SerializeField] private GameObject singleButton;
    [SerializeField] private GameObject multiButton;
    [SerializeField] private GameObject single;
    [SerializeField] private GameObject multi;
    [SerializeField] private MapGenerator generator;

    private string joincode;
    private Camera cam;
    public static int MaxPlayer = 2;

    private void Awake()
    {
        cam = Camera.main;
    }
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in" + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        generator.SpawnBase();
    }

    private async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(MaxPlayer);

            joincode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            code.text = joincode;

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);


            NetworkManager.Singleton.StartHost();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }


    private async void JoinRelay(string join_code)
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(join_code);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);


            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    public void HostGame()
    {
        CreateRelay();

        joinButton.SetActive(false);
        inputCode.SetActive(false);
        hostButton.SetActive(false);

        generator.GenerateWorld(true);
    }

    public void JoinGame()
    {
        joincode = code.text.Substring(0, 6);
        if (joincode.Length != 6)
        {
            Debug.Log("zly kod");
            return;
        }

        JoinRelay(joincode);
    }

    public override void OnNetworkDespawn()
    {
        if (!NetworkManager.Singleton.IsConnectedClient)
            Disconnected();

        base.OnNetworkDespawn();
    }

    public void Disconnected()
    {
        code.text = "";
        // po wyjœciu gracza
    }

    public void Back()
    {
        NetworkManager.Shutdown();

        joinButton.SetActive(true);
        inputCode.SetActive(true);
        hostButton.SetActive(true);

        singleButton.SetActive(true);
        multiButton.SetActive(true);

        single.SetActive(false);
        multi.SetActive(false);

        backButton.SetActive(false);
    }

    public void StartGameMulti()
    {
        singleButton.SetActive(false);
        multiButton.SetActive(false);
        backButton.SetActive(false);
        single.SetActive(false);
        multi.SetActive(false);

        cam.GetComponent<CameraFollow>().Offset = new Vector3(-3, 5, 0);

        //generator.GenerateWorld(true);
    }

    public void StartGameSingle()
    {
        singleButton.SetActive(false);
        multiButton.SetActive(false);
        backButton.SetActive(false);
        single.SetActive(false);
        multi.SetActive(false);

        generator.GenerateWorld(false);
        GameObject.FindGameObjectWithTag("manager").GetComponent<Manager>().SetPlayer();
        cam.GetComponent<CameraFollow>().Offset = new Vector3(-3, 5, 0);
    }

    public void Single()
    {
        single.SetActive(true);

        singleButton.SetActive(false);
        multiButton.SetActive(false);
        backButton.SetActive(true);
    }

    public void Multi()
    {
        multi.SetActive(true);

        singleButton.SetActive(false);
        multiButton.SetActive(false);
        backButton.SetActive(true);
    }
}
