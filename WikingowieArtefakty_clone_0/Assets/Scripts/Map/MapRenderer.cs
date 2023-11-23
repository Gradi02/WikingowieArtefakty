using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRenderer : MonoBehaviour
{
    private MapGenerator gen;
    private Transform player;
    public bool showAll = true;
    private Camera cam;

    private void Start()
    {
        player = transform;
        cam = Camera.main;
        gen = GameObject.Find("GeneratorManager").GetComponent<MapGenerator>();
    }
    void Update()
    {
        if (showAll)
        {
            foreach (GameObject g in gen.GetGroundBlocks())
            {
                if (g != null)
                {
                    g.SetActive(true);
                }
            }

            foreach (GameObject g in gen.GetObjectsBlocks())
            {
                if (g != null)
                {
                    g.SetActive(true);
                }
            }

            return;
        }

       /* if (cam != null)
        {
            foreach (GameObject g in gen.GetGroundBlocks())
            {
                if (g != null)
                {
                    if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(cam), g.GetComponent<MeshRenderer>().bounds))
                    {
                        g.SetActive(true);
                    }
                    else
                    {
                        g.SetActive(false);
                    }
                }
            }
        } */
            if (player != null)
            {
                foreach (GameObject g in gen.GetObjectsBlocks())
                {
                    if (g != null)
                    {
                        if (Vector3.Distance(g.transform.position, player.position + new Vector3(5, 0, 0)) >= 20)
                        {
                            g.SetActive(false);
                        }
                        else
                        {
                            g.SetActive(true);
                        }
                    }
                }

                foreach (GameObject g in gen.GetGroundBlocks())
                {
                    if (g != null)
                    {
                        if (Vector3.Distance(g.transform.position, player.position + new Vector3(5, 0, 0)) >= 20)
                        {
                            g.SetActive(false);
                        }
                        else
                        {
                            g.SetActive(true);
                        }
                    }
                }
        }
        
    }
}
