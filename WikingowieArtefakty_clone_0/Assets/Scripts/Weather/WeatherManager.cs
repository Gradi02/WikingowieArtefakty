using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class WeatherManager : NetworkBehaviour
{
    [SerializeField] private GameObject generator;
    [SerializeField] private TimeManager timeManager;

    [ServerRpc]
    public void SetRainServerRpc(bool s)
    {
        SetRainClientRpc(s);
    }

    [ClientRpc]
    public void SetRainClientRpc(bool s)
    {
        foreach(rainManager c in generator.GetComponentsInChildren<rainManager>())
        {
            c.SetRainActivity(s);
        }

        timeManager.lockFog = s;

        if (s)
        {
            if (!timeManager.isFog) StartCoroutine(timeManager.FogON());
        }
        else
        {
            if (timeManager.isFog) StartCoroutine(timeManager.FogOFF());
        }
    }
}
