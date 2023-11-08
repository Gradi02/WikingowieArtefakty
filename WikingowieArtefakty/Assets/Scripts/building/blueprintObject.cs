using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blueprintObject : MonoBehaviour
{
    //public int currentResource = [0,0];
    //public int requiredResource = [3,0];

    private GameObject buildingPrefab;
    private GameObject toBuild;

    private Camera mainCamera;
    private LayerMask groundLayerMask;
    
    private Ray ray;
    private RaycastHit hit;
    
    

    private void prepareBuilding()
    {
        if (toBuild) 
            Destroy(toBuild);

        toBuild = Instantiate(buildingPrefab);
        toBuild.SetActive(false);
    }
    
    private void setBuildingPrefab(GameObject prefab)
    {
        buildingPrefab = prefab;
        prepareBuilding();
    }
     
    public void Start()
    {
        buildingPrefab = null;
    }

    public void Update()
    {
        if (buildingPrefab != null)
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f, groundLayerMask))
            {
                if (!toBuild.activeSelf)
                    toBuild.activeSelf(true);
                toBuild.transform.position = hit.point;
            }
        }
    }
}
