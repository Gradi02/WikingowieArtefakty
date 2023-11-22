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
    [SerializeField] private GameObject inputCode;
    [SerializeField] private TextMeshProUGUI code;
    [SerializeField] private GameObject mapButton;

    private string joincode;
    public static int MaxPlayer = 2;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in" + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
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
        mapButton.SetActive(true);
    }

    public void JoinGame()
    {
        //joincode = code.text.Substring(0, 6);
        joincode = inputCode.GetComponent<TMP_InputField>().text;
        if (joincode.Length != 6)
        {
            Debug.Log("zly kod");
            return;
        }

        JoinRelay(joincode);
        Debug.Log(joincode);
    }
}
