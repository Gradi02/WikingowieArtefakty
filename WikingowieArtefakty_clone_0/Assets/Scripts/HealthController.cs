using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthController : MonoBehaviour
{
    public GameObject player;
    
    
    public void AssignPlayer(GameObject p)
    {
        player = p;
        player.GetComponent<PlayerStats>().SetHPinfo(gameObject);
        player.GetComponent<PlayerStats>().SyncBarNicksServerRpc();
    }

    private void FixedUpdate()
    {
    }

    public void SetName(string n)
    {
        Debug.Log(n);
        transform.Find("nick").GetComponent<TextMeshProUGUI>().text = n;
    }
}
