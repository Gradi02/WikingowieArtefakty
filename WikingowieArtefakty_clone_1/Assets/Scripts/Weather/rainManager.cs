using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rainManager : MonoBehaviour
{
    [SerializeField] private GameObject rainObj;
    private bool rainStatus = false;

    public void SetRainActivity(bool setin)
    {
        rainStatus = setin;
    }

    private void Update()
    {
        if(rainStatus && !rainObj.activeSelf)
        {
            rainObj.SetActive(true);
        }
        else if(!rainStatus && rainObj.activeSelf)
        {
            rainObj.SetActive(false);
        }
    }
}
