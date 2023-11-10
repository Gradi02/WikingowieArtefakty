using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

        BuildingManager m = toBuild.getComponent<BuildingManager>();
        m.isFixed = false;
        m.SetPlacementMode(PlacementMode.Valid);
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
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if(toBuild.activeSelf) 
                    toBuild.SetActive(false);
            }
            
            else if (!toBuild.activeSelf)
                toBuild.SetActive(true);
            
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f, groundLayerMask))
            {
                if (!toBuild.activeSelf)
                    toBuild.SetActive(true);
                toBuild.transform.position = hit.point;
            }
        }
    }
}
