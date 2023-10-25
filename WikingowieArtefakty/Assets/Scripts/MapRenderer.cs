using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRenderer : MonoBehaviour
{
    private Camera cam;
    public MapGenerator gen;

    private void Start()
    {
        cam = this.gameObject.GetComponent<Camera>();
    }
    void Update()
    {
        if (cam != null)
        {
            foreach(GameObject g in gen.GetGroundBlocks())
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
        }
    }
}
