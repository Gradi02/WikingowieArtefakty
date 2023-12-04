using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class MapRenderer : NetworkBehaviour
{
    private MapGenerator gen;
    private Transform player;
    public bool showAll = false;
    private Camera cam;

    private void Start()
    {
        gen = GameObject.Find("GeneratorManager").GetComponent<MapGenerator>();

        if (!IsOwner) return;
        player = transform;
        cam = Camera.main;
    }
    void Update()
    {
        if (showAll)
        {
            foreach (Transform g in gen.transform)
            {
                if (g.gameObject != null)
                {
                    g.gameObject.SetActive(true);
                }
            }

            return;
        }

        if (player != null)
        {
            foreach (Transform g in gen.transform)
            {
                if (g.gameObject != null)
                {
                    if (Vector3.Distance(g.transform.position, player.position + new Vector3(5, 0, 0)) >= 20)
                    {
                        g.gameObject.SetActive(false);
                    }
                    else
                    {
                        g.gameObject.SetActive(true);
                    }
                }
            }
        }
        
    }
}
