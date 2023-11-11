using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject playerPref;
    public Camera cam;
    GameObject player;
    private Vector3 middle = new();

    void Start()
    {
        Invoke(nameof(SetPlayer), 1);
        Invoke(nameof(ResetCameraSmoothTime), 10);
    }

    public void SetMiddle(Vector3 md)
    {
        middle = md;
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
