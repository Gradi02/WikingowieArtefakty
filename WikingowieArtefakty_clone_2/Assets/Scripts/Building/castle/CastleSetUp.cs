using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CastleSetUp : NetworkBehaviour
{
    public float visitDistance;

    private CameraFollow cam;
    private bool visited = false;

    private void Start()
    {
        cam = Camera.main.GetComponent<CameraFollow>();
    }
    void Update()
    {
        if (!IsOwner) return;

        Transform player = NetworkManager.LocalClient.PlayerObject.transform;

        if (!visited)
        {
            if (Vector3.Distance(player.position, transform.position) <= visitDistance)
            {
                visited = true;
                cam.SmoothTime = 1;
                cam.Target = this.gameObject.transform;
                Invoke(nameof(ResetCamTarget), 3);
            }
        }
    }

    private void ResetCamTarget()
    {
        cam.Target = NetworkManager.LocalClient.PlayerObject.transform;
        cam.ResetSmooth();
    }
}
