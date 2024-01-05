using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class MapRenderer : NetworkBehaviour
{
    private MapGenerator gen;
    public bool showAll = false;
    private Camera cam;

    private void Start()
    {
        gen = GameObject.Find("GeneratorManager").GetComponent<MapGenerator>();

        cam = GetComponent<Camera>();
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

        if (cam != null)
        {
            foreach (Transform g in gen.transform)
            {
                if (g.gameObject != null)
                {

                    //if(Vector3.SqrMagnitude(g.transform.position - (cam.transform.position - GetComponent<CameraFollow>().Offset)) >= 625)
                    if (Vector3.Distance(g.transform.position, cam.transform.position - GetComponent<CameraFollow>().Offset) >= 35)
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
